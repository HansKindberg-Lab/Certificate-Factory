using Application.Models.Cryptography.Configuration;
using Microsoft.Extensions.Options;

namespace Application.Models.Cryptography
{
	public interface IAsymmetricAlgorithmOptions : ICloneable<IAsymmetricAlgorithmOptions>
	{
		#region Methods

		ICertificate CreateCertificate(IOptionsMonitor<CertificateFactoryOptions> certificateFactoryOptionsMonitor, ICertificateOptions certificateOptions, ILoggerFactory loggerFactory, ISystemClock systemClock);

		#endregion
	}
}