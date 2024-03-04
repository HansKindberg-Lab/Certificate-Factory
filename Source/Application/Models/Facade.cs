using Application.Models.Configuration;
using Application.Models.Cryptography;
using Application.Models.Cryptography.Archiving;
using Application.Models.Cryptography.Storing;
using Application.Models.Cryptography.Transferring;
using Application.Models.IO;
using Microsoft.Extensions.Options;

namespace Application.Models
{
	public class Facade(IApplicationCertificateStore applicationCertificateStore, IArchiveFactory archiveFactory, IAsymmetricAlgorithmRepository asymmetricAlgorithmRepository, ICertificateConstructionHelper certificateConstructionHelper, ICertificateExporter certificateExporter, ICertificateFactory certificateFactory, IOptionsMonitor<CertificateFormOptions> certificateFormOptionsMonitor, ICertificateLoader certificateLoader, IFileNameResolver fileNameResolver, ILoggerFactory loggerFactory, ISystemClock systemClock) : IFacade
	{
		#region Properties

		public virtual IApplicationCertificateStore ApplicationCertificateStore { get; } = applicationCertificateStore ?? throw new ArgumentNullException(nameof(applicationCertificateStore));
		public virtual IArchiveFactory ArchiveFactory { get; } = archiveFactory ?? throw new ArgumentNullException(nameof(archiveFactory));
		public virtual IAsymmetricAlgorithmRepository AsymmetricAlgorithmRepository { get; } = asymmetricAlgorithmRepository ?? throw new ArgumentNullException(nameof(asymmetricAlgorithmRepository));
		public virtual ICertificateConstructionHelper CertificateConstructionHelper { get; } = certificateConstructionHelper ?? throw new ArgumentNullException(nameof(certificateConstructionHelper));
		public virtual ICertificateExporter CertificateExporter { get; } = certificateExporter ?? throw new ArgumentNullException(nameof(certificateExporter));
		public virtual ICertificateFactory CertificateFactory { get; } = certificateFactory ?? throw new ArgumentNullException(nameof(certificateFactory));
		public virtual IOptionsMonitor<CertificateFormOptions> CertificateFormOptionsMonitor { get; } = certificateFormOptionsMonitor ?? throw new ArgumentNullException(nameof(certificateFormOptionsMonitor));
		public virtual ICertificateLoader CertificateLoader { get; } = certificateLoader ?? throw new ArgumentNullException(nameof(certificateLoader));
		public virtual IFileNameResolver FileNameResolver { get; } = fileNameResolver ?? throw new ArgumentNullException(nameof(fileNameResolver));
		public virtual ILoggerFactory LoggerFactory { get; } = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
		public virtual ISystemClock SystemClock { get; } = systemClock ?? throw new ArgumentNullException(nameof(systemClock));

		#endregion
	}
}