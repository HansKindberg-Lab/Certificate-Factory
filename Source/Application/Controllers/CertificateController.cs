using System.Net;
using System.Security.Cryptography.X509Certificates;
using Application.Models;
using Application.Models.Collections.Generic.Extensions;
using Application.Models.Cryptography;
using Application.Models.Views.Certificate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Controllers
{
	public class CertificateController(IFacade facade) : BasicCertificateController<CertificateForm>(facade)
	{
		#region Methods

		protected internal override ICertificate CreateCertificate(IAsymmetricAlgorithmOptions asymmetricAlgorithmOptions, CertificateForm form, ICertificate issuer)
		{
			ArgumentNullException.ThrowIfNull(form);

			var certificateOptions = new CertificateOptions
			{
				Issuer = issuer,
				NotAfter = this.ResolveDate(form.NotAfter),
				NotBefore = this.ResolveDate(form.NotBefore),
				Subject = form.Subject,
			};

			if(form.CertificateAuthorityEnabled)
			{
				certificateOptions.CertificateAuthority = new CertificateAuthorityOptions
				{
					PathLengthConstraint = form.PathLengthConstraint
				};
			}

			if(form.EnhancedKeyUsage.Any())
				certificateOptions.EnhancedKeyUsage = Enum.Parse<EnhancedKeyUsage>(form.EnhancedKeyUsage.ToCommaSeparatedString(), true);

			certificateOptions.HashAlgorithm = form.HashAlgorithm;

			if(form.KeyUsage.Any())
				certificateOptions.KeyUsage = Enum.Parse<X509KeyUsageFlags>(form.KeyUsage.ToCommaSeparatedString(), true);

			var dnsNames = this.SplitOnLineBreaks(form.DnsNames).ToArray();
			var emailAddresses = this.SplitOnLineBreaks(form.EmailAddresses).ToArray();
			var ipAddresses = this.SplitOnLineBreaks(form.IpAddresses).ToArray();
			var uris = this.SplitOnLineBreaks(form.Uris).ToArray();
			var userPrincipalNames = this.SplitOnLineBreaks(form.UserPrincipalNames).ToArray();

			if(dnsNames.Any() || emailAddresses.Any() || ipAddresses.Any() || uris.Any() || userPrincipalNames.Any())
				certificateOptions.SubjectAlternativeName = new SubjectAlternativeNameOptions();

			foreach(var dnsName in dnsNames)
			{
				certificateOptions.SubjectAlternativeName.DnsNames.Add(dnsName);
			}

			foreach(var emailAddress in emailAddresses)
			{
				certificateOptions.SubjectAlternativeName.EmailAddresses.Add(emailAddress);
			}

			foreach(var ipAddress in ipAddresses)
			{
				certificateOptions.SubjectAlternativeName.IpAddresses.Add(IPAddress.Parse(ipAddress));
			}

			foreach(var uri in uris)
			{
				certificateOptions.SubjectAlternativeName.Uris.Add(new Uri(uri, UriKind.Absolute));
			}

			foreach(var userPrincipalName in userPrincipalNames)
			{
				certificateOptions.SubjectAlternativeName.UserPrincipalNames.Add(userPrincipalName);
			}

			return this.Facade.CertificateFactory.Create(asymmetricAlgorithmOptions, certificateOptions);
		}

		public virtual async Task<IActionResult> Index()
		{
			var model = new CertificateViewModel();

			this.PopulateFormLists(model.Form);

			return await Task.FromResult(this.View(model));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public virtual async Task<IActionResult> Index(CertificateForm form)
		{
			this.PopulateFormLists(form);

			var actionResult = this.CertificateArchiveFile("certificate", form) ?? this.View(new CertificateViewModel { Form = form });

			return await Task.FromResult(actionResult);
		}

		protected internal virtual void PopulateEnhancedKeyUsageList(CertificateForm form)
		{
			ArgumentNullException.ThrowIfNull(form);

			if(!form.EnhancedKeyUsage.Any() || !Enum.TryParse(form.EnhancedKeyUsage.ToCommaSeparatedString(), true, out EnhancedKeyUsage selectedEnhancedKeyUsage))
				selectedEnhancedKeyUsage = default;

			foreach(var enhancedKeyUsage in Enum.GetValues<EnhancedKeyUsage>().Where(enhancedKeyUsage => enhancedKeyUsage > 0).OrderBy(enhancedKeyUsage => enhancedKeyUsage.ToString()))
			{
				var selected = selectedEnhancedKeyUsage.HasFlag(enhancedKeyUsage);
				var value = enhancedKeyUsage.ToString();

				form.EnhancedKeyUsageList.Add(new SelectListItem(value, value, selected));
			}
		}

		protected internal override void PopulateFormLists(CertificateForm form)
		{
			base.PopulateFormLists(form);

			this.PopulateEnhancedKeyUsageList(form);
			this.PopulateHashAlgorithmList(form);
			this.PopulateKeyUsageList(form);
		}

		protected internal virtual void PopulateHashAlgorithmList(CertificateForm form)
		{
			ArgumentNullException.ThrowIfNull(form);

			// MD5 and SHA1 should not be used: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithmname#remarks
			foreach(var hashAlgorithm in Enum.GetValues<HashAlgorithm>().Where(hashAlgorithm => hashAlgorithm > HashAlgorithm.Sha1).OrderBy(hashAlgorithm => hashAlgorithm.ToString()))
			{
				var selected = form.HashAlgorithm == hashAlgorithm;
				var value = hashAlgorithm.ToString();

				form.HashAlgorithmList.Add(new SelectListItem(value, value, selected));
			}
		}

		protected internal virtual void PopulateKeyUsageList(CertificateForm form)
		{
			ArgumentNullException.ThrowIfNull(form);

			if(!form.KeyUsage.Any() || !Enum.TryParse(form.KeyUsage.ToCommaSeparatedString(), true, out X509KeyUsageFlags selectedKeyUsage))
				selectedKeyUsage = default;

			foreach(var keyUsage in Enum.GetValues<X509KeyUsageFlags>().Where(keyUsage => keyUsage > 0).OrderBy(keyUsage => keyUsage.ToString()))
			{
				var selected = selectedKeyUsage.HasFlag(keyUsage);
				var value = keyUsage.ToString();

				form.KeyUsageList.Add(new SelectListItem(value, value, selected));
			}
		}

		protected internal virtual DateTimeOffset? ResolveDate(DateTimeOffset? certificateDate)
		{
			if(certificateDate == null)
				return null;

			// The value coming from the UI will be a local date, <input type="date" .... />.
			return new DateTimeOffset(certificateDate.Value.Year, certificateDate.Value.Month, certificateDate.Value.Day, certificateDate.Value.Hour, certificateDate.Value.Minute, certificateDate.Value.Second, TimeSpan.Zero);
		}

		protected internal override void Validate(CertificateForm form)
		{
			base.Validate(form);

			this.ValidateEnhancedKeyUsage(form);
			this.ValidateIpAddresses(form);
			this.ValidateKeyUsage(form);
			this.ValidateUris(form);
		}

		protected internal virtual void ValidateEnhancedKeyUsage(CertificateForm form)
		{
			if(form == null || !form.EnhancedKeyUsage.Any())
				return;

			var enhancedKeyUsage = form.EnhancedKeyUsage.ToCommaSeparatedString();

			if(Enum.TryParse(enhancedKeyUsage, true, out EnhancedKeyUsage _))
				return;

			const string enhancedKeyUsageKey = nameof(CertificateForm.EnhancedKeyUsage);
			this.ModelState.AddModelError(enhancedKeyUsageKey, $"\"{enhancedKeyUsageKey}\": the value \"{enhancedKeyUsage}\" is invalid.");
		}

		protected internal virtual void ValidateIpAddresses(CertificateForm form)
		{
			var ipAddresses = this.SplitOnLineBreaks(form?.IpAddresses).ToArray();

			if(!ipAddresses.Any())
				return;

			const string ipAddressesKey = nameof(CertificateForm.IpAddresses);

			foreach(var ipAddress in ipAddresses)
			{
				if(!IPAddress.TryParse(ipAddress, out _))
					this.ModelState.AddModelError(ipAddressesKey, $"\"{ipAddressesKey}\": the value \"{ipAddress}\" is an invalid ip-address.");
			}
		}

		protected internal virtual void ValidateKeyUsage(CertificateForm form)
		{
			if(form == null || !form.KeyUsage.Any())
				return;

			var keyUsage = form.KeyUsage.ToCommaSeparatedString();

			if(Enum.TryParse(keyUsage, true, out X509KeyUsageFlags _))
				return;

			const string keyUsageKey = nameof(CertificateForm.KeyUsage);
			this.ModelState.AddModelError(keyUsageKey, $"\"{keyUsageKey}\": the value \"{keyUsage}\" is invalid.");
		}

		protected internal virtual void ValidateUris(CertificateForm form)
		{
			var uris = this.SplitOnLineBreaks(form?.Uris).ToArray();

			if(!uris.Any())
				return;

			const string urisKey = nameof(CertificateForm.Uris);

			foreach(var uri in uris)
			{
				if(!Uri.TryCreate(uri, UriKind.Absolute, out _))
					this.ModelState.AddModelError(urisKey, $"\"{urisKey}\": the value \"{uri}\" is an invalid uri.");
			}
		}

		#endregion
	}
}