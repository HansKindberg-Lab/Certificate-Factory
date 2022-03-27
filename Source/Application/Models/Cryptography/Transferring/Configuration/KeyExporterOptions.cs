using System.Security.Cryptography;

namespace Application.Models.Cryptography.Transferring.Configuration
{
	public class KeyExporterOptions
	{
		#region Properties

		public virtual PbeEncryptionAlgorithm EncryptionAlgorithm { get; set; } = PbeEncryptionAlgorithm.Aes128Cbc;
		public virtual HashAlgorithm HashAlgorithm { get; set; } = HashAlgorithm.Sha512;
		public virtual int IterationCount { get; set; } = 600_000;

		#endregion
	}
}