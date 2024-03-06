using Application.Models.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Models.Cryptography
{
	[TestClass]
	public class CertificateAuthorityOptionsTest
	{
		#region Methods

		[TestMethod]
		public async Task Clone_ShouldReturnAClone()
		{
			await Task.CompletedTask;

			var certificateAuthorityOptions = new CertificateAuthorityOptions();
			var clone = certificateAuthorityOptions.Clone();

			Assert.IsTrue(certificateAuthorityOptions.CertificateAuthority);
			Assert.IsTrue(clone.CertificateAuthority);
			Assert.IsFalse(certificateAuthorityOptions.HasPathLengthConstraint);
			Assert.IsFalse(clone.HasPathLengthConstraint);
			Assert.AreEqual(0, certificateAuthorityOptions.PathLengthConstraint);
			Assert.AreEqual(0, clone.PathLengthConstraint);

			certificateAuthorityOptions.CertificateAuthority = false;
			Assert.IsFalse(certificateAuthorityOptions.CertificateAuthority);
			Assert.IsTrue(clone.CertificateAuthority);
			clone.CertificateAuthority = false;
			Assert.IsFalse(certificateAuthorityOptions.CertificateAuthority);
			Assert.IsFalse(clone.CertificateAuthority);

			certificateAuthorityOptions.HasPathLengthConstraint = true;
			Assert.IsTrue(certificateAuthorityOptions.HasPathLengthConstraint);
			Assert.IsFalse(clone.HasPathLengthConstraint);
			clone.HasPathLengthConstraint = true;
			Assert.IsTrue(certificateAuthorityOptions.HasPathLengthConstraint);
			Assert.IsTrue(clone.HasPathLengthConstraint);

			certificateAuthorityOptions.PathLengthConstraint = 10;
			Assert.AreEqual(10, certificateAuthorityOptions.PathLengthConstraint);
			Assert.AreEqual(0, clone.PathLengthConstraint);
			clone.PathLengthConstraint = 20;
			Assert.AreEqual(10, certificateAuthorityOptions.PathLengthConstraint);
			Assert.AreEqual(20, clone.PathLengthConstraint);
			certificateAuthorityOptions = new CertificateAuthorityOptions { PathLengthConstraint = 4 };
			clone = certificateAuthorityOptions.Clone();
			Assert.AreEqual(4, certificateAuthorityOptions.PathLengthConstraint);
			Assert.AreEqual(4, clone.PathLengthConstraint);
			certificateAuthorityOptions.PathLengthConstraint = 1;
			clone.PathLengthConstraint = 20;
			Assert.AreEqual(1, certificateAuthorityOptions.PathLengthConstraint);
			Assert.AreEqual(20, clone.PathLengthConstraint);
			certificateAuthorityOptions.PathLengthConstraint = 10;
			Assert.AreEqual(10, certificateAuthorityOptions.PathLengthConstraint);
			Assert.AreEqual(20, clone.PathLengthConstraint);
		}

		#endregion
	}
}