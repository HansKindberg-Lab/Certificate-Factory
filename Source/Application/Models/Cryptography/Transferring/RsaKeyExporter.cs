using System.Security.Cryptography;
using Application.Models.Cryptography.Transferring.Configuration;
using Microsoft.Extensions.Options;

namespace Application.Models.Cryptography.Transferring
{
	/// <inheritdoc />
	public class RsaKeyExporter(IOptionsMonitor<KeyExporterOptions> optionsMonitor, RSA rsa) : KeyExporter<RSA>(rsa, optionsMonitor) { }
}