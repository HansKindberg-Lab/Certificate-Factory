using Application.Models.Cryptography;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Models.Cryptography
{
	[TestClass]
	public class CertificateFactoryTest
	{
		#region Methods

		[TestMethod]
		public async Task Create_ShouldReturnACertificate()
		{
			await using(var serviceProvider = await this.CreateServiceProviderAsync())
			{
				var certificateFactory = serviceProvider.GetRequiredService<ICertificateFactory>();

				var rsaOptions = new RsaOptions();

				var certificateOptions = new CertificateOptions
				{
					NotAfter = new DateTimeOffset(2030, 1, 1, 0, 0, 0, TimeSpan.Zero),
					NotBefore = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero),
					SerialNumber = new SerialNumberOptions
					{
						Value = "00d63c23df87a790dc4ed72c09172487ea8bde25a3"
					},
					Subject = "CN=Test"
				};

				using(var certificate = certificateFactory.Create(rsaOptions, certificateOptions))
				{
					Assert.IsNotNull(certificate);
					Assert.AreEqual("00D63C23DF87A790DC4ED72C09172487EA8BDE25A3", certificate.SerialNumber);
					//Assert.AreEqual(string.Empty, certificate.ToString());
				}
			}
		}

		protected internal virtual async Task<ServiceProvider> CreateServiceProviderAsync()
		{
			var services = Global.CreateServices();

			return await Task.FromResult(services.BuildServiceProvider());
		}

		#endregion
	}
}