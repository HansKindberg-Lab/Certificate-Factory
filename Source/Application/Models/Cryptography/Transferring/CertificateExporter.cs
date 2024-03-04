namespace Application.Models.Cryptography.Transferring
{
	/// <inheritdoc />
	public class CertificateExporter : ICertificateExporter
	{
		#region Constructors

		public CertificateExporter(IKeyExporterFactory keyExporterFactory, ILoggerFactory loggerFactory)
		{
			this.KeyExporterFactory = keyExporterFactory ?? throw new ArgumentNullException(nameof(keyExporterFactory));
			this.Logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger(this.GetType());
		}

		#endregion

		#region Properties

		protected internal virtual IKeyExporterFactory KeyExporterFactory { get; }
		protected internal virtual ILogger Logger { get; }

		#endregion

		#region Methods

		public virtual ICertificateTransfer Export(ICertificate certificate, string password)
		{
			ArgumentNullException.ThrowIfNull(certificate);

			using(var asymmetricAlgorithm = certificate.GetPrivateKeyAsymmetricAlgorithm())
			{
				var keyExporter = asymmetricAlgorithm != null ? this.KeyExporterFactory.Create(asymmetricAlgorithm) : null;

				var certificateExport = new CertificateTransfer
				{
					CertificatePem = certificate.GetCertificatePem(),
					Pfx = certificate.GetPfx(password),
					Pkcs12 = certificate.GetPkcs12(password)
				};

				// ReSharper disable InvertIf
				if(keyExporter != null)
				{
					certificateExport.EncryptedPrivateKeyPem = keyExporter.GetEncryptedPrivateKeyPem(password);
					certificateExport.PrivateKeyPem = keyExporter.GetPrivateKeyPem();
					certificateExport.PublicKeyPem = keyExporter.GetPublicKeyPem();
				}
				// ReSharper restore InvertIf

				return certificateExport;
			}
		}

		#endregion
	}
}