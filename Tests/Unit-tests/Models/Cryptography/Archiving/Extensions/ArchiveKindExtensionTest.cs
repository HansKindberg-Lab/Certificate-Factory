using Application.Models.Cryptography.Archiving;
using Application.Models.Cryptography.Archiving.Extensions;

namespace UnitTests.Models.Cryptography.Archiving.Extensions
{
	public class ArchiveKindExtensionTest
	{
		#region Methods

		[Fact]
		public async Task Description_Test()
		{
			await Task.CompletedTask;

			Assert.Equal("All", ArchiveKind.All.Description());
			Assert.Equal("*.crt and *.key files", ArchiveKind.CrtAndKey.Description());
			Assert.Equal("*.crt, *.key and *.p12 files", ArchiveKind.CrtAndKeyAndP12.Description());
			Assert.Equal("*.crt, *.key, *.p12 and *.pfx files", ArchiveKind.CrtAndKeyAndP12AndPfx.Description());
			Assert.Equal("*.crt, *.key and *.pfx files", ArchiveKind.CrtAndKeyAndPfx.Description());
			Assert.Equal("*.p12 file", ArchiveKind.P12.Description());
			Assert.Equal("*.pfx file", ArchiveKind.Pfx.Description());
		}

		[Fact]
		public async Task Example_Test()
		{
			await Task.CompletedTask;

			Assert.Equal($" - certificate.CertificatePem.crt{Environment.NewLine} - certificate.CertificatePem.one-liner.crt{Environment.NewLine} - certificate.EncryptedPrivateKeyPem.key{Environment.NewLine} - certificate.EncryptedPrivateKeyPem.one-liner.key{Environment.NewLine} - certificate.p12{Environment.NewLine} - certificate.pfx{Environment.NewLine} - certificate.PrivateKeyPem.key{Environment.NewLine} - certificate.PrivateKeyPem.one-liner.key{Environment.NewLine} - certificate.PublicKeyPem.key{Environment.NewLine} - certificate.PublicKeyPem.one-liner.key", ArchiveKind.All.Example());
			Assert.Equal($" - certificate.crt{Environment.NewLine} - certificate.key", ArchiveKind.CrtAndKey.Example());
			Assert.Equal($" - certificate.crt{Environment.NewLine} - certificate.key{Environment.NewLine} - certificate.p12", ArchiveKind.CrtAndKeyAndP12.Example());
			Assert.Equal($" - certificate.crt{Environment.NewLine} - certificate.key{Environment.NewLine} - certificate.p12{Environment.NewLine} - certificate.pfx", ArchiveKind.CrtAndKeyAndP12AndPfx.Example());
			Assert.Equal($" - certificate.crt{Environment.NewLine} - certificate.key{Environment.NewLine} - certificate.pfx", ArchiveKind.CrtAndKeyAndPfx.Example());
			Assert.Equal(" - certificate.p12", ArchiveKind.P12.Example());
			Assert.Equal(" - certificate.pfx", ArchiveKind.Pfx.Example());
		}

		#endregion
	}
}