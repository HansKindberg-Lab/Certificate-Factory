using System.Security.Cryptography;
using Application.Models.Cryptography.Extensions;
using Application.Models.Cryptography.Transferring.Configuration;
using Microsoft.Extensions.Options;

namespace Application.Models.Cryptography.Transferring
{
	/// <inheritdoc />
	public abstract class KeyExporter<T>(T asymmetricAlgorithm, IOptionsMonitor<KeyExporterOptions> optionsMonitor) : IKeyExporter where T : AsymmetricAlgorithm
	{
		#region Fields

		private const string _encryptedPrivateKeyPemLabelFormat = "{0}ENCRYPTED PRIVATE KEY";
		private const string _privateKeyPemLabelFormat = "{0}PRIVATE KEY";
		private const string _publicKeyPemLabelFormat = "{0}PUBLIC KEY";

		#endregion

		#region Properties

		protected internal virtual AsymmetricAlgorithm AsymmetricAlgorithm { get; } = asymmetricAlgorithm ?? throw new ArgumentNullException(nameof(asymmetricAlgorithm));
		protected internal virtual string EncryptedPrivateKeyPemLabel => string.Format(null, this.EncryptedPrivateKeyPemLabelFormat, this.EncryptedPrivateKeyPemLabelPrefix);
		protected internal virtual string EncryptedPrivateKeyPemLabelFormat => _encryptedPrivateKeyPemLabelFormat;
		protected internal virtual string EncryptedPrivateKeyPemLabelPrefix => string.Empty;
		protected internal virtual IOptionsMonitor<KeyExporterOptions> OptionsMonitor { get; } = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
		protected internal virtual string PrivateKeyPemLabel => string.Format(null, this.PrivateKeyPemLabelFormat, this.PrivateKeyPemLabelPrefix);
		protected internal virtual string PrivateKeyPemLabelFormat => _privateKeyPemLabelFormat;
		protected internal virtual string PrivateKeyPemLabelPrefix => string.Empty;
		protected internal virtual string PublicKeyPemLabel => string.Format(null, this.PublicKeyPemLabelFormat, this.PublicKeyPemLabelPrefix);
		protected internal virtual string PublicKeyPemLabelFormat => _publicKeyPemLabelFormat;
		protected internal virtual string PublicKeyPemLabelPrefix => string.Empty;

		#endregion

		#region Methods

		public virtual string GetEncryptedPrivateKeyPem(string password)
		{
			// https://github.com/dotnet/runtime/issues/51597/#issuecomment-823675868

			var options = this.OptionsMonitor.CurrentValue;
			var parameters = new PbeParameters(options.EncryptionAlgorithm, options.HashAlgorithm.ToHashAlgorithmName(), options.IterationCount);

			return new string(PemEncoding.Write(this.EncryptedPrivateKeyPemLabel, this.AsymmetricAlgorithm.ExportEncryptedPkcs8PrivateKey(password, parameters)));
		}

		public virtual string GetPrivateKeyPem()
		{
			return new string(PemEncoding.Write(this.PrivateKeyPemLabel, this.AsymmetricAlgorithm.ExportPkcs8PrivateKey()));
		}

		public virtual string GetPublicKeyPem()
		{
			return new string(PemEncoding.Write(this.PublicKeyPemLabel, this.AsymmetricAlgorithm.ExportSubjectPublicKeyInfo()));
		}

		#endregion
	}
}