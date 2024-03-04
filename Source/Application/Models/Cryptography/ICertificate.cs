using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Application.Models.Cryptography
{
	/// <inheritdoc />
	public interface ICertificate : IDisposable
	{
		#region Properties

		bool Archived { get; }
		bool HasPrivateKey { get; }
		string Issuer { get; }

		/// <summary>
		/// The object identifier, oid, for the key algorithm.
		/// </summary>
		string KeyAlgorithm { get; }

		/// <summary>
		/// The name of the key algorithm.
		/// </summary>
		string KeyAlgorithmName { get; }

		DateTime NotAfter { get; }
		DateTime NotBefore { get; }
		IEnumerable<byte> RawData { get; }
		string SerialNumber { get; }
		string Subject { get; }
		string Thumbprint { get; }

		#endregion

		#region Methods

		IEnumerable<byte> Export(X509ContentType contentType, string password);
		string GetCertificatePem();
		IEnumerable<byte> GetPfx(string password);
		IEnumerable<byte> GetPkcs12(string password);
		AsymmetricAlgorithm GetPrivateKeyAsymmetricAlgorithm();
		string ToString(bool verbose);

		#endregion
	}
}