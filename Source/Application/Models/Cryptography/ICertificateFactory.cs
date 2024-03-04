using System.Security.Cryptography.X509Certificates;

namespace Application.Models.Cryptography
{
	public interface ICertificateFactory
	{
		#region Methods

		/// <summary>
		/// Create a new certificate.
		/// </summary>
		ICertificate Create(IAsymmetricAlgorithmOptions asymmetricAlgorithmOptions, ICertificateOptions certificateOptions);

		/// <summary>
		/// Wrap an existing certificate.
		/// </summary>
		ICertificate Create(X509Certificate2 certificate, ICertificateStore store);

		#endregion
	}
}