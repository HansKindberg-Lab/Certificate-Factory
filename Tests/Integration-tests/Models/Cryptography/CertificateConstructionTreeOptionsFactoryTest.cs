using System.Security.Cryptography.X509Certificates;
using Application.Models.Cryptography;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Models.Cryptography
{
	[TestClass]
	public class CertificateConstructionTreeOptionsFactoryTest
	{
		#region Methods

		[TestMethod]
		public async Task Create_Test()
		{
			await Task.CompletedTask;

			var loggerFactory = new NullLoggerFactory();

			using(var certificateWrapper = new X509Certificate2Wrapper(GetCertificate(), loggerFactory))
			{
				var constructionTree = new CertificateConstructionTreeOptionsFactory(loggerFactory).Create(certificateWrapper);

				Assert.IsNotNull(constructionTree);
				Assert.AreEqual(1, constructionTree.Roots.Count);
			}
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

		#endregion
	}
}