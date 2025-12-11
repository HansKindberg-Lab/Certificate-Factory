using Application.Models.Cryptography;

namespace UnitTests.Models.Cryptography
{
	public class CertificateConstructionOptionsTest
	{
		#region Methods

		[Fact]
		public async Task Clone_Test()
		{
			await Task.CompletedTask;

			var certificateConstructionOptions = new CertificateConstructionOptions();

			Assert.NotNull(certificateConstructionOptions);
			Assert.Null(certificateConstructionOptions.AsymmetricAlgorithm);
			Assert.Null(certificateConstructionOptions.CertificateAuthority);
			Assert.Null(certificateConstructionOptions.CrlDistributionPoint);
			Assert.Null(certificateConstructionOptions.EnhancedKeyUsage);
			Assert.Null(certificateConstructionOptions.HashAlgorithm);
			Assert.Null(certificateConstructionOptions.KeyUsage);
			Assert.Null(certificateConstructionOptions.NotAfter);
			Assert.Null(certificateConstructionOptions.NotBefore);
			Assert.Null(certificateConstructionOptions.Subject);
			Assert.Null(certificateConstructionOptions.SubjectAlternativeName);

			var clone = certificateConstructionOptions.Clone();

			Assert.NotNull(clone);
			Assert.False(ReferenceEquals(certificateConstructionOptions, clone));
			Assert.Null(clone.AsymmetricAlgorithm);
			Assert.Null(clone.CertificateAuthority);
			Assert.Null(clone.CrlDistributionPoint);
			Assert.Null(clone.EnhancedKeyUsage);
			Assert.Null(clone.HashAlgorithm);
			Assert.Null(clone.KeyUsage);
			Assert.Null(clone.NotAfter);
			Assert.Null(clone.NotBefore);
			Assert.Null(clone.Subject);
			Assert.Null(clone.SubjectAlternativeName);
		}

		#endregion
	}
}