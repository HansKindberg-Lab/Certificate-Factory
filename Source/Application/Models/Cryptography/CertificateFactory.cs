using Application.Models.Cryptography.Configuration;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;

namespace Application.Models.Cryptography
{
	/// <inheritdoc />
	public class CertificateFactory : ICertificateFactory
	{
		#region Constructors

		public CertificateFactory(IOptionsMonitor<CertificateFactoryOptions> optionsMonitor, ISystemClock systemClock)
		{
			this.OptionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
			this.SystemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
		}

		#endregion

		#region Properties

		protected internal virtual IOptionsMonitor<CertificateFactoryOptions> OptionsMonitor { get; }
		protected internal virtual ISystemClock SystemClock { get; }

		#endregion

		#region Methods

		public virtual ICertificate Create(IAsymmetricAlgorithmOptions asymmetricAlgorithmOptions, ICertificateOptions certificateOptions)
		{
			if(asymmetricAlgorithmOptions == null)
				throw new ArgumentNullException(nameof(asymmetricAlgorithmOptions));

			if(certificateOptions == null)
				throw new ArgumentNullException(nameof(certificateOptions));

			return asymmetricAlgorithmOptions.CreateCertificate(this.OptionsMonitor, certificateOptions, this.SystemClock);
		}

		#endregion
	}
}