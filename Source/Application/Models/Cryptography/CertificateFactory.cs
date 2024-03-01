using Application.Models.Cryptography.Configuration;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;

namespace Application.Models.Cryptography
{
	/// <inheritdoc />
	public class CertificateFactory(IOptionsMonitor<CertificateFactoryOptions> optionsMonitor, ISystemClock systemClock) : ICertificateFactory
	{
		#region Properties

		protected internal virtual IOptionsMonitor<CertificateFactoryOptions> OptionsMonitor { get; } = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
		protected internal virtual ISystemClock SystemClock { get; } = systemClock ?? throw new ArgumentNullException(nameof(systemClock));

		#endregion

		#region Methods

		public virtual ICertificate Create(IAsymmetricAlgorithmOptions asymmetricAlgorithmOptions, ICertificateOptions certificateOptions)
		{
			ArgumentNullException.ThrowIfNull(asymmetricAlgorithmOptions);
			ArgumentNullException.ThrowIfNull(certificateOptions);

			return asymmetricAlgorithmOptions.CreateCertificate(this.OptionsMonitor, certificateOptions, this.SystemClock);
		}

		#endregion
	}
}