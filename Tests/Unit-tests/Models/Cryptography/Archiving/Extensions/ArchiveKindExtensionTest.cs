using Application.Models.Cryptography.Archiving;
using Application.Models.Cryptography.Archiving.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Models.Cryptography.Archiving.Extensions
{
	[TestClass]
	public class ArchiveKindExtensionTest
	{
		#region Methods

		[TestMethod]
		public async Task Description_Test()
		{
			await Task.CompletedTask;

			Assert.AreEqual("All", ArchiveKind.All.Description());
			Assert.AreEqual("*.crt and *.key files", ArchiveKind.CrtAndKey.Description());
			Assert.AreEqual("*.crt, *.key and *.p12 files", ArchiveKind.CrtAndKeyAndP12.Description());
			Assert.AreEqual("*.crt, *.key, *.p12 and *.pfx files", ArchiveKind.CrtAndKeyAndP12AndPfx.Description());
			Assert.AreEqual("*.crt, *.key and *.pfx files", ArchiveKind.CrtAndKeyAndPfx.Description());
			Assert.AreEqual("*.p12 file", ArchiveKind.P12.Description());
			Assert.AreEqual("*.pfx file", ArchiveKind.Pfx.Description());
		}

		[TestMethod]
		public async Task Example_Test()
		{
			await Task.CompletedTask;

			Assert.AreEqual($" - certificate.CertificatePem.crt{Environment.NewLine} - certificate.CertificatePem.one-liner.crt{Environment.NewLine} - certificate.EncryptedPrivateKeyPem.key{Environment.NewLine} - certificate.EncryptedPrivateKeyPem.one-liner.key{Environment.NewLine} - certificate.p12{Environment.NewLine} - certificate.pfx{Environment.NewLine} - certificate.PrivateKeyPem.key{Environment.NewLine} - certificate.PrivateKeyPem.one-liner.key{Environment.NewLine} - certificate.PublicKeyPem.key{Environment.NewLine} - certificate.PublicKeyPem.one-liner.key", ArchiveKind.All.Example());
			Assert.AreEqual($" - certificate.crt{Environment.NewLine} - certificate.key", ArchiveKind.CrtAndKey.Example());
			Assert.AreEqual($" - certificate.crt{Environment.NewLine} - certificate.key{Environment.NewLine} - certificate.p12", ArchiveKind.CrtAndKeyAndP12.Example());
			Assert.AreEqual($" - certificate.crt{Environment.NewLine} - certificate.key{Environment.NewLine} - certificate.p12{Environment.NewLine} - certificate.pfx", ArchiveKind.CrtAndKeyAndP12AndPfx.Example());
			Assert.AreEqual($" - certificate.crt{Environment.NewLine} - certificate.key{Environment.NewLine} - certificate.pfx", ArchiveKind.CrtAndKeyAndPfx.Example());
			Assert.AreEqual(" - certificate.p12", ArchiveKind.P12.Example());
			Assert.AreEqual(" - certificate.pfx", ArchiveKind.Pfx.Example());
		}

		#endregion
	}
}