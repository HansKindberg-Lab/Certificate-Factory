using System.Net;
using System.Security.Cryptography.X509Certificates;
using Application.Models.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Models.Cryptography
{
	[TestClass]
	public class CertificateConstructionTreeOptionsTest
	{
		#region Fields

		private const string _resourcesDirectoryRelativePath = "Models/Cryptography/Resources/CertificateConstructionTreeOptions";

		#endregion

		#region Methods

		[TestMethod]
		public async Task Configuration_Bind_Test_1()
		{
			var configuration = await this.CreateConfigurationAsync("Configuration-1.json");
			var constructionTree = new CertificateConstructionTreeOptions();
			configuration.Bind(constructionTree);
			Assert.AreEqual("RSA:2048:Pkcs1", constructionTree.Defaults.AsymmetricAlgorithm);
			Assert.AreEqual(HashAlgorithm.Sha256, constructionTree.Defaults.HashAlgorithm);
			Assert.AreEqual(new DateTimeOffset(2050, 1, 1, 0, 0, 0, DateTimeOffset.Parse("2000-01-01", null).Offset), constructionTree.Defaults.NotAfter);
			Assert.AreEqual("root-certificate", constructionTree.Roots.Keys.ElementAt(0));
			Assert.AreEqual("CN=Test Root CA", constructionTree.Roots["root-certificate"].Certificate.Subject);
			Assert.IsTrue(constructionTree.RootsDefaults.CertificateAuthority.CertificateAuthority);
			Assert.IsFalse(constructionTree.RootsDefaults.CertificateAuthority.HasPathLengthConstraint);
			Assert.AreEqual(0, constructionTree.RootsDefaults.CertificateAuthority.PathLengthConstraint);
			Assert.AreEqual(X509KeyUsageFlags.KeyCertSign, constructionTree.RootsDefaults.KeyUsage);
		}

		[TestMethod]
		public async Task Configuration_Bind_Test_2()
		{
			var configuration = await this.CreateConfigurationAsync("Configuration-4.json");
			var constructionTree = new CertificateConstructionTreeOptions();
			configuration.Bind(constructionTree);

			Assert.AreEqual(3, constructionTree.Defaults.SubjectAlternativeName.DnsNames.Count);
			Assert.AreEqual("site-1.example.org", constructionTree.Defaults.SubjectAlternativeName.DnsNames.ElementAt(0));
			Assert.AreEqual("site-2.example.org", constructionTree.Defaults.SubjectAlternativeName.DnsNames.ElementAt(1));
			Assert.AreEqual("site-3.example.org", constructionTree.Defaults.SubjectAlternativeName.DnsNames.ElementAt(2));

			Assert.AreEqual(3, constructionTree.Defaults.SubjectAlternativeName.EmailAddresses.Count);
			Assert.AreEqual("user-1@example.org", constructionTree.Defaults.SubjectAlternativeName.EmailAddresses.ElementAt(0));
			Assert.AreEqual("user-2@example.org", constructionTree.Defaults.SubjectAlternativeName.EmailAddresses.ElementAt(1));
			Assert.AreEqual("user-3@example.org", constructionTree.Defaults.SubjectAlternativeName.EmailAddresses.ElementAt(2));

			Assert.AreEqual(3, constructionTree.Defaults.SubjectAlternativeName.IpAddresses.Count);
			Assert.AreEqual(IPAddress.Parse("127.0.0.1"), constructionTree.Defaults.SubjectAlternativeName.IpAddresses.ElementAt(0));
			Assert.AreEqual(IPAddress.Parse("127.0.0.2"), constructionTree.Defaults.SubjectAlternativeName.IpAddresses.ElementAt(1));
			Assert.AreEqual(IPAddress.Parse("::1"), constructionTree.Defaults.SubjectAlternativeName.IpAddresses.ElementAt(2));

			Assert.AreEqual(3, constructionTree.Defaults.SubjectAlternativeName.Uris.Count);
			Assert.AreEqual(new Uri("https://site-1.example.org"), constructionTree.Defaults.SubjectAlternativeName.Uris.ElementAt(0));
			Assert.AreEqual(new Uri("https://site-2.example.org"), constructionTree.Defaults.SubjectAlternativeName.Uris.ElementAt(1));
			Assert.AreEqual(new Uri("https://site-3.example.org"), constructionTree.Defaults.SubjectAlternativeName.Uris.ElementAt(2));

			Assert.AreEqual(3, constructionTree.Defaults.SubjectAlternativeName.UserPrincipalNames.Count);
			Assert.AreEqual("user-1@example.org", constructionTree.Defaults.SubjectAlternativeName.UserPrincipalNames.ElementAt(0));
			Assert.AreEqual("user-2@example.org", constructionTree.Defaults.SubjectAlternativeName.UserPrincipalNames.ElementAt(1));
			Assert.AreEqual("user-3@example.org", constructionTree.Defaults.SubjectAlternativeName.UserPrincipalNames.ElementAt(2));
		}

		protected internal virtual async Task<IConfiguration> CreateConfigurationAsync(string jsonFileName)
		{
			return await Task.FromResult(Global.CreateConfiguration(Path.Combine(_resourcesDirectoryRelativePath, jsonFileName)));
		}

		[TestMethod]
		public async Task ToJson_Test_1()
		{
			var configuration = await this.CreateConfigurationAsync("Configuration-3.json");
			var constructionTree = new CertificateConstructionTreeOptions();
			configuration.Bind(constructionTree);

			var expectedJson = await File.ReadAllTextAsync(Path.Combine(Global.ProjectDirectoryPath, _resourcesDirectoryRelativePath, "Expected-1.json"));

			var json = constructionTree.ToJson();

			Assert.AreEqual(expectedJson, json);
		}

		[TestMethod]
		public async Task ToJson_Test_2()
		{
			var configuration = await this.CreateConfigurationAsync("Configuration-4.json");
			var constructionTree = new CertificateConstructionTreeOptions();
			configuration.Bind(constructionTree);

			var expectedJson = await File.ReadAllTextAsync(Path.Combine(Global.ProjectDirectoryPath, _resourcesDirectoryRelativePath, "Expected-2.json"));

			var json = constructionTree.ToJson();

			Assert.AreEqual(expectedJson, json);
		}

		#endregion
	}
}