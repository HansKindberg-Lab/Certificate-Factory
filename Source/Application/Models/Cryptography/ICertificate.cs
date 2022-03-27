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

		string ToString(bool verbose);

		#endregion
	}
}