using Application.Models.Cryptography;

namespace UnitTests.Models.Cryptography
{
	public class CertificateAuthorityOptionsTest
	{
		#region Methods

		[Fact]
		public async Task Clone_ShouldReturnAClone()
		{
			await Task.CompletedTask;

			var certificateAuthorityOptions = new CertificateAuthorityOptions();
			var clone = certificateAuthorityOptions.Clone();

			Assert.True(certificateAuthorityOptions.CertificateAuthority);
			Assert.True(clone.CertificateAuthority);
			Assert.False(certificateAuthorityOptions.HasPathLengthConstraint);
			Assert.False(clone.HasPathLengthConstraint);
			Assert.Equal(0, certificateAuthorityOptions.PathLengthConstraint);
			Assert.Equal(0, clone.PathLengthConstraint);

			certificateAuthorityOptions.CertificateAuthority = false;
			Assert.False(certificateAuthorityOptions.CertificateAuthority);
			Assert.True(clone.CertificateAuthority);
			clone.CertificateAuthority = false;
			Assert.False(certificateAuthorityOptions.CertificateAuthority);
			Assert.False(clone.CertificateAuthority);

			certificateAuthorityOptions.HasPathLengthConstraint = true;
			Assert.True(certificateAuthorityOptions.HasPathLengthConstraint);
			Assert.False(clone.HasPathLengthConstraint);
			clone.HasPathLengthConstraint = true;
			Assert.True(certificateAuthorityOptions.HasPathLengthConstraint);
			Assert.True(clone.HasPathLengthConstraint);

			certificateAuthorityOptions.PathLengthConstraint = 10;
			Assert.Equal(10, certificateAuthorityOptions.PathLengthConstraint);
			Assert.Equal(0, clone.PathLengthConstraint);
			clone.PathLengthConstraint = 20;
			Assert.Equal(10, certificateAuthorityOptions.PathLengthConstraint);
			Assert.Equal(20, clone.PathLengthConstraint);
			certificateAuthorityOptions = new CertificateAuthorityOptions { PathLengthConstraint = 4 };
			clone = certificateAuthorityOptions.Clone();
			Assert.Equal(4, certificateAuthorityOptions.PathLengthConstraint);
			Assert.Equal(4, clone.PathLengthConstraint);
			certificateAuthorityOptions.PathLengthConstraint = 1;
			clone.PathLengthConstraint = 20;
			Assert.Equal(1, certificateAuthorityOptions.PathLengthConstraint);
			Assert.Equal(20, clone.PathLengthConstraint);
			certificateAuthorityOptions.PathLengthConstraint = 10;
			Assert.Equal(10, certificateAuthorityOptions.PathLengthConstraint);
			Assert.Equal(20, clone.PathLengthConstraint);
		}

		#endregion
	}
}