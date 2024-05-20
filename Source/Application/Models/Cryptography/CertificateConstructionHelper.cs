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
				AuthorityInformationAccess = certificateConstructionOptionsClone.AuthorityInformationAccess,
				CertificateAuthority = certificateConstructionOptionsClone.CertificateAuthority,
				CrlDistributionPoint = certificateConstructionOptionsClone.CrlDistributionPoint,
				Issuer = issuer,
				NotAfter = certificateConstructionOptionsClone.NotAfter,
				NotBefore = certificateConstructionOptionsClone.NotBefore,
				SerialNumber = certificateConstructionOptionsClone.SerialNumber,
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

				if(defaultClone != null)
				{
					if(certificateConstructionOptions.AuthorityInformationAccess == null)
					{
						certificateConstructionOptions.AuthorityInformationAccess = defaultClone.AuthorityInformationAccess;
					}
					else
					{
						if(!certificateConstructionOptions.AuthorityInformationAccess.CaIssuerUris.Any())
							certificateConstructionOptions.AuthorityInformationAccess.CaIssuerUris.Add(defaultClone.AuthorityInformationAccess.CaIssuerUris);

						if(!certificateConstructionOptions.AuthorityInformationAccess.OcspUris.Any())
							certificateConstructionOptions.AuthorityInformationAccess.OcspUris.Add(defaultClone.AuthorityInformationAccess.OcspUris);
					}
				}

				if(certificateConstructionOptions.CertificateAuthority == null && defaultClone.CertificateAuthority != null)
					certificateConstructionOptions.CertificateAuthority = defaultClone.CertificateAuthority;

				if(defaultClone != null)
				{
					if(certificateConstructionOptions.CrlDistributionPoint == null)
					{
						certificateConstructionOptions.CrlDistributionPoint = defaultClone.CrlDistributionPoint;
					}
					else
					{
						if(!certificateConstructionOptions.CrlDistributionPoint.Uris.Any())
							certificateConstructionOptions.CrlDistributionPoint.Uris.Add(defaultClone.CrlDistributionPoint.Uris);
					}
				}

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

				if(certificateConstructionOptions.SerialNumber == null && defaultClone.SerialNumber != null)
					certificateConstructionOptions.SerialNumber = defaultClone.SerialNumber;

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