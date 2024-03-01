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
			Assert.IsNull(certificateAuthorityOptions.PathLengthConstraint);
			Assert.IsNull(clone.PathLengthConstraint);
			certificateAuthorityOptions.PathLengthConstraint = 10;
			Assert.AreEqual(10, certificateAuthorityOptions.PathLengthConstraint);
			Assert.IsNull(clone.PathLengthConstraint);
			clone.PathLengthConstraint = 20;
			Assert.AreEqual(10, certificateAuthorityOptions.PathLengthConstraint);
			Assert.AreEqual(20, clone.PathLengthConstraint);

			certificateAuthorityOptions = new CertificateAuthorityOptions { PathLengthConstraint = 4 };
			clone = certificateAuthorityOptions.Clone();
			Assert.AreEqual(4, certificateAuthorityOptions.PathLengthConstraint);
			Assert.AreEqual(4, clone.PathLengthConstraint);
			certificateAuthorityOptions.PathLengthConstraint = null;
			clone.PathLengthConstraint = 20;
			Assert.IsNull(certificateAuthorityOptions.PathLengthConstraint);
			Assert.AreEqual(20, clone.PathLengthConstraint);
			certificateAuthorityOptions.PathLengthConstraint = 10;
			Assert.AreEqual(10, certificateAuthorityOptions.PathLengthConstraint);
			Assert.AreEqual(20, clone.PathLengthConstraint);
		}

		#endregion
	}
}