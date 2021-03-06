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
using Microsoft.Extensions.Internal;

namespace Application.Models.DependencyInjection.Extensions
{
	public static class ServiceCollectionExtension
	{
		#region Methods

		public static IServiceCollection AddCertificateFactory(this IServiceCollection services, IConfiguration configuration, string certificateFactoryConfigurationKey = nameof(CertificateFactory), string certificateFormConfigurationKey = "CertificateForm", string certificateStoreConfigurationKey = nameof(CertificateStore), string keyExporterConfigurationKey = "KeyExporter")
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			if(configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if(certificateFactoryConfigurationKey == null)
				throw new ArgumentNullException(nameof(certificateFactoryConfigurationKey));

			if(certificateFormConfigurationKey == null)
				throw new ArgumentNullException(nameof(certificateFormConfigurationKey));

			if(certificateStoreConfigurationKey == null)
				throw new ArgumentNullException(nameof(certificateStoreConfigurationKey));

			if(keyExporterConfigurationKey == null)
				throw new ArgumentNullException(nameof(keyExporterConfigurationKey));

			var certificateFactorySection = configuration.GetSection(certificateFactoryConfigurationKey);
			services.Configure<CertificateFactoryOptions>(certificateFactorySection);

			var certificateFormSection = configuration.GetSection(certificateFormConfigurationKey);
			services.Configure<CertificateFormOptions>(certificateFormSection);

			var certificateStoreSection = configuration.GetSection(certificateStoreConfigurationKey);
			services.Configure<CertificateStoreOptions>(certificateStoreSection);

			var keyExporterSection = configuration.GetSection(keyExporterConfigurationKey);
			services.Configure<KeyExporterOptions>(keyExporterSection);

			services.TryAddSingleton<IArchiveFactory, ArchiveFactory>();
			services.TryAddSingleton<ICertificateExporter, CertificateExporter>();
			services.TryAddSingleton<ICertificateFactory, CertificateFactory>();
			services.TryAddSingleton<ICertificateStore, CertificateStore>();
			services.TryAddScoped<IFacade, Facade>();
			services.TryAddSingleton<IFileNameResolver, FileNameResolver>();
			services.TryAddSingleton<IKeyExporterFactory, KeyExporterFactory>();
			services.TryAddSingleton<ISystemClock, SystemClock>();

			return services;
		}

		#endregion
	}
}