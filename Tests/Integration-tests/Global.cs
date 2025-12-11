using Application.Models.DependencyInjection.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace IntegrationTests
{
	public static class Global
	{
		#region Fields

		public static readonly DirectoryInfo ProjectDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent!.Parent!.Parent!;

		#endregion

		#region Properties

		public static IConfiguration Configuration => field ??= CreateConfiguration("appsettings.json");
		public static IHostEnvironment HostEnvironment => field ??= CreateHostEnvironment("Integration-tests");

		#endregion

		#region Methods

		public static IConfiguration CreateConfiguration(params string[] jsonFilePaths)
		{
			return CreateConfigurationBuilder(jsonFilePaths).Build();
		}

		public static IConfigurationBuilder CreateConfigurationBuilder(params string[] jsonFilePaths)
		{
			var configurationBuilder = new ConfigurationBuilder();
			configurationBuilder.Properties.Add("FileProvider", HostEnvironment.ContentRootFileProvider);

			foreach(var path in jsonFilePaths ?? [])
			{
				configurationBuilder.AddJsonFile(path, false, true);
			}

			return configurationBuilder;
		}

		public static IHostEnvironment CreateHostEnvironment(string environmentName)
		{
			return new HostingEnvironment
			{
				ApplicationName = typeof(Global)!.Assembly!.GetName()!.Name!,
				ContentRootFileProvider = new PhysicalFileProvider(ProjectDirectory.FullName),
				ContentRootPath = ProjectDirectory.FullName,
				EnvironmentName = environmentName
			};
		}

		public static IServiceCollection CreateServices()
		{
			return CreateServices(Configuration);
		}

		public static IServiceCollection CreateServices(IConfiguration configuration)
		{
			var services = new ServiceCollection();

			services.AddSingleton(configuration);
			services.AddSingleton(HostEnvironment);
			services.AddSingleton<ILoggerFactory, NullLoggerFactory>();
			services.AddCertificateFactory(configuration);

			return services;
		}

		#endregion
	}
}