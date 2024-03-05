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

		#endregion
	}
}