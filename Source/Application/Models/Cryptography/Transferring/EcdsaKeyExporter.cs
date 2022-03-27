using System.Security.Cryptography;
using Application.Models.Cryptography.Transferring.Configuration;
using Microsoft.Extensions.Options;

namespace Application.Models.Cryptography.Transferring
{
	/// <inheritdoc />
	public class EcdsaKeyExporter : KeyExporter<ECDsa>
	{
		#region Constructors

		public EcdsaKeyExporter(ECDsa ecdsa, IOptionsMonitor<KeyExporterOptions> optionsMonitor) : base(ecdsa, optionsMonitor) { }

		#endregion
	}
}