using System.Net;
using System.Security.Cryptography.X509Certificates;
using Application.Models.Cryptography;
using Microsoft.Extensions.Configuration;

namespace IntegrationTests.Models.Cryptography
{
	public class CertificateConstructionTreeOptionsTest
	{
		#region Fields

		private const string _resourcesDirectoryRelativePath = "Models/Cryptography/Resources/CertificateConstructionTreeOptions";

		#endregion

		#region Methods

		[Fact]
		public async Task Configuration_Bind_Test_1()
		{
			var configuration = await this.CreateConfigurationAsync("Configuration-1.json");
			var constructionTree = new CertificateConstructionTreeOptions();
			configuration.Bind(constructionTree);
			Assert.Equal("RSA:2048:Pkcs1", constructionTree.Defaults.AsymmetricAlgorithm);
			Assert.Equal(HashAlgorithm.Sha256, constructionTree.Defaults.HashAlgorithm);
			Assert.Equal(new DateTimeOffset(2050, 1, 1, 0, 0, 0, DateTimeOffset.Parse("2000-01-01", null).Offset), constructionTree.Defaults.NotAfter);
			Assert.Equal("root-certificate", constructionTree.Roots.Keys.ElementAt(0));
			Assert.Equal("CN=Test Root CA", constructionTree.Roots["root-certificate"].Certificate.Subject);
			Assert.True(constructionTree.RootsDefaults.CertificateAuthority.CertificateAuthority);
			Assert.False(constructionTree.RootsDefaults.CertificateAuthority.HasPathLengthConstraint);
			Assert.Equal(0, constructionTree.RootsDefaults.CertificateAuthority.PathLengthConstraint);
			Assert.Equal(X509KeyUsageFlags.KeyCertSign, constructionTree.RootsDefaults.KeyUsage);
		}

		[Fact]
		public async Task Configuration_Bind_Test_2()
		{
			var configuration = await this.CreateConfigurationAsync("Configuration-4.json");
			var constructionTree = new CertificateConstructionTreeOptions();
			configuration.Bind(constructionTree);

			Assert.Equal(3, constructionTree.Defaults.SubjectAlternativeName.DnsNames.Count);
			Assert.Equal("site-1.example.org", constructionTree.Defaults.SubjectAlternativeName.DnsNames.ElementAt(0));
			Assert.Equal("site-2.example.org", constructionTree.Defaults.SubjectAlternativeName.DnsNames.ElementAt(1));
			Assert.Equal("site-3.example.org", constructionTree.Defaults.SubjectAlternativeName.DnsNames.ElementAt(2));

			Assert.Equal(3, constructionTree.Defaults.SubjectAlternativeName.EmailAddresses.Count);
			Assert.Equal("user-1@example.org", constructionTree.Defaults.SubjectAlternativeName.EmailAddresses.ElementAt(0));
			Assert.Equal("user-2@example.org", constructionTree.Defaults.SubjectAlternativeName.EmailAddresses.ElementAt(1));
			Assert.Equal("user-3@example.org", constructionTree.Defaults.SubjectAlternativeName.EmailAddresses.ElementAt(2));

			Assert.Equal(3, constructionTree.Defaults.SubjectAlternativeName.IpAddresses.Count);
			Assert.Equal(IPAddress.Parse("127.0.0.1"), constructionTree.Defaults.SubjectAlternativeName.IpAddresses.ElementAt(0));
			Assert.Equal(IPAddress.Parse("127.0.0.2"), constructionTree.Defaults.SubjectAlternativeName.IpAddresses.ElementAt(1));
			Assert.Equal(IPAddress.Parse("::1"), constructionTree.Defaults.SubjectAlternativeName.IpAddresses.ElementAt(2));

			Assert.Equal(3, constructionTree.Defaults.SubjectAlternativeName.Uris.Count);
			Assert.Equal(new Uri("https://site-1.example.org"), constructionTree.Defaults.SubjectAlternativeName.Uris.ElementAt(0));
			Assert.Equal(new Uri("https://site-2.example.org"), constructionTree.Defaults.SubjectAlternativeName.Uris.ElementAt(1));
			Assert.Equal(new Uri("https://site-3.example.org"), constructionTree.Defaults.SubjectAlternativeName.Uris.ElementAt(2));

			Assert.Equal(3, constructionTree.Defaults.SubjectAlternativeName.UserPrincipalNames.Count);
			Assert.Equal("user-1@example.org", constructionTree.Defaults.SubjectAlternativeName.UserPrincipalNames.ElementAt(0));
			Assert.Equal("user-2@example.org", constructionTree.Defaults.SubjectAlternativeName.UserPrincipalNames.ElementAt(1));
			Assert.Equal("user-3@example.org", constructionTree.Defaults.SubjectAlternativeName.UserPrincipalNames.ElementAt(2));
		}

		protected internal virtual async Task<IConfiguration> CreateConfigurationAsync(string jsonFileName)
		{
			return await Task.FromResult(Global.CreateConfiguration(Path.Combine(_resourcesDirectoryRelativePath, jsonFileName)));
		}

		[Fact]
		public async Task ToJson_Test_1()
		{
			var configuration = await this.CreateConfigurationAsync("Configuration-3.json");
			var constructionTree = new CertificateConstructionTreeOptions();
			configuration.Bind(constructionTree);

			var expectedJson = await File.ReadAllTextAsync(Path.Combine(Global.ProjectDirectory.FullName, _resourcesDirectoryRelativePath, "Expected-1.json"));

			var json = constructionTree.ToJson();

			Assert.Equal(expectedJson, json);
		}

		[Fact]
		public async Task ToJson_Test_2()
		{
			var configuration = await this.CreateConfigurationAsync("Configuration-4.json");
			var constructionTree = new CertificateConstructionTreeOptions();
			configuration.Bind(constructionTree);

			var expectedJson = await File.ReadAllTextAsync(Path.Combine(Global.ProjectDirectory.FullName, _resourcesDirectoryRelativePath, "Expected-2.json"));

			var json = constructionTree.ToJson();

			Assert.Equal(expectedJson, json);
		}

		#endregion
	}
}