using System.Net;
using System.Security.Cryptography.X509Certificates;
using Application.Models.Cryptography;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Models.Cryptography
{
	[TestClass]
	public class X509Certificate2WrapperTest
	{
		#region Methods

		protected internal virtual async Task<X509Certificate2Wrapper> CreateCertificate(string crtFileName)
		{
			await Task.CompletedTask;

			var crtFilePath = Path.Combine(Global.ProjectDirectoryPath, "Models", "Cryptography", "Resources", "X509Certificate2Wrapper", crtFileName);
			var certificate = new X509Certificate2(crtFilePath);

			return new X509Certificate2Wrapper(certificate, new NullLoggerFactory());
		}

		private static X509Certificate2 GetCertificate()
		{
			using(var store = new X509Store(StoreName.My, StoreLocation.CurrentUser))
			{
				store.Open(OpenFlags.ReadOnly);

				if(store.Certificates.Any())
					return store.Certificates[0];
			}

			using(var store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
			{
				store.Open(OpenFlags.ReadOnly);

				if(store.Certificates.Any())
					return store.Certificates[0];
			}

			throw new InvalidOperationException("Could not get a certificate.");
		}

		[TestMethod]
		public async Task GetChain_Test()
		{
			await Task.CompletedTask;

			using(var certificateWrapper = new X509Certificate2Wrapper(GetCertificate(), new NullLoggerFactory()))
			{
				var chain = certificateWrapper.GetChain().ToList();

				Assert.AreEqual(3, chain.Count);

				Assert.AreEqual(certificateWrapper.WrappedCertificate, ((X509Certificate2Wrapper)chain[0]).WrappedCertificate);
			}
		}

		[TestMethod]
		public async Task GetChainInternal_Test()
		{
			await Task.CompletedTask;

			using(var certificateWrapper = new X509Certificate2Wrapper(GetCertificate(), new NullLoggerFactory()))
			{
				using(var chain = certificateWrapper.GetChainInternal())
				{
					Assert.IsFalse(chain.ChainStatus.Any());

					var certificates = chain.ChainElements.Select(element => element.Certificate).ToList();

					Assert.AreEqual(3, certificates.Count);

					Assert.AreEqual(certificateWrapper.WrappedCertificate, certificates[0]);
				}
			}
		}

		[TestMethod]
		public async Task GetEnhancedKeyUsage_Test()
		{
			await Task.CompletedTask;

			using(var certificateWrapper = new X509Certificate2Wrapper(GetCertificate(), new NullLoggerFactory()))
			{
				Assert.AreEqual(EnhancedKeyUsage.ClientAuthentication, certificateWrapper.GetEnhancedKeyUsage());
			}
		}

		[TestMethod]
		public async Task GetSubjectAlternativeName_Test()
		{
			var certificate = await this.CreateCertificate("subject-alternative-name-certificate.crt");

			var subjectAlternativeName = certificate.GetSubjectAlternativeName();

			Assert.IsNotNull(subjectAlternativeName);

			Assert.AreEqual(3, subjectAlternativeName.DnsNames.Count);
			Assert.AreEqual("dns-name-1.example.org", subjectAlternativeName.DnsNames.ElementAt(0));
			Assert.AreEqual("dns-name-2.example.org", subjectAlternativeName.DnsNames.ElementAt(1));
			Assert.AreEqual("dns-name-3.example.org", subjectAlternativeName.DnsNames.ElementAt(2));

			Assert.AreEqual(3, subjectAlternativeName.EmailAddresses.Count);
			Assert.AreEqual("email-address-1@example.org", subjectAlternativeName.EmailAddresses.ElementAt(0));
			Assert.AreEqual("email-address-2@example.org", subjectAlternativeName.EmailAddresses.ElementAt(1));
			Assert.AreEqual("email-address-3@example.org", subjectAlternativeName.EmailAddresses.ElementAt(2));

			Assert.AreEqual(6, subjectAlternativeName.IpAddresses.Count);
			Assert.AreEqual(IPAddress.Parse("10.10.10.10"), subjectAlternativeName.IpAddresses.ElementAt(0));
			Assert.AreEqual(IPAddress.Parse("11.11.11.11"), subjectAlternativeName.IpAddresses.ElementAt(1));
			Assert.AreEqual(IPAddress.Parse("12.12.12.12"), subjectAlternativeName.IpAddresses.ElementAt(2));
			Assert.AreEqual(IPAddress.Parse("127.0.0.1"), subjectAlternativeName.IpAddresses.ElementAt(3));
			Assert.AreEqual(IPAddress.Parse("127.0.0.2"), subjectAlternativeName.IpAddresses.ElementAt(4));
			Assert.AreEqual(IPAddress.Parse("127.0.0.3"), subjectAlternativeName.IpAddresses.ElementAt(5));

			Assert.AreEqual(3, subjectAlternativeName.Uris.Count);
			Assert.AreEqual(new Uri("https://uri-1.example.org"), subjectAlternativeName.Uris.ElementAt(0));
			Assert.AreEqual(new Uri("https://uri-2.example.org"), subjectAlternativeName.Uris.ElementAt(1));
			Assert.AreEqual(new Uri("https://uri-3.example.org"), subjectAlternativeName.Uris.ElementAt(2));

			Assert.AreEqual(3, subjectAlternativeName.UserPrincipalNames.Count);
			Assert.AreEqual("user-principal-name-1@example.org", subjectAlternativeName.UserPrincipalNames.ElementAt(0));
			Assert.AreEqual("user-principal-name-2@example.org", subjectAlternativeName.UserPrincipalNames.ElementAt(1));
			Assert.AreEqual("user-principal-name-3@example.org", subjectAlternativeName.UserPrincipalNames.ElementAt(2));
		}

		#endregion
	}
}