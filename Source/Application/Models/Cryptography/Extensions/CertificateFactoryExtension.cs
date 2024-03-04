using System.Diagnostics.CodeAnalysis;
using Application.Models.Collections.Generic.Extensions;
using Application.Models.Cryptography.Storing;
using Microsoft.Extensions.Internal;

namespace Application.Models.Cryptography.Extensions
{
	public static class CertificateFactoryExtension
	{
		#region Methods

		public static IDictionary<string, ICertificate> Create(this ICertificateFactory certificateFactory, IAsymmetricAlgorithmRepository asymmetricAlgorithmRepository, ICertificateConstructionHelper certificateConstructionHelper, CertificateConstructionTreeOptions constructionTree)
		{
			ArgumentNullException.ThrowIfNull(certificateFactory);
			ArgumentNullException.ThrowIfNull(asymmetricAlgorithmRepository);
			ArgumentNullException.ThrowIfNull(certificateConstructionHelper);
			ArgumentNullException.ThrowIfNull(constructionTree);

			try
			{
				if(!constructionTree.Roots.Any())
					throw new ArgumentException("The construction-tree contains no roots.");

				return certificateFactory.Create(asymmetricAlgorithmRepository, certificateConstructionHelper, constructionTree.Defaults, constructionTree.RootsDefaults, constructionTree.Roots);
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException("Could not create certificates from construction-tree.", exception);
			}
		}

		public static IDictionary<string, ICertificate> Create(this ICertificateFactory certificateFactory, IAsymmetricAlgorithmRepository asymmetricAlgorithmRepository, ICertificateConstructionHelper certificateConstructionHelper, CertificateConstructionOptions defaults, CertificateConstructionOptions levelDefaults, IDictionary<string, CertificateConstructionNodeOptions> nodes, ICertificate issuer = null)
		{
			ArgumentNullException.ThrowIfNull(certificateFactory);

			try
			{
				var result = new Dictionary<string, ICertificate>(StringComparer.OrdinalIgnoreCase);

				certificateFactory.Populate(asymmetricAlgorithmRepository, certificateConstructionHelper, defaults, levelDefaults, nodes, result, issuer);

				return result;
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException("Could not create certificates from construction-nodes.", exception);
			}
		}

		[SuppressMessage("Usage", "CA2254:Template should be a static expression")]
		public static ICertificate Create(this ICertificateFactory certificateFactory, IAsymmetricAlgorithmOptions asymmetricAlgorithmOptions, Action<ICertificateOptions> certificateOptionsAction, ICertificateStore certificateStore, ushort? lifetime, ILogger logger, ISystemClock systemClock, string templateName)
		{
			ArgumentNullException.ThrowIfNull(certificateFactory);
			ArgumentNullException.ThrowIfNull(asymmetricAlgorithmOptions);
			ArgumentNullException.ThrowIfNull(certificateOptionsAction);
			ArgumentNullException.ThrowIfNull(certificateStore);
			ArgumentNullException.ThrowIfNull(logger);
			ArgumentNullException.ThrowIfNull(systemClock);
			ArgumentNullException.ThrowIfNull(templateName);

			var templates = certificateStore.Templates();

			if(!templates.TryGetValue(templateName, out var concreteCertificateOptions))
			{
				if(logger.IsEnabled(LogLevel.Warning))
					logger.LogWarning($"Could not find a certificate-template named \"{templateName}\".");
			}

			var certificateOptions = concreteCertificateOptions != null ? concreteCertificateOptions.Clone() : new CertificateOptions();

			if(lifetime != null)
			{
				certificateOptions.NotBefore = systemClock.UtcNow;
				certificateOptions.NotAfter = certificateOptions.NotBefore.Value.AddMonths(lifetime.Value);
			}

			certificateOptionsAction(certificateOptions);

			return certificateFactory.Create(asymmetricAlgorithmOptions, certificateOptions);
		}

		public static ICertificate CreateClientCertificate(this ICertificateFactory certificateFactory, IAsymmetricAlgorithmOptions asymmetricAlgorithmOptions, ICertificateStore certificateStore, ICertificate issuer, ushort? lifetime, ILogger logger, string subject, ISystemClock systemClock)
		{
			ArgumentNullException.ThrowIfNull(certificateFactory);

			void SetCertificateOptions(ICertificateOptions certificateOptions)
			{
				certificateOptions.Issuer = issuer;
				certificateOptions.Subject = subject;
			}

			return certificateFactory.Create(asymmetricAlgorithmOptions, SetCertificateOptions, certificateStore, lifetime, logger, systemClock, "ClientCertificate");
		}

		public static ICertificate CreateIntermediateCertificate(this ICertificateFactory certificateFactory, IAsymmetricAlgorithmOptions asymmetricAlgorithmOptions, ICertificateStore certificateStore, ICertificate issuer, ushort? lifetime, ILogger logger, string subject, ISystemClock systemClock)
		{
			ArgumentNullException.ThrowIfNull(certificateFactory);
			ArgumentNullException.ThrowIfNull(issuer);

			void SetCertificateOptions(ICertificateOptions certificateOptions)
			{
				certificateOptions.Issuer = issuer;
				certificateOptions.Subject = subject;
			}

			return certificateFactory.Create(asymmetricAlgorithmOptions, SetCertificateOptions, certificateStore, lifetime, logger, systemClock, "IntermediateCertificate");
		}

		public static ICertificate CreateRootCertificate(this ICertificateFactory certificateFactory, IAsymmetricAlgorithmOptions asymmetricAlgorithmOptions, ICertificateStore certificateStore, ushort? lifetime, ILogger logger, string subject, ISystemClock systemClock)
		{
			ArgumentNullException.ThrowIfNull(certificateFactory);

			void SetCertificateOptions(ICertificateOptions certificateOptions)
			{
				certificateOptions.Subject = subject;
			}

			return certificateFactory.Create(asymmetricAlgorithmOptions, SetCertificateOptions, certificateStore, lifetime, logger, systemClock, "RootCertificate");
		}

		public static ICertificate CreateTlsCertificate(this ICertificateFactory certificateFactory, IAsymmetricAlgorithmOptions asymmetricAlgorithmOptions, ICertificateStore certificateStore, IEnumerable<string> dnsNames, ICertificate issuer, ushort? lifetime, ILogger logger, string subject, ISystemClock systemClock)
		{
			ArgumentNullException.ThrowIfNull(certificateFactory);

			var dnsNamesCopy = (dnsNames ?? []).ToArray();

			void SetCertificateOptions(ICertificateOptions certificateOptions)
			{
				certificateOptions.Issuer = issuer;
				certificateOptions.Subject = subject;

				if(!dnsNamesCopy.Any())
					return;

				certificateOptions.SubjectAlternativeName ??= new SubjectAlternativeNameOptions();

				certificateOptions.SubjectAlternativeName.DnsNames.Add(dnsNamesCopy);
			}

			return certificateFactory.Create(asymmetricAlgorithmOptions, SetCertificateOptions, certificateStore, lifetime, logger, systemClock, "TlsCertificate");
		}

		private static void Populate(this ICertificateFactory certificateFactory, IAsymmetricAlgorithmRepository asymmetricAlgorithmRepository, ICertificateConstructionHelper certificateConstructionHelper, CertificateConstructionOptions defaults, CertificateConstructionOptions levelDefaults, IDictionary<string, CertificateConstructionNodeOptions> nodes, IDictionary<string, ICertificate> result, ICertificate issuer = null)
		{
			ArgumentNullException.ThrowIfNull(certificateFactory);
			ArgumentNullException.ThrowIfNull(asymmetricAlgorithmRepository);
			ArgumentNullException.ThrowIfNull(certificateConstructionHelper);
			ArgumentNullException.ThrowIfNull(nodes);
			ArgumentNullException.ThrowIfNull(result);

			try
			{
				foreach(var (key, node) in nodes)
				{
					var certificateConstructionOptions = node.Certificate?.Clone() ?? new CertificateConstructionOptions();

					var defaultOptions = new List<CertificateConstructionOptions>();

					if(levelDefaults != null)
						defaultOptions.Add(levelDefaults);

					if(defaults != null)
						defaultOptions.Add(defaults);

					certificateConstructionHelper.SetDefaults(certificateConstructionOptions, defaultOptions);

					if(!asymmetricAlgorithmRepository.Dictionary.TryGetValue(certificateConstructionOptions.AsymmetricAlgorithm, out var asymmetricAlgorithmInformation))
						throw new InvalidOperationException($"Could not get asymmetric algorithm information for key \"{certificateConstructionOptions.AsymmetricAlgorithm}\".");

					var certificateOptions = certificateConstructionHelper.CreateCertificateOptions(certificateConstructionOptions, issuer);

					var certificate = certificateFactory.Create(asymmetricAlgorithmInformation.Options, certificateOptions);

					var certificateKey = key;
					var index = 1;
					while(result.ContainsKey(certificateKey))
					{
						certificateKey = $"{certificateKey}-{index}";
						index++;
					}

					result.Add(certificateKey, certificate);

					certificateFactory.Populate(asymmetricAlgorithmRepository, certificateConstructionHelper, defaults, node.IssuedCertificatesDefaults, node.IssuedCertificates, result, certificate);
				}
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException("Could not populate certificates-result.", exception);
			}
		}

		#endregion
	}
}