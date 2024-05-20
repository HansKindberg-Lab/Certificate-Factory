using System.Security.Cryptography.X509Certificates;

namespace Application.Models.Cryptography
{
	/// <inheritdoc />
	public interface ICertificateOptions : ICloneable<ICertificateOptions>
	{
		#region Properties

		IAuthorityInformationAccessOptions AuthorityInformationAccess { get; set; }
		ICertificateAuthorityOptions CertificateAuthority { get; set; }
		ICrlDistributionPointOptions CrlDistributionPoint { get; set; }
		EnhancedKeyUsage EnhancedKeyUsage { get; set; }
		HashAlgorithm HashAlgorithm { get; set; }
		ICertificate Issuer { get; set; }
		X509KeyUsageFlags KeyUsage { get; set; }
		DateTimeOffset? NotAfter { get; set; }
		DateTimeOffset? NotBefore { get; set; }
		ISerialNumberOptions SerialNumber { get; set; }
		string Subject { get; set; }
		ISubjectAlternativeNameOptions SubjectAlternativeName { get; set; }

		#endregion
	}
}