using System.Diagnostics.CodeAnalysis;
using Application.Models.Collections.Generic.Extensions;
using Application.Models.Cryptography.Storing;
using Microsoft.Extensions.Internal;

namespace Application.Models.Cryptography.Extensions
{
	public static class CertificateFactoryExtension
	{
		#region Methods

		[SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates")]
		[SuppressMessage("Usage", "CA2254:Template should be a static expression")]
		public static ICertificate Create(this ICertificateFactory certificateFactory, IAsymmetricAlgorithmOptions asymmetricAlgorithmOptions, Action<ICertificateOptions> certificateOptionsAction, ICertificateStore certificateStore, ushort? lifetime, ILogger logger, ISystemClock systemClock, string templateName)
		{
			if(certificateFactory == null)
				throw new ArgumentNullException(nameof(certificateFactory));

			if(asymmetricAlgorithmOptions == null)
				throw new ArgumentNullException(nameof(asymmetricAlgorithmOptions));

			if(certificateOptionsAction == null)
				throw new ArgumentNullException(nameof(certificateOptionsAction));

			if(certificateStore == null)
				throw new ArgumentNullException(nameof(certificateStore));

			if(logger == null)
				throw new ArgumentNullException(nameof(logger));

			if(systemClock == null)
				throw new ArgumentNullException(nameof(systemClock));

			if(templateName == null)
				throw new ArgumentNullException(nameof(templateName));

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
			if(certificateFactory == null)
				throw new ArgumentNullException(nameof(certificateFactory));

			void SetCertificateOptions(ICertificateOptions certificateOptions)
			{
				certificateOptions.Issuer = issuer;
				certificateOptions.Subject = subject;
			}

			return certificateFactory.Create(asymmetricAlgorithmOptions, SetCertificateOptions, certificateStore, lifetime, logger, systemClock, "ClientCertificate");
		}

		public static ICertificate CreateIntermediateCertificate(this ICertificateFactory certificateFactory, IAsymmetricAlgorithmOptions asymmetricAlgorithmOptions, ICertificateStore certificateStore, ICertificate issuer, ushort? lifetime, ILogger logger, string subject, ISystemClock systemClock)
		{
			if(certificateFactory == null)
				throw new ArgumentNullException(nameof(certificateFactory));

			if(issuer == null)
				throw new ArgumentNullException(nameof(issuer));

			void SetCertificateOptions(ICertificateOptions certificateOptions)
			{
				certificateOptions.Issuer = issuer;
				certificateOptions.Subject = subject;
			}

			return certificateFactory.Create(asymmetricAlgorithmOptions, SetCertificateOptions, certificateStore, lifetime, logger, systemClock, "IntermediateCertificate");
		}

		public static ICertificate CreateRootCertificate(this ICertificateFactory certificateFactory, IAsymmetricAlgorithmOptions asymmetricAlgorithmOptions, ICertificateStore certificateStore, ushort? lifetime, ILogger logger, string subject, ISystemClock systemClock)
		{
			if(certificateFactory == null)
				throw new ArgumentNullException(nameof(certificateFactory));

			void SetCertificateOptions(ICertificateOptions certificateOptions)
			{
				certificateOptions.Subject = subject;
			}

			return certificateFactory.Create(asymmetricAlgorithmOptions, SetCertificateOptions, certificateStore, lifetime, logger, systemClock, "RootCertificate");
		}

		public static ICertificate CreateTlsCertificate(this ICertificateFactory certificateFactory, IAsymmetricAlgorithmOptions asymmetricAlgorithmOptions, ICertificateStore certificateStore, IEnumerable<string> dnsNames, ICertificate issuer, ushort? lifetime, ILogger logger, string subject, ISystemClock systemClock)
		{
			if(certificateFactory == null)
				throw new ArgumentNullException(nameof(certificateFactory));

			var dnsNamesCopy = (dnsNames ?? Enumerable.Empty<string>()).ToArray();

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

		#endregion
	}
}