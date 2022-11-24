using System.ComponentModel;
using Application.Models.ComponentModel;

namespace Application.Models.Cryptography.Archiving
{
	public enum ArchiveKind
	{
		[Description("All")] [Example(" - certificate.CertificatePem.crt\a - certificate.CertificatePem.one-liner.crt\a - certificate.EncryptedPrivateKeyPem.key\a - certificate.EncryptedPrivateKeyPem.one-liner.key\a - certificate.p12\a - certificate.pfx\a - certificate.PrivateKeyPem.key\a - certificate.PrivateKeyPem.one-liner.key\a - certificate.PublicKeyPem.key\a - certificate.PublicKeyPem.one-liner.key")]
		All,

		[Description("*.crt, *.key, *.p12 and *.pfx files")] [Example(" - certificate.crt\a - certificate.key\a - certificate.p12\a - certificate.pfx")]
		CrtAndKeyAndP12AndPfx,

		[Description("*.crt, *.key and *.p12 files")] [Example(" - certificate.crt\a - certificate.key\a - certificate.p12")]
		CrtAndKeyAndP12,

		[Description("*.crt, *.key and *.pfx files")] [Example(" - certificate.crt\a - certificate.key\a - certificate.pfx")]
		CrtAndKeyAndPfx,

		[Description("*.crt and *.key files")] [Example(" - certificate.crt\a - certificate.key")]
		CrtAndKey,

		[Description("*.p12 file")] [Example(" - certificate.p12")]
		P12,

		[Description("*.pfx file")] [Example(" - certificate.pfx")]
		Pfx
	}
}