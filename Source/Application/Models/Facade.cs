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
	public class Facade : IFacade
	{
		#region Constructors

		public Facade(IArchiveFactory archiveFactory, IAsymmetricAlgorithmRepository asymmetricAlgorithmRepository, ICertificateConstructionHelper certificateConstructionHelper, ICertificateExporter certificateExporter, ICertificateFactory certificateFactory, IOptionsMonitor<CertificateFormOptions> certificateFormOptionsMonitor, ICertificateStore certificateStore, IFileNameResolver fileNameResolver, ILoggerFactory loggerFactory, ISystemClock systemClock)
		{
			this.ArchiveFactory = archiveFactory ?? throw new ArgumentNullException(nameof(archiveFactory));
			this.AsymmetricAlgorithmRepository = asymmetricAlgorithmRepository ?? throw new ArgumentNullException(nameof(asymmetricAlgorithmRepository));
			this.CertificateConstructionHelper = certificateConstructionHelper ?? throw new ArgumentNullException(nameof(certificateConstructionHelper));
			this.CertificateExporter = certificateExporter ?? throw new ArgumentNullException(nameof(certificateExporter));
			this.CertificateFactory = certificateFactory ?? throw new ArgumentNullException(nameof(certificateFactory));
			this.CertificateFormOptionsMonitor = certificateFormOptionsMonitor ?? throw new ArgumentNullException(nameof(certificateFormOptionsMonitor));
			this.CertificateStore = certificateStore ?? throw new ArgumentNullException(nameof(certificateStore));
			this.FileNameResolver = fileNameResolver ?? throw new ArgumentNullException(nameof(fileNameResolver));
			this.LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
			this.SystemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
		}

		#endregion

		#region Properties

		public virtual IArchiveFactory ArchiveFactory { get; }
		public virtual IAsymmetricAlgorithmRepository AsymmetricAlgorithmRepository { get; }
		public virtual ICertificateConstructionHelper CertificateConstructionHelper { get; }
		public virtual ICertificateExporter CertificateExporter { get; }
		public virtual ICertificateFactory CertificateFactory { get; }
		public virtual IOptionsMonitor<CertificateFormOptions> CertificateFormOptionsMonitor { get; }
		public virtual ICertificateStore CertificateStore { get; }
		public virtual IFileNameResolver FileNameResolver { get; }
		public virtual ILoggerFactory LoggerFactory { get; }
		public virtual ISystemClock SystemClock { get; }

		#endregion
	}
}