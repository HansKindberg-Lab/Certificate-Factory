using System.Security.Cryptography.X509Certificates;

namespace Application.Models.Cryptography.Storing.Configuration
{
	public class CertificateStoreOptions
	{
		#region Properties

		public virtual IList<StoreCertificateOptions> Certificates { get; } = [];

		public virtual IDictionary<string, CertificateOptions> Templates { get; } = new Dictionary<string, CertificateOptions>(StringComparer.OrdinalIgnoreCase)
		{
			{
				"ClientCertificate", new CertificateOptions
				{
					EnhancedKeyUsage = EnhancedKeyUsage.ClientAuthentication,
					KeyUsage = X509KeyUsageFlags.DigitalSignature
				}
			},
			{
				"IntermediateCertificate", new CertificateOptions
				{
					CertificateAuthority = new CertificateAuthorityOptions
					{
						PathLengthConstraint = 0
					},
					KeyUsage = X509KeyUsageFlags.KeyCertSign
				}
			},
			{
				"RootCertificate", new CertificateOptions
				{
					CertificateAuthority = new CertificateAuthorityOptions(),
					KeyUsage = X509KeyUsageFlags.KeyCertSign
				}
			},
			{
				"TlsCertificate", new CertificateOptions
				{
					EnhancedKeyUsage = EnhancedKeyUsage.ServerAuthentication,
					KeyUsage = X509KeyUsageFlags.DigitalSignature
				}
			}
		};

		#endregion
	}
}