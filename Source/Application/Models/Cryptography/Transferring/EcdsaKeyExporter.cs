using System.Security.Cryptography;
using Application.Models.Cryptography.Transferring.Configuration;
using Microsoft.Extensions.Options;

namespace Application.Models.Cryptography.Transferring
{
	/// <inheritdoc />
	public class EcdsaKeyExporter(ECDsa ecdsa, IOptionsMonitor<KeyExporterOptions> optionsMonitor) : KeyExporter<ECDsa>(ecdsa, optionsMonitor) { }
}