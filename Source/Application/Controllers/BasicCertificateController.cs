using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Application.Models;
using Application.Models.Cryptography;
using Application.Models.Views.Shared.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Controllers
{
	// ReSharper disable StaticMemberInGenericType
	public abstract class BasicCertificateController<TForm> : SiteController where TForm : BasicCertificateForm
	{
		#region Fields

		private static IDictionary<string, AsymmetricAlgorithmInformation> _asymmetricAlgorithmDictionary;
		private static string _asymmetricAlgorithmDictionaryDefaultKey;
		private static readonly object _asymmetricAlgorithmDictionaryLock = new();

		#endregion

		#region Constructors

		protected BasicCertificateController(IFacade facade) : base(facade) { }

		#endregion

		#region Properties

		[SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code")]
		protected internal virtual IDictionary<string, AsymmetricAlgorithmInformation> AsymmetricAlgorithmDictionary
		{
			get
			{
				// ReSharper disable InvertIf
				if(_asymmetricAlgorithmDictionary == null)
				{
					lock(_asymmetricAlgorithmDictionaryLock)
					{
						if(_asymmetricAlgorithmDictionary == null)
						{
							var asymmetricAlgorithmDictionary = new Dictionary<string, AsymmetricAlgorithmInformation>(StringComparer.OrdinalIgnoreCase);

							const string ecdsaName = nameof(ECDsa);

							asymmetricAlgorithmDictionary.Add(ecdsaName, new AsymmetricAlgorithmInformation
							{
								Options = new EcdsaOptions(),
								Text = $"{ecdsaName} (default)"
							});

							foreach(var ellipticCurve in Enum.GetValues<EllipticCurve>())
							{
								asymmetricAlgorithmDictionary.Add($"{ecdsaName}:{ellipticCurve}", new AsymmetricAlgorithmInformation
								{
									Options = new EcdsaOptions { EllipticCurve = ellipticCurve },
									Text = $"{ecdsaName} - {ellipticCurve}"
								});
							}

							const string rsaName = nameof(RSA);

							foreach(var keySize in new[] { 512, 1024, 2048, 4096 })
							{
								foreach(var signaturePadding in Enum.GetValues<RSASignaturePaddingMode>())
								{
									asymmetricAlgorithmDictionary.Add($"{rsaName}:{keySize}:{signaturePadding}", new AsymmetricAlgorithmInformation
									{
										Options = new RsaOptions { KeySize = keySize, SignaturePadding = signaturePadding },
										Text = $"{rsaName} - {keySize} - {signaturePadding}"
									});
								}
							}

							_asymmetricAlgorithmDictionary = asymmetricAlgorithmDictionary;
						}
					}
				}
				// ReSharper restore InvertIf

				return _asymmetricAlgorithmDictionary;
			}
		}

		protected internal virtual string AsymmetricAlgorithmDictionaryDefaultKey
		{
			get
			{
				// ReSharper disable InvertIf
				if(_asymmetricAlgorithmDictionaryDefaultKey == null)
				{
					var asymmetricAlgorithmDictionaryDefault = this.AsymmetricAlgorithmDictionary.FirstOrDefault(item => item.Value.Options is RsaOptions { KeySize: 2048, SignaturePadding: RSASignaturePaddingMode.Pkcs1 });

					if(asymmetricAlgorithmDictionaryDefault.Equals(default(KeyValuePair<string, AsymmetricAlgorithmInformation>)))
						throw new InvalidOperationException("Invalid asymmetric algorithm setup.");

					_asymmetricAlgorithmDictionaryDefaultKey = asymmetricAlgorithmDictionaryDefault.Key;
				}
				// ReSharper restore InvertIf

				return _asymmetricAlgorithmDictionaryDefaultKey;
			}
		}

		#endregion

		#region Methods

		[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
		[SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates")]
		[SuppressMessage("Usage", "CA2254:Template should be a static expression")]
		protected internal virtual IActionResult CertificateArchiveFile(string certificateTypeLabel, TForm form)
		{
			if(certificateTypeLabel == null)
				throw new ArgumentNullException(nameof(certificateTypeLabel));

			if(form == null)
				throw new ArgumentNullException(nameof(form));

			this.Validate(form);

			if(!this.ModelState.IsValid)
			{
				this.SortModelState();
				return null;
			}

			try
			{
				if(!this.AsymmetricAlgorithmDictionary.TryGetValue(form.AsymmetricAlgorithm, out var asymmetricAlgorithmInformation))
					throw new InvalidOperationException($"Could not find asymmetric algorithm \"{form.AsymmetricAlgorithm}\".");

				var asymmetricAlgorithmOptions = asymmetricAlgorithmInformation.Options.Clone();

				using(var issuer = this.GetIssuer(form as IIssuedCertificateForm))
				{
					using(var certificate = this.CreateCertificate(asymmetricAlgorithmOptions, form, issuer))
					{
						var archive = this.Facade.ArchiveFactory.Create(certificate, form.Password);

						return this.File(archive.Bytes.ToArray(), archive.ContentType, this.Facade.FileNameResolver.Resolve($"{form.Subject}.Certificate.zip"));
					}
				}
			}
			catch(Exception exception)
			{
				var message = $"Could not create {certificateTypeLabel}.";

				if(this.Logger.IsEnabled(LogLevel.Error))
					this.Logger.LogError(exception, message);

				this.ModelState.AddModelError("Exception", exception.ToString());

				return null;
			}
		}

		protected internal abstract ICertificate CreateCertificate(IAsymmetricAlgorithmOptions asymmetricAlgorithmOptions, TForm form, ICertificate issuer);

		protected internal virtual ICertificate GetIssuer(IIssuedCertificateForm form)
		{
			ICertificate issuer = null;
			var thumbprint = form?.Issuer;

			// ReSharper disable InvertIf
			if(!string.IsNullOrWhiteSpace(thumbprint))
			{
				var certificates = this.Facade.CertificateStore.Certificates().ToList();

				for(var i = certificates.Count - 1; i >= 0; i--)
				{
					var certificate = certificates[i];

					if(!string.Equals(certificate.Thumbprint, thumbprint, StringComparison.OrdinalIgnoreCase))
						continue;

					issuer = certificate;
					certificates.RemoveAt(i);
					break;
				}

				foreach(var certificate in certificates)
				{
					certificate.Dispose();
				}
			}
			// ReSharper restore InvertIf

			return issuer;
		}

		protected internal virtual void PopulateAsymmetricAlgorithmList(IAsymmetricAlgorithmForm form)
		{
			if(form == null)
				throw new ArgumentNullException(nameof(form));

			foreach(var (key, information) in this.AsymmetricAlgorithmDictionary)
			{
				var selected = string.Equals(form.AsymmetricAlgorithm, key, StringComparison.OrdinalIgnoreCase);

				form.AsymmetricAlgorithmList.Add(new SelectListItem(information.Text, key, selected));
			}

			// ReSharper disable InvertIf
			if(!form.AsymmetricAlgorithmList.Any(asymmetricAlgorithm => asymmetricAlgorithm.Selected))
			{
				try
				{
					form.AsymmetricAlgorithmList.First(asymmetricAlgorithm => string.Equals(this.AsymmetricAlgorithmDictionaryDefaultKey, asymmetricAlgorithm.Value, StringComparison.OrdinalIgnoreCase)).Selected = true;
				}
				catch(Exception exception)
				{
					throw new InvalidOperationException("Invalid asymmetric algorithm setup.", exception);
				}
			}
			// ReSharper restore InvertIf
		}

		protected internal virtual void PopulateFormLists(TForm form)
		{
			if(form == null)
				throw new ArgumentNullException(nameof(form));

			this.PopulateAsymmetricAlgorithmList(form);

			if(form is IIssuedCertificateForm issuedCertificateForm)
				this.PopulateIssuerList(issuedCertificateForm);
		}

		protected internal virtual void PopulateIssuerList(IIssuedCertificateForm form)
		{
			if(form == null)
				throw new ArgumentNullException(nameof(form));

			form.IssuerList.Add(new SelectListItem(null, null));

			foreach(var certificate in this.Facade.CertificateStore.Certificates().OrderBy(certificate => certificate.Subject))
			{
				using(certificate)
				{
					var selected = string.Equals(form.Issuer, certificate.Thumbprint, StringComparison.OrdinalIgnoreCase);

					form.IssuerList.Add(new SelectListItem(certificate.Subject, certificate.Thumbprint, selected));
				}
			}
		}

		protected internal virtual IEnumerable<string> SplitOnLineBreaks(string value)
		{
			if(string.IsNullOrEmpty(value))
				return Enumerable.Empty<string>();

			const char lineBreak = '\n';

			return value.Replace(Environment.NewLine, lineBreak.ToString(), StringComparison.OrdinalIgnoreCase).Split(lineBreak);
		}

		protected internal virtual void Validate(TForm form)
		{
			this.ValidateDefault(form);
		}

		protected internal virtual void ValidateDefault(BasicCertificateForm form)
		{
			this.ValidateSubject(form);
			this.ValidateIssuer(form);
		}

		protected internal virtual void ValidateIssuer(BasicCertificateForm form)
		{
			var asymmetricAlgorithm = form?.AsymmetricAlgorithm;

			if(string.IsNullOrEmpty(asymmetricAlgorithm))
				return;

			if(form is not IIssuedCertificateForm issuedCertificateForm)
				return;

			using(var issuer = this.GetIssuer(issuedCertificateForm))
			{
				if(issuer == null)
					return;

				if(!this.AsymmetricAlgorithmDictionary.TryGetValue(asymmetricAlgorithm, out var asymmetricAlgorithmInformation))
					return;

				const string issuerKey = nameof(IIssuedCertificateForm.Issuer);

				// ReSharper disable ConvertIfStatementToSwitchStatement
				if(asymmetricAlgorithmInformation.Options is RsaOptions && !"1.2.840.113549.1.1.1".Equals(issuer.KeyAlgorithm, StringComparison.OrdinalIgnoreCase))
					this.ModelState.AddModelError(issuerKey, $"\"{issuerKey}\" is invalid. The issuer must have a \"RSA\" key algorithm.");
				// ReSharper restore ConvertIfStatementToSwitchStatement

				if(asymmetricAlgorithmInformation.Options is EcdsaOptions && !"1.2.840.10045.2.1".Equals(issuer.KeyAlgorithm, StringComparison.OrdinalIgnoreCase))
					this.ModelState.AddModelError(issuerKey, $"\"{issuerKey}\" is invalid. The issuer must have a \"ECDsa\" key algorithm. The same type as \"Asymmetric algorithm\".");
			}
		}

		[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
		protected internal virtual void ValidateSubject(BasicCertificateForm form)
		{
			var subject = form?.Subject;

			if(string.IsNullOrEmpty(subject))
				return;

			try
			{
				var _ = new X500DistinguishedName(subject);
			}
			catch
			{
				const string subjectKey = nameof(BasicCertificateForm.Subject);

				this.ModelState.AddModelError(subjectKey, $"\"{subjectKey}\" has an invalid value. The value is not a valid X500 distinguished name.");
			}
		}

		#endregion
	}
	// ReSharper restore StaticMemberInGenericType
}