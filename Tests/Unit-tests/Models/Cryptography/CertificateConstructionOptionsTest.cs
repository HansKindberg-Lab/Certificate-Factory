using Application.Models.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Models.Cryptography
{
	[TestClass]
	public class CertificateConstructionOptionsTest
	{
		#region Methods

		[TestMethod]
		public async Task Clone_Test()
		{
			await Task.CompletedTask;

			var certificateConstructionOptions = new CertificateConstructionOptions();

			Assert.IsNotNull(certificateConstructionOptions);
			Assert.IsNull(certificateConstructionOptions.AsymmetricAlgorithm);
			Assert.IsNull(certificateConstructionOptions.CertificateAuthority);
			Assert.IsNull(certificateConstructionOptions.CrlDistributionPoint);
			Assert.IsNull(certificateConstructionOptions.EnhancedKeyUsage);
			Assert.IsNull(certificateConstructionOptions.HashAlgorithm);
			Assert.IsNull(certificateConstructionOptions.KeyUsage);
			Assert.IsNull(certificateConstructionOptions.NotAfter);
			Assert.IsNull(certificateConstructionOptions.NotBefore);
			Assert.IsNull(certificateConstructionOptions.Subject);
			Assert.IsNull(certificateConstructionOptions.SubjectAlternativeName);

			var clone = certificateConstructionOptions.Clone();

			Assert.IsNotNull(clone);
			Assert.IsFalse(ReferenceEquals(certificateConstructionOptions, clone));
			Assert.IsNull(clone.AsymmetricAlgorithm);
			Assert.IsNull(clone.CertificateAuthority);
			Assert.IsNull(clone.CrlDistributionPoint);
			Assert.IsNull(clone.EnhancedKeyUsage);
			Assert.IsNull(clone.HashAlgorithm);
			Assert.IsNull(clone.KeyUsage);
			Assert.IsNull(clone.NotAfter);
			Assert.IsNull(clone.NotBefore);
			Assert.IsNull(clone.Subject);
			Assert.IsNull(clone.SubjectAlternativeName);
		}

		#endregion
	}
}