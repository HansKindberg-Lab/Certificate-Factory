namespace Application.Models.Cryptography.Transferring
{
	/// <inheritdoc />
	public class CertificateTransfer : ICertificateTransfer
	{
		#region Properties

		public virtual string CertificatePem { get; set; }
		public virtual string EncryptedPrivateKeyPem { get; set; }
		public virtual IEnumerable<byte> Pfx { get; set; }
		public virtual IEnumerable<byte> Pkcs12 { get; set; }
		public virtual string PrivateKeyPem { get; set; }
		public virtual string PublicKeyPem { get; set; }

		#endregion
	}
}