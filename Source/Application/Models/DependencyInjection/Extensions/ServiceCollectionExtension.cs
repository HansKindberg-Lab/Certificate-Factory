using Application.Models.Configuration;
using Application.Models.Cryptography;
using Application.Models.Cryptography.Archiving;
using Application.Models.Cryptography.Configuration;
using Application.Models.Cryptography.Storing;
using Application.Models.Cryptography.Storing.Configuration;
using Application.Models.Cryptography.Transferring;
using Application.Models.Cryptography.Transferring.Configuration;
using Application.Models.IO;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Application.Models.DependencyInjection.Extensions
{
	public static class ServiceCollectionExtension
	{
		#region Methods

		public static IServiceCollection AddCertificateFactory(this IServiceCollection services, IConfiguration configuration, string applicationCertificateStoreConfigurationKey = nameof(ApplicationCertificateStore), string certificateFactoryConfigurationKey = nameof(CertificateFactory), string certificateFormConfigurationKey = "CertificateForm", string keyExporterConfigurationKey = "KeyExporter")
		{
			ArgumentNullException.ThrowIfNull(services);
			ArgumentNullException.ThrowIfNull(configuration);
			ArgumentNullException.ThrowIfNull(applicationCertificateStoreConfigurationKey);
			ArgumentNullException.ThrowIfNull(certificateFactoryConfigurationKey);
			ArgumentNullException.ThrowIfNull(certificateFormConfigurationKey);
			ArgumentNullException.ThrowIfNull(keyExporterConfigurationKey);

			var applicationCertificateStoreSection = configuration.GetSection(applicationCertificateStoreConfigurationKey);
			services.Configure<ApplicationCertificateStoreOptions>(applicationCertificateStoreSection);

			var certificateFactorySection = configuration.GetSection(certificateFactoryConfigurationKey);
			services.Configure<CertificateFactoryOptions>(certificateFactorySection);

			var certificateFormSection = configuration.GetSection(certificateFormConfigurationKey);
			services.Configure<CertificateFormOptions>(certificateFormSection);

			var keyExporterSection = configuration.GetSection(keyExporterConfigurationKey);
			services.Configure<KeyExporterOptions>(keyExporterSection);

			services.TryAddSingleton<IApplicationCertificateStore, ApplicationCertificateStore>();
			services.TryAddSingleton<IArchiveFactory, ArchiveFactory>();
			services.TryAddSingleton<IAsymmetricAlgorithmRepository, AsymmetricAlgorithmRepository>();
			services.TryAddSingleton<ICertificateConstructionHelper, CertificateConstructionHelper>();
			services.TryAddSingleton<ICertificateExporter, CertificateExporter>();
			services.TryAddSingleton<ICertificateFactory, CertificateFactory>();
			services.TryAddScoped<IFacade, Facade>();
			services.TryAddSingleton<IFileNameResolver, FileNameResolver>();
			services.TryAddSingleton<IKeyExporterFactory, KeyExporterFactory>();
			services.TryAddSingleton<ISystemClock, SystemClock>();

			return services;
		}

		#endregion
	}
}