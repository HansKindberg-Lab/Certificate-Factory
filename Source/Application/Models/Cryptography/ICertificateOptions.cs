using System.Security.Cryptography.X509Certificates;

namespace Application.Models.Cryptography
{
	/// <inheritdoc />
	public interface ICertificateOptions : ICloneable<ICertificateOptions>
	{
		#region Properties

		ICertificateAuthorityOptions CertificateAuthority { get; set; }
		EnhancedKeyUsage EnhancedKeyUsage { get; set; }
		HashAlgorithm HashAlgorithm { get; set; }
		ICertificate Issuer { get; set; }
		X509KeyUsageFlags KeyUsage { get; set; }
		DateTimeOffset? NotAfter { get; set; }
		DateTimeOffset? NotBefore { get; set; }

		/// <summary>
		/// The serial-number size. The number of bytes a generated serial-number will have. If null the configured serial-number size is used. Only used if the "Issuer" is not null. If the "Issuer" is null, a self-signed certificate is created and a default serial-number size, built-in, is used.
		/// </summary>
		byte? SerialNumberSize { get; set; }

		string Subject { get; set; }
		ISubjectAlternativeNameOptions SubjectAlternativeName { get; set; }

		#endregion
	}
}