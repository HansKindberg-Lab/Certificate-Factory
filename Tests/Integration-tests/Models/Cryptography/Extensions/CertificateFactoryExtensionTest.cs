using System.Security.Cryptography.X509Certificates;
using Application.Models.Cryptography;
using Application.Models.Cryptography.Extensions;
using Application.Models.Cryptography.Storing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Models.Cryptography.Extensions
{
	[TestClass]
	public class CertificateFactoryExtensionTest
	{
		#region Methods

		[TestMethod]
		public async Task CreateClientCertificate_ShouldReturnAClientCertificate()
		{
			const string rootSubject = "CN=Root";
			const string subject = "CN=Bob Bobson, E=bob.bobson@example.org, G=Bob, SERIALNUMBER=123456789, SN=Bobson";

			await using(var serviceProvider = await this.CreateServiceProviderAsync())
			{
				var applicationCertificateStore = serviceProvider.GetRequiredService<IApplicationCertificateStore>();
				var certificateFactory = serviceProvider.GetRequiredService<ICertificateFactory>();
				var systemClock = serviceProvider.GetRequiredService<ISystemClock>();

				var rsaOptions = new RsaOptions();

				using(var issuer = certificateFactory.CreateRootCertificate(applicationCertificateStore, rsaOptions, null, NullLogger.Instance, rootSubject, systemClock))
				{
					using(var certificate = certificateFactory.CreateClientCertificate(applicationCertificateStore, rsaOptions, issuer, null, NullLogger.Instance, subject, systemClock))
					{
						await this.ValidateClientCertificate(certificate, issuer, rootSubject, subject);
					}
				}

				var ecdsaOptions = new EcdsaOptions();

				using(var issuer = certificateFactory.CreateRootCertificate(applicationCertificateStore, ecdsaOptions, null, NullLogger.Instance, rootSubject, systemClock))
				{
					using(var certificate = certificateFactory.CreateClientCertificate(applicationCertificateStore, ecdsaOptions, issuer, null, NullLogger.Instance, subject, systemClock))
					{
						await this.ValidateClientCertificate(certificate, issuer, rootSubject, subject);
					}
				}
			}
		}

		[TestMethod]
		public async Task CreateIntermediateCertificate_ShouldReturnAnIntermediateCertificate()
		{
			const string rootSubject = "CN=Root";
			const string subject = "CN=Intermediate";

			await using(var serviceProvider = await this.CreateServiceProviderAsync())
			{
				var applicationCertificateStore = serviceProvider.GetRequiredService<IApplicationCertificateStore>();
				var certificateFactory = serviceProvider.GetRequiredService<ICertificateFactory>();
				var systemClock = serviceProvider.GetRequiredService<ISystemClock>();

				var rsaOptions = new RsaOptions();

				using(var issuer = certificateFactory.CreateRootCertificate(applicationCertificateStore, rsaOptions, null, NullLogger.Instance, rootSubject, systemClock))
				{
					using(var certificate = certificateFactory.CreateIntermediateCertificate(applicationCertificateStore, rsaOptions, issuer, null, NullLogger.Instance, subject, systemClock))
					{
						await this.ValidateIntermediateCertificate(certificate, issuer, rootSubject, subject);
					}
				}

				var ecdsaOptions = new EcdsaOptions();

				using(var issuer = certificateFactory.CreateRootCertificate(applicationCertificateStore, ecdsaOptions, null, NullLogger.Instance, rootSubject, systemClock))
				{
					using(var certificate = certificateFactory.CreateIntermediateCertificate(applicationCertificateStore, ecdsaOptions, issuer, null, NullLogger.Instance, subject, systemClock))
					{
						await this.ValidateIntermediateCertificate(certificate, issuer, rootSubject, subject);
					}
				}
			}
		}

		[TestMethod]
		public async Task CreateRootCertificate_ShouldReturnARootCertificate()
		{
			const string subject = "CN=Root";

			await using(var serviceProvider = await this.CreateServiceProviderAsync())
			{
				var applicationCertificateStore = serviceProvider.GetRequiredService<IApplicationCertificateStore>();
				var certificateFactory = serviceProvider.GetRequiredService<ICertificateFactory>();
				var systemClock = serviceProvider.GetRequiredService<ISystemClock>();

				var rsaOptions = new RsaOptions();

				using(var certificate = certificateFactory.CreateRootCertificate(applicationCertificateStore, rsaOptions, null, NullLogger.Instance, subject, systemClock))
				{
					await this.ValidateRootCertificate(certificate, subject);
				}

				var ecdsaOptions = new EcdsaOptions();

				using(var certificate = certificateFactory.CreateRootCertificate(applicationCertificateStore, ecdsaOptions, null, NullLogger.Instance, subject, systemClock))
				{
					await this.ValidateRootCertificate(certificate, subject);
				}
			}
		}

		protected internal virtual async Task<ServiceProvider> CreateServiceProviderAsync()
		{
			var services = Global.CreateServices();

			return await Task.FromResult(services.BuildServiceProvider());
		}

		[TestMethod]
		public async Task CreateTlsCertificate_ShouldReturnATlsCertificate()
		{
			const string rootSubject = "CN=Root";
			const string subject = "CN=site-1.example.org";

			await using(var serviceProvider = await this.CreateServiceProviderAsync())
			{
				var applicationCertificateStore = serviceProvider.GetRequiredService<IApplicationCertificateStore>();
				var certificateFactory = serviceProvider.GetRequiredService<ICertificateFactory>();
				var systemClock = serviceProvider.GetRequiredService<ISystemClock>();

				var rsaOptions = new RsaOptions();

				using(var issuer = certificateFactory.CreateRootCertificate(applicationCertificateStore, rsaOptions, null, NullLogger.Instance, rootSubject, systemClock))
				{
					using(var certificate = certificateFactory.CreateTlsCertificate(applicationCertificateStore, rsaOptions, ["site-1.example.org", "site-2.example.org", "site-3.example.org"], issuer, null, NullLogger.Instance, subject, systemClock))
					{
						await this.ValidateTlsCertificate(certificate, issuer, rootSubject, subject);
					}
				}

				var ecdsaOptions = new EcdsaOptions();

				using(var issuer = certificateFactory.CreateRootCertificate(applicationCertificateStore, ecdsaOptions, null, NullLogger.Instance, rootSubject, systemClock))
				{
					using(var certificate = certificateFactory.CreateTlsCertificate(applicationCertificateStore, ecdsaOptions, [subject, "site-2.example.org", "site-3.example.org"], issuer, null, NullLogger.Instance, subject, systemClock))
					{
						await this.ValidateTlsCertificate(certificate, issuer, rootSubject, subject);
					}
				}
			}
		}

		protected internal virtual async Task ValidateClientCertificate(ICertificate certificate, ICertificate issuer, string rootSubject, string subject)
		{
			await Task.CompletedTask;

			Assert.IsNotNull(certificate);
			Assert.IsFalse(certificate.Archived);
			Assert.IsTrue(certificate.HasPrivateKey);
			Assert.AreEqual(rootSubject, certificate.Issuer);
			Assert.IsNotNull(issuer);
			Assert.AreEqual(issuer.NotAfter, certificate.NotAfter);
			Assert.IsTrue(issuer.NotBefore <= certificate.NotBefore);
			Assert.AreEqual(subject, certificate.Subject);

			var wrappedCertificate = certificate.Unwrap();
			Assert.AreEqual(2, wrappedCertificate.Extensions.Count);
			var extensions = wrappedCertificate.Extensions.ToList();

			var enhancedKeyUsageExtension = extensions[0] as X509EnhancedKeyUsageExtension;
			Assert.IsNotNull(enhancedKeyUsageExtension);
			Assert.IsTrue(enhancedKeyUsageExtension.Critical);
			Assert.AreEqual(1, enhancedKeyUsageExtension.EnhancedKeyUsages.Count);
			Assert.AreEqual("1.3.6.1.5.5.7.3.2", enhancedKeyUsageExtension.EnhancedKeyUsages[0]?.Value);

			var keyUsageExtension = extensions[1] as X509KeyUsageExtension;
			Assert.IsNotNull(keyUsageExtension);
			Assert.AreEqual(X509KeyUsageFlags.DigitalSignature, keyUsageExtension.KeyUsages);

			//Assert.AreEqual(string.Empty, certificate.ToString(true));
		}

		protected internal virtual async Task ValidateIntermediateCertificate(ICertificate certificate, ICertificate issuer, string rootSubject, string subject)
		{
			await Task.CompletedTask;

			Assert.IsNotNull(certificate);
			Assert.IsFalse(certificate.Archived);
			Assert.IsTrue(certificate.HasPrivateKey);
			Assert.AreEqual(rootSubject, certificate.Issuer);
			Assert.IsNotNull(issuer);
			Assert.AreEqual(certificate.Issuer, issuer.Subject);
			Assert.AreEqual(subject, certificate.Subject);
			Assert.IsNotNull(certificate.Issuer);

			var wrappedCertificate = certificate.Unwrap();
			Assert.AreEqual(2, wrappedCertificate.Extensions.Count);
			var extensions = wrappedCertificate.Extensions.ToList();

			var basicConstraintsExtension = extensions[0] as X509BasicConstraintsExtension;
			Assert.IsNotNull(basicConstraintsExtension);
			Assert.IsTrue(basicConstraintsExtension.CertificateAuthority);
			Assert.IsTrue(basicConstraintsExtension.Critical);
			Assert.IsTrue(basicConstraintsExtension.HasPathLengthConstraint);
			Assert.AreEqual(0, basicConstraintsExtension.PathLengthConstraint);

			var keyUsageExtension = extensions[1] as X509KeyUsageExtension;
			Assert.IsNotNull(keyUsageExtension);
			Assert.AreEqual(X509KeyUsageFlags.KeyCertSign, keyUsageExtension.KeyUsages);

			//Assert.AreEqual(string.Empty, certificate.ToString(true));
		}

		protected internal virtual async Task ValidateRootCertificate(ICertificate certificate, string subject)
		{
			await Task.CompletedTask;

			Assert.IsNotNull(certificate);
			Assert.IsFalse(certificate.Archived);
			Assert.IsTrue(certificate.HasPrivateKey);
			Assert.AreEqual(subject, certificate.Subject);

			var wrappedCertificate = certificate.Unwrap();
			Assert.AreEqual(2, wrappedCertificate.Extensions.Count);
			var extensions = wrappedCertificate.Extensions.ToList();

			var basicConstraintsExtension = extensions[0] as X509BasicConstraintsExtension;
			Assert.IsNotNull(basicConstraintsExtension);
			Assert.IsTrue(basicConstraintsExtension.CertificateAuthority);
			Assert.IsTrue(basicConstraintsExtension.Critical);
			Assert.IsFalse(basicConstraintsExtension.HasPathLengthConstraint);
			Assert.AreEqual(0, basicConstraintsExtension.PathLengthConstraint);

			var keyUsageExtension = extensions[1] as X509KeyUsageExtension;
			Assert.IsNotNull(keyUsageExtension);
			Assert.AreEqual(X509KeyUsageFlags.KeyCertSign, keyUsageExtension.KeyUsages);

			//Assert.AreEqual(string.Empty, certificate.ToString(true));
		}

		protected internal virtual async Task ValidateTlsCertificate(ICertificate certificate, ICertificate issuer, string rootSubject, string subject)
		{
			await Task.CompletedTask;

			Assert.IsNotNull(certificate);
			Assert.IsFalse(certificate.Archived);
			Assert.IsTrue(certificate.HasPrivateKey);
			Assert.AreEqual(rootSubject, certificate.Issuer);
			Assert.AreEqual(issuer.NotAfter, certificate.NotAfter);
			Assert.IsTrue(issuer.NotBefore <= certificate.NotBefore);
			Assert.AreEqual(subject, certificate.Subject);

			var wrappedCertificate = certificate.Unwrap();
			Assert.AreEqual(3, wrappedCertificate.Extensions.Count);
			var extensions = wrappedCertificate.Extensions.ToList();

			var extension = extensions[0];
			Assert.IsNotNull(extension);
			Assert.IsFalse(extension.Critical);
			Assert.AreEqual("2.5.29.17", extension.Oid?.Value);

			var enhancedKeyUsageExtension = extensions[1] as X509EnhancedKeyUsageExtension;
			Assert.IsNotNull(enhancedKeyUsageExtension);
			Assert.IsTrue(enhancedKeyUsageExtension.Critical);
			Assert.AreEqual(1, enhancedKeyUsageExtension.EnhancedKeyUsages.Count);
			Assert.AreEqual("1.3.6.1.5.5.7.3.1", enhancedKeyUsageExtension.EnhancedKeyUsages[0]?.Value);

			var keyUsageExtension = extensions[2] as X509KeyUsageExtension;
			Assert.IsNotNull(keyUsageExtension);
			Assert.AreEqual(X509KeyUsageFlags.DigitalSignature, keyUsageExtension.KeyUsages);

			//Assert.AreEqual(string.Empty, certificate.ToString(true));
		}

		#endregion
	}
}