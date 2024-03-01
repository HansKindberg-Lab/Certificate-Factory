using System.Security.Cryptography;
using Application.Models.Cryptography.Transferring.Configuration;
using Microsoft.Extensions.Options;

namespace Application.Models.Cryptography.Transferring
{
	/// <inheritdoc />
	public class KeyExporterFactory(IOptionsMonitor<KeyExporterOptions> optionsMonitor) : IKeyExporterFactory
	{
		#region Properties

		protected internal virtual IOptionsMonitor<KeyExporterOptions> OptionsMonitor { get; } = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));

		#endregion

		#region Methods

		public virtual IKeyExporter Create(AsymmetricAlgorithm asymmetricAlgorithm)
		{
			return asymmetricAlgorithm switch
			{
				null => throw new ArgumentNullException(nameof(asymmetricAlgorithm)),
				ECDsa ecdsa => new EcdsaKeyExporter(ecdsa, this.OptionsMonitor),
				RSA rsa => new RsaKeyExporter(this.OptionsMonitor, rsa),
				_ => throw new InvalidOperationException($"Could not create key-exporter from asymmetric algorithm of type \"{asymmetricAlgorithm.GetType()}\".")
			};
		}

		#endregion
	}
}