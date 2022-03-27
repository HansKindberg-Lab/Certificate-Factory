using System.Security.Cryptography;
using Application.Models.Cryptography.Transferring.Configuration;
using Microsoft.Extensions.Options;

namespace Application.Models.Cryptography.Transferring
{
	/// <inheritdoc />
	public class RsaKeyExporter : KeyExporter<RSA>
	{
		#region Constructors

		public RsaKeyExporter(IOptionsMonitor<KeyExporterOptions> optionsMonitor, RSA rsa) : base(rsa, optionsMonitor) { }

		#endregion
	}
}