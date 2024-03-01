using System.Diagnostics.CodeAnalysis;
using Application.Models.Collections.Generic.Extensions;

namespace Application.Models.Cryptography
{
	public class CertificateConstructionHelper : ICertificateConstructionHelper
	{
		#region Methods

		public virtual ICertificateOptions CreateCertificateOptions(CertificateConstructionOptions certificateConstructionOptions, ICertificate issuer = null)
		{
			ArgumentNullException.ThrowIfNull(certificateConstructionOptions);

			var certificateConstructionOptionsClone = certificateConstructionOptions.Clone();
			var certificateOptions = new CertificateOptions
			{
				CertificateAuthority = certificateConstructionOptionsClone.CertificateAuthority,
				Issuer = issuer,
				NotAfter = certificateConstructionOptionsClone.NotAfter,
				NotBefore = certificateConstructionOptionsClone.NotBefore,
				SerialNumberSize = certificateConstructionOptionsClone.SerialNumberSize,
				Subject = certificateConstructionOptionsClone.Subject,
				SubjectAlternativeName = certificateConstructionOptionsClone.SubjectAlternativeName
			};

			if(certificateConstructionOptionsClone.EnhancedKeyUsage != null)
				certificateOptions.EnhancedKeyUsage = certificateConstructionOptionsClone.EnhancedKeyUsage.Value;

			if(certificateConstructionOptionsClone.HashAlgorithm != null)
				certificateOptions.HashAlgorithm = certificateConstructionOptionsClone.HashAlgorithm.Value;

			if(certificateConstructionOptionsClone.KeyUsage != null)
				certificateOptions.KeyUsage = certificateConstructionOptionsClone.KeyUsage.Value;

			return certificateOptions;
		}

		[SuppressMessage("Maintainability", "CA1502:Avoid excessive complexity")]
		public virtual void SetDefaults(CertificateConstructionOptions certificateConstructionOptions, IEnumerable<CertificateConstructionOptions> defaults)
		{
			ArgumentNullException.ThrowIfNull(certificateConstructionOptions);
			ArgumentNullException.ThrowIfNull(defaults);

			foreach(var @default in defaults)
			{
				if(@default == null)
					continue;

				var defaultClone = @default.Clone();

				if(certificateConstructionOptions.AsymmetricAlgorithm == null && defaultClone.AsymmetricAlgorithm != null)
					certificateConstructionOptions.AsymmetricAlgorithm = defaultClone.AsymmetricAlgorithm;

				if(certificateConstructionOptions.CertificateAuthority == null && defaultClone.CertificateAuthority != null)
					certificateConstructionOptions.CertificateAuthority = defaultClone.CertificateAuthority;

				if(certificateConstructionOptions.EnhancedKeyUsage == null && defaultClone.EnhancedKeyUsage != null)
					certificateConstructionOptions.EnhancedKeyUsage = defaultClone.EnhancedKeyUsage;

				if(certificateConstructionOptions.HashAlgorithm == null && defaultClone.HashAlgorithm != null)
					certificateConstructionOptions.HashAlgorithm = defaultClone.HashAlgorithm;

				if(certificateConstructionOptions.KeyUsage == null && defaultClone.KeyUsage != null)
					certificateConstructionOptions.KeyUsage = defaultClone.KeyUsage;

				if(certificateConstructionOptions.NotAfter == null && defaultClone.NotAfter != null)
					certificateConstructionOptions.NotAfter = defaultClone.NotAfter;

				if(certificateConstructionOptions.NotBefore == null && defaultClone.NotBefore != null)
					certificateConstructionOptions.NotBefore = defaultClone.NotBefore;

				if(certificateConstructionOptions.SerialNumberSize == null && defaultClone.SerialNumberSize != null)
					certificateConstructionOptions.SerialNumberSize = defaultClone.SerialNumberSize;

				if(certificateConstructionOptions.Subject == null && defaultClone.Subject != null)
					certificateConstructionOptions.Subject = defaultClone.Subject;

				if(defaultClone.SubjectAlternativeName != null)
				{
					if(certificateConstructionOptions.SubjectAlternativeName == null)
					{
						certificateConstructionOptions.SubjectAlternativeName = defaultClone.SubjectAlternativeName;
					}
					else
					{
						if(!certificateConstructionOptions.SubjectAlternativeName.DnsNames.Any())
							certificateConstructionOptions.SubjectAlternativeName.DnsNames.Add(defaultClone.SubjectAlternativeName.DnsNames);

						if(!certificateConstructionOptions.SubjectAlternativeName.EmailAddresses.Any())
							certificateConstructionOptions.SubjectAlternativeName.EmailAddresses.Add(defaultClone.SubjectAlternativeName.EmailAddresses);

						if(!certificateConstructionOptions.SubjectAlternativeName.IpAddresses.Any())
							certificateConstructionOptions.SubjectAlternativeName.IpAddresses.Add(defaultClone.SubjectAlternativeName.IpAddresses);

						if(!certificateConstructionOptions.SubjectAlternativeName.Uris.Any())
							certificateConstructionOptions.SubjectAlternativeName.Uris.Add(defaultClone.SubjectAlternativeName.Uris);

						if(!certificateConstructionOptions.SubjectAlternativeName.UserPrincipalNames.Any())
							certificateConstructionOptions.SubjectAlternativeName.UserPrincipalNames.Add(defaultClone.SubjectAlternativeName.UserPrincipalNames);
					}
				}
			}
		}

		#endregion
	}
}