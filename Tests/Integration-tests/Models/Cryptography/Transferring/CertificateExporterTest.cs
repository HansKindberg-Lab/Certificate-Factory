using System.Security.Cryptography.X509Certificates;
using Application.Models.Cryptography;
using Application.Models.Cryptography.Extensions;
using Application.Models.Cryptography.Storing;
using Application.Models.Cryptography.Transferring;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Models.Cryptography.Transferring
{
	[TestClass]
	public class CertificateExporterTest
	{
		#region Methods

		protected internal virtual async Task<ServiceProvider> CreateServiceProviderAsync()
		{
			var services = Global.CreateServices();

			return await Task.FromResult(services.BuildServiceProvider());
		}

		[TestMethod]
		public async Task Export_ShouldReturnACertificateExport()
		{
			const string rootSubject = "CN=Root";
			const string subject = "CN=site-1.example.org";

			await using(var serviceProvider = await this.CreateServiceProviderAsync())
			{
				var certificateFactory = serviceProvider.GetRequiredService<ICertificateFactory>();
				var certificateStore = serviceProvider.GetRequiredService<ICertificateStore>();
				var systemClock = serviceProvider.GetRequiredService<ISystemClock>();

				var asymmetricAlgorithmOptions = new RsaOptions();

				using(var issuer = certificateFactory.CreateRootCertificate(asymmetricAlgorithmOptions, certificateStore, null, NullLogger.Instance, rootSubject, systemClock))
				{
					using(var certificate = certificateFactory.CreateTlsCertificate(asymmetricAlgorithmOptions, certificateStore, ["site-1.example.org", "site-2.example.org", "site-3.example.org"], issuer, null, NullLogger.Instance, subject, systemClock))
					{
						var certificateExporter = serviceProvider.GetRequiredService<ICertificateExporter>();
						var certificateExport = certificateExporter.Export(certificate, "password");
						Assert.IsNotNull(certificateExport);

						using(var x509Certificate = X509Certificate2.CreateFromPem(certificateExport.CertificatePem))
						{
							Assert.IsFalse(x509Certificate.HasPrivateKey);
							Assert.AreEqual(rootSubject, x509Certificate.Issuer);
							Assert.AreEqual(subject, x509Certificate.Subject);
						}

						using(var x509Certificate = X509Certificate2.CreateFromPem(certificateExport.CertificatePem, certificateExport.PrivateKeyPem))
						{
							Assert.IsTrue(x509Certificate.HasPrivateKey);
							Assert.AreEqual(rootSubject, x509Certificate.Issuer);
							Assert.AreEqual(subject, x509Certificate.Subject);
						}

						var certificateAndPrivateKeyPem = certificateExport.CertificatePem + Environment.NewLine + certificateExport.PrivateKeyPem;

						using(var x509Certificate = X509Certificate2.CreateFromPem(certificateAndPrivateKeyPem))
						{
							Assert.IsFalse(x509Certificate.HasPrivateKey);
							Assert.AreEqual(rootSubject, x509Certificate.Issuer);
							Assert.AreEqual(subject, x509Certificate.Subject);
						}

						using(var x509Certificate = X509Certificate2.CreateFromPem(certificateAndPrivateKeyPem, certificateAndPrivateKeyPem))
						{
							Assert.IsTrue(x509Certificate.HasPrivateKey);
							Assert.AreEqual(rootSubject, x509Certificate.Issuer);
							Assert.AreEqual(subject, x509Certificate.Subject);
						}
					}
				}
			}
		}

		#endregion
	}
}