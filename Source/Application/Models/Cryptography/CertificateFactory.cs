using System.Security.Cryptography.X509Certificates;
using Application.Models.Cryptography.Configuration;
using Microsoft.Extensions.Options;

namespace Application.Models.Cryptography
{
	/// <inheritdoc />
	public class CertificateFactory(ILoggerFactory loggerFactory, IOptionsMonitor<CertificateFactoryOptions> optionsMonitor, ISystemClock systemClock) : ICertificateFactory
	{
		#region Properties

		protected internal virtual ILoggerFactory LoggerFactory { get; } = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
		protected internal virtual IOptionsMonitor<CertificateFactoryOptions> OptionsMonitor { get; } = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
		protected internal virtual ISystemClock SystemClock { get; } = systemClock ?? throw new ArgumentNullException(nameof(systemClock));

		#endregion

		#region Methods

		public virtual ICertificate Create(IAsymmetricAlgorithmOptions asymmetricAlgorithmOptions, ICertificateOptions certificateOptions)
		{
			ArgumentNullException.ThrowIfNull(asymmetricAlgorithmOptions);
			ArgumentNullException.ThrowIfNull(certificateOptions);

			return asymmetricAlgorithmOptions.CreateCertificate(this.OptionsMonitor, certificateOptions, this.LoggerFactory, this.SystemClock);
		}

		public virtual ICertificate Create(X509Certificate2 certificate, ICertificateStore store)
		{
			ArgumentNullException.ThrowIfNull(certificate);

			return new X509Certificate2Wrapper(certificate, this.LoggerFactory)
			{
				Store = store
			};
		}

		#endregion
	}
}