using Application.Models.Cryptography;
using Application.Models.Cryptography.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Models.Cryptography.Extensions
{
	[TestClass]
	public class EnhancedKeyUsageExtensionTest
	{
		#region Methods

		[TestMethod]
		public async Task Description_Test()
		{
			await Task.CompletedTask;

			var descriptions = EnhancedKeyUsage.None.Descriptions();
			Assert.IsFalse(descriptions.Any());

			descriptions = EnhancedKeyUsage.ClientAuthentication.Descriptions();
			Assert.AreEqual(1, descriptions.Count);
			Assert.AreEqual("1.3.6.1.5.5.7.3.2", descriptions.First());

			descriptions = EnhancedKeyUsage.CodeSigning.Descriptions();
			Assert.AreEqual(1, descriptions.Count);
			Assert.AreEqual("1.3.6.1.5.5.7.3.3", descriptions.First());

			descriptions = EnhancedKeyUsage.SecureEmail.Descriptions();
			Assert.AreEqual(1, descriptions.Count);
			Assert.AreEqual("1.3.6.1.5.5.7.3.4", descriptions.First());

			descriptions = EnhancedKeyUsage.ServerAuthentication.Descriptions();
			Assert.AreEqual(1, descriptions.Count);
			Assert.AreEqual("1.3.6.1.5.5.7.3.1", descriptions.First());

			descriptions = EnhancedKeyUsage.TimestampSigning.Descriptions();
			Assert.AreEqual(1, descriptions.Count);
			Assert.AreEqual("1.3.6.1.5.5.7.3.8", descriptions.First());

			descriptions = (EnhancedKeyUsage.ClientAuthentication | EnhancedKeyUsage.ServerAuthentication).Descriptions();
			Assert.AreEqual(2, descriptions.Count);
			Assert.AreEqual("1.3.6.1.5.5.7.3.1", descriptions.First());
			Assert.AreEqual("1.3.6.1.5.5.7.3.2", descriptions.ElementAt(1));

			descriptions = (EnhancedKeyUsage.ClientAuthentication | EnhancedKeyUsage.CodeSigning | EnhancedKeyUsage.None | EnhancedKeyUsage.SecureEmail | EnhancedKeyUsage.ServerAuthentication | EnhancedKeyUsage.TimestampSigning).Descriptions();
			Assert.AreEqual(5, descriptions.Count);
			Assert.AreEqual("1.3.6.1.5.5.7.3.1", descriptions.First());
			Assert.AreEqual("1.3.6.1.5.5.7.3.2", descriptions.ElementAt(1));
			Assert.AreEqual("1.3.6.1.5.5.7.3.3", descriptions.ElementAt(2));
			Assert.AreEqual("1.3.6.1.5.5.7.3.4", descriptions.ElementAt(3));
			Assert.AreEqual("1.3.6.1.5.5.7.3.8", descriptions.Last());
		}

		#endregion
	}
}