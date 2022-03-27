namespace Application.Models.Cryptography.Transferring
{
	public interface ICertificateTransfer
	{
		#region Properties

		string CertificatePem { get; }
		string EncryptedPrivateKeyPem { get; }
		IEnumerable<byte> Pfx { get; }
		IEnumerable<byte> Pkcs12 { get; }
		string PrivateKeyPem { get; }
		string PublicKeyPem { get; }

		#endregion
	}
}