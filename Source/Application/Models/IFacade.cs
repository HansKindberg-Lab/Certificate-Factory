using Application.Models.Configuration;
using Application.Models.Cryptography;
using Application.Models.Cryptography.Archiving;
using Application.Models.Cryptography.Storing;
using Application.Models.Cryptography.Transferring;
using Application.Models.IO;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;

namespace Application.Models
{
	public interface IFacade
	{
		#region Properties

		IArchiveFactory ArchiveFactory { get; }
		ICertificateExporter CertificateExporter { get; }
		ICertificateFactory CertificateFactory { get; }
		IOptionsMonitor<CertificateFormOptions> CertificateFormOptionsMonitor { get; }
		ICertificateStore CertificateStore { get; }
		IFileNameResolver FileNameResolver { get; }
		ILoggerFactory LoggerFactory { get; }
		ISystemClock SystemClock { get; }

		#endregion
	}
}