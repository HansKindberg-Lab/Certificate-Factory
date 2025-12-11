using System.Net;
using System.Security.Cryptography.X509Certificates;
using Application.Models.Cryptography;
using Microsoft.Extensions.Logging.Abstractions;

namespace IntegrationTests.Models.Cryptography
{
	public class X509Certificate2WrapperTest
	{
		#region Methods

		protected internal virtual async Task<X509Certificate2Wrapper> CreateCertificate(string crtFileName)
		{
			await Task.CompletedTask;

			var crtFilePath = Path.Combine(Global.ProjectDirectory.FullName, "Models", "Cryptography", "Resources", "X509Certificate2Wrapper", crtFileName);
			var certificate = X509CertificateLoader.LoadCertificateFromFile(crtFilePath);

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

		[Fact]
		public async Task GetChain_Test()
		{
			await Task.CompletedTask;

			using(var certificateWrapper = new X509Certificate2Wrapper(GetCertificate(), new NullLoggerFactory()))
			{
				var chain = certificateWrapper.GetChain().ToList();

				Assert.Equal(3, chain.Count);

				Assert.Equal(certificateWrapper.WrappedCertificate, ((X509Certificate2Wrapper)chain[0]).WrappedCertificate);
			}
		}

		[Fact]
		public async Task GetChainInternal_Test()
		{
			await Task.CompletedTask;

			using(var certificateWrapper = new X509Certificate2Wrapper(GetCertificate(), new NullLoggerFactory()))
			{
				using(var chain = certificateWrapper.GetChainInternal())
				{
					Assert.False(chain.ChainStatus.Any());

					var certificates = chain.ChainElements.Select(element => element.Certificate).ToList();

					Assert.Equal(3, certificates.Count);

					Assert.Equal(certificateWrapper.WrappedCertificate, certificates[0]);
				}
			}
		}

		[Fact]
		public async Task GetEnhancedKeyUsage_Test()
		{
			await Task.CompletedTask;

			using(var certificateWrapper = new X509Certificate2Wrapper(GetCertificate(), new NullLoggerFactory()))
			{
				Assert.Equal(EnhancedKeyUsage.ClientAuthentication, certificateWrapper.GetEnhancedKeyUsage());
			}
		}

		[Fact]
		public async Task GetSubjectAlternativeName_Test()
		{
			var certificate = await this.CreateCertificate("subject-alternative-name-certificate.crt");

			var subjectAlternativeName = certificate.GetSubjectAlternativeName();

			Assert.NotNull(subjectAlternativeName);

			Assert.Equal(3, subjectAlternativeName.DnsNames.Count);
			Assert.Equal("dns-name-1.example.org", subjectAlternativeName.DnsNames.ElementAt(0));
			Assert.Equal("dns-name-2.example.org", subjectAlternativeName.DnsNames.ElementAt(1));
			Assert.Equal("dns-name-3.example.org", subjectAlternativeName.DnsNames.ElementAt(2));

			Assert.Equal(3, subjectAlternativeName.EmailAddresses.Count);
			Assert.Equal("email-address-1@example.org", subjectAlternativeName.EmailAddresses.ElementAt(0));
			Assert.Equal("email-address-2@example.org", subjectAlternativeName.EmailAddresses.ElementAt(1));
			Assert.Equal("email-address-3@example.org", subjectAlternativeName.EmailAddresses.ElementAt(2));

			Assert.Equal(6, subjectAlternativeName.IpAddresses.Count);
			Assert.Equal(IPAddress.Parse("10.10.10.10"), subjectAlternativeName.IpAddresses.ElementAt(0));
			Assert.Equal(IPAddress.Parse("11.11.11.11"), subjectAlternativeName.IpAddresses.ElementAt(1));
			Assert.Equal(IPAddress.Parse("12.12.12.12"), subjectAlternativeName.IpAddresses.ElementAt(2));
			Assert.Equal(IPAddress.Parse("127.0.0.1"), subjectAlternativeName.IpAddresses.ElementAt(3));
			Assert.Equal(IPAddress.Parse("127.0.0.2"), subjectAlternativeName.IpAddresses.ElementAt(4));
			Assert.Equal(IPAddress.Parse("127.0.0.3"), subjectAlternativeName.IpAddresses.ElementAt(5));

			Assert.Equal(3, subjectAlternativeName.Uris.Count);
			Assert.Equal(new Uri("https://uri-1.example.org"), subjectAlternativeName.Uris.ElementAt(0));
			Assert.Equal(new Uri("https://uri-2.example.org"), subjectAlternativeName.Uris.ElementAt(1));
			Assert.Equal(new Uri("https://uri-3.example.org"), subjectAlternativeName.Uris.ElementAt(2));

			Assert.Equal(3, subjectAlternativeName.UserPrincipalNames.Count);
			Assert.Equal("user-principal-name-1@example.org", subjectAlternativeName.UserPrincipalNames.ElementAt(0));
			Assert.Equal("user-principal-name-2@example.org", subjectAlternativeName.UserPrincipalNames.ElementAt(1));
			Assert.Equal("user-principal-name-3@example.org", subjectAlternativeName.UserPrincipalNames.ElementAt(2));
		}

		#endregion
	}
}