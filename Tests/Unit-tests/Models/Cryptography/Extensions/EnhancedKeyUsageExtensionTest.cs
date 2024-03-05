using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Application.Models.Cryptography;
using Application.Models.Cryptography.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Models.Cryptography.Extensions
{
	[TestClass]
	public class EnhancedKeyUsageExtensionTest
	{
		#region Methods

		private static X509EnhancedKeyUsageExtension CreateEnhancedKeyUsageExtension(params string[] values)
		{
			var collection = new OidCollection();

			foreach(var value in values)
			{
				collection.Add(new Oid(value));
			}

			return new X509EnhancedKeyUsageExtension(collection, false);
		}

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

		[TestMethod]
		public async Task GetByExtension_Test()
		{
			await Task.CompletedTask;

			var enhancedKeyUsageExtension = new X509EnhancedKeyUsageExtension();
			var enhancedKeyUsage = EnhancedKeyUsageExtension.GetByExtension(enhancedKeyUsageExtension);
			Assert.IsNull(enhancedKeyUsage);

			enhancedKeyUsageExtension = CreateEnhancedKeyUsageExtension("1.1");
			enhancedKeyUsage = EnhancedKeyUsageExtension.GetByExtension(enhancedKeyUsageExtension);
			Assert.IsNull(enhancedKeyUsage);

			enhancedKeyUsageExtension = CreateEnhancedKeyUsageExtension(EnhancedKeyUsageValues.ClientAuthentication);
			enhancedKeyUsage = EnhancedKeyUsageExtension.GetByExtension(enhancedKeyUsageExtension);
			Assert.AreEqual(EnhancedKeyUsage.ClientAuthentication, enhancedKeyUsage);

			enhancedKeyUsageExtension = CreateEnhancedKeyUsageExtension(EnhancedKeyUsageValues.CodeSigning);
			enhancedKeyUsage = EnhancedKeyUsageExtension.GetByExtension(enhancedKeyUsageExtension);
			Assert.AreEqual(EnhancedKeyUsage.CodeSigning, enhancedKeyUsage);

			enhancedKeyUsageExtension = CreateEnhancedKeyUsageExtension(EnhancedKeyUsageValues.SecureEmail);
			enhancedKeyUsage = EnhancedKeyUsageExtension.GetByExtension(enhancedKeyUsageExtension);
			Assert.AreEqual(EnhancedKeyUsage.SecureEmail, enhancedKeyUsage);

			enhancedKeyUsageExtension = CreateEnhancedKeyUsageExtension(EnhancedKeyUsageValues.ServerAuthentication);
			enhancedKeyUsage = EnhancedKeyUsageExtension.GetByExtension(enhancedKeyUsageExtension);
			Assert.AreEqual(EnhancedKeyUsage.ServerAuthentication, enhancedKeyUsage);

			enhancedKeyUsageExtension = CreateEnhancedKeyUsageExtension(EnhancedKeyUsageValues.TimestampSigning);
			enhancedKeyUsage = EnhancedKeyUsageExtension.GetByExtension(enhancedKeyUsageExtension);
			Assert.AreEqual(EnhancedKeyUsage.TimestampSigning, enhancedKeyUsage);

			enhancedKeyUsageExtension = CreateEnhancedKeyUsageExtension(EnhancedKeyUsageValues.ClientAuthentication, EnhancedKeyUsageValues.SecureEmail, EnhancedKeyUsageValues.ServerAuthentication, EnhancedKeyUsageValues.TimestampSigning);
			enhancedKeyUsage = EnhancedKeyUsageExtension.GetByExtension(enhancedKeyUsageExtension);
			Assert.IsNotNull(enhancedKeyUsage);
			Assert.AreEqual(EnhancedKeyUsage.ClientAuthentication | EnhancedKeyUsage.SecureEmail | EnhancedKeyUsage.ServerAuthentication | EnhancedKeyUsage.TimestampSigning, enhancedKeyUsage);
			Assert.IsTrue(enhancedKeyUsage.Value.HasFlag(EnhancedKeyUsage.ClientAuthentication));
			Assert.IsTrue(enhancedKeyUsage.Value.HasFlag(EnhancedKeyUsage.SecureEmail));
			Assert.IsTrue(enhancedKeyUsage.Value.HasFlag(EnhancedKeyUsage.ServerAuthentication));
			Assert.IsTrue(enhancedKeyUsage.Value.HasFlag(EnhancedKeyUsage.TimestampSigning));
		}

		#endregion
	}
}