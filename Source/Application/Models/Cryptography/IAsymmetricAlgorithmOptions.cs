using Application.Models.Cryptography.Configuration;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;

namespace Application.Models.Cryptography
{
	public interface IAsymmetricAlgorithmOptions : ICloneable<IAsymmetricAlgorithmOptions>
	{
		#region Methods

		ICertificate CreateCertificate(IOptionsMonitor<CertificateFactoryOptions> certificateFactoryOptionsMonitor, ICertificateOptions certificateOptions, ISystemClock systemClock);

		#endregion
	}
}