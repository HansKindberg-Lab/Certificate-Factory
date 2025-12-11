using System.Security.Cryptography.X509Certificates;
using Application.Models;
using Application.Models.Cryptography;
using Application.Models.Cryptography.Extensions;
using Application.Models.Cryptography.Storing;
using Application.Models.Cryptography.Transferring;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace IntegrationTests.Models.Cryptography.Transferring
{
	public class CertificateExporterTest
	{
		#region Methods

		protected internal virtual async Task<ServiceProvider> CreateServiceProviderAsync()
		{
			var services = Global.CreateServices();

			return await Task.FromResult(services.BuildServiceProvider());
		}

		[Fact]
		public async Task Export_ShouldReturnACertificateExport()
		{
			const string rootSubject = "CN=Root";
			const string subject = "CN=site-1.example.org";

			await using(var serviceProvider = await this.CreateServiceProviderAsync())
			{
				var applicationCertificateStore = serviceProvider.GetRequiredService<IApplicationCertificateStore>();
				var certificateFactory = serviceProvider.GetRequiredService<ICertificateFactory>();
				var systemClock = serviceProvider.GetRequiredService<ISystemClock>();

				var asymmetricAlgorithmOptions = new RsaOptions();

				using(var issuer = certificateFactory.CreateRootCertificate(applicationCertificateStore, asymmetricAlgorithmOptions, null, NullLogger.Instance, rootSubject, systemClock))
				{
					using(var certificate = certificateFactory.CreateTlsCertificate(applicationCertificateStore, asymmetricAlgorithmOptions, ["site-1.example.org", "site-2.example.org", "site-3.example.org"], issuer, null, NullLogger.Instance, subject, systemClock))
					{
						var certificateExporter = serviceProvider.GetRequiredService<ICertificateExporter>();
						var certificateExport = certificateExporter.Export(certificate, "password");
						Assert.NotNull(certificateExport);

						using(var x509Certificate = X509Certificate2.CreateFromPem(certificateExport.CertificatePem))
						{
							Assert.False(x509Certificate.HasPrivateKey);
							Assert.Equal(rootSubject, x509Certificate.Issuer);
							Assert.Equal(subject, x509Certificate.Subject);
						}

						using(var x509Certificate = X509Certificate2.CreateFromPem(certificateExport.CertificatePem, certificateExport.PrivateKeyPem))
						{
							Assert.True(x509Certificate.HasPrivateKey);
							Assert.Equal(rootSubject, x509Certificate.Issuer);
							Assert.Equal(subject, x509Certificate.Subject);
						}

						var certificateAndPrivateKeyPem = certificateExport.CertificatePem + Environment.NewLine + certificateExport.PrivateKeyPem;

						using(var x509Certificate = X509Certificate2.CreateFromPem(certificateAndPrivateKeyPem))
						{
							Assert.False(x509Certificate.HasPrivateKey);
							Assert.Equal(rootSubject, x509Certificate.Issuer);
							Assert.Equal(subject, x509Certificate.Subject);
						}

						using(var x509Certificate = X509Certificate2.CreateFromPem(certificateAndPrivateKeyPem, certificateAndPrivateKeyPem))
						{
							Assert.True(x509Certificate.HasPrivateKey);
							Assert.Equal(rootSubject, x509Certificate.Issuer);
							Assert.Equal(subject, x509Certificate.Subject);
						}
					}
				}
			}
		}

		#endregion
	}
}