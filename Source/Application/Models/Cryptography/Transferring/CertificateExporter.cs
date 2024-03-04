using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Application.Models.Cryptography.Extensions;

namespace Application.Models.Cryptography.Transferring
{
	/// <inheritdoc />
	public class CertificateExporter : ICertificateExporter
	{
		#region Fields

		private const string _certificatePemLabel = "CERTIFICATE";

		#endregion

		#region Constructors

		public CertificateExporter(IKeyExporterFactory keyExporterFactory, ILoggerFactory loggerFactory)
		{
			this.KeyExporterFactory = keyExporterFactory ?? throw new ArgumentNullException(nameof(keyExporterFactory));
			this.Logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger(this.GetType());
		}

		#endregion

		#region Properties

		protected internal virtual string CertificatePemLabel => _certificatePemLabel;
		protected internal virtual IKeyExporterFactory KeyExporterFactory { get; }
		protected internal virtual ILogger Logger { get; }

		#endregion

		#region Methods

		public virtual ICertificateTransfer Export(ICertificate certificate, string password)
		{
			ArgumentNullException.ThrowIfNull(certificate);

			var wrappedCertificate = certificate.Unwrap();

			var asymmetricAlgorithm = this.GetPrivateKeyAsymmetricAlgorithm(wrappedCertificate);

			var keyExporter = asymmetricAlgorithm != null ? this.KeyExporterFactory.Create(asymmetricAlgorithm) : null;

			var certificateExport = new CertificateTransfer
			{
				CertificatePem = this.GetCertificatePem(certificate),
				Pfx = this.GetPfx(wrappedCertificate, password),
				Pkcs12 = this.GetPkcs12(wrappedCertificate, password)
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

		public virtual string GetCertificatePem(ICertificate certificate)
		{
			ArgumentNullException.ThrowIfNull(certificate);

			return new string(PemEncoding.Write(this.CertificatePemLabel, (certificate.RawData ?? []).ToArray()));
		}

		protected internal virtual IEnumerable<byte> GetPfx(X509Certificate2 certificate, string password)
		{
			ArgumentNullException.ThrowIfNull(certificate);

			return certificate.Export(X509ContentType.Pfx, password);
		}

		protected internal virtual IEnumerable<byte> GetPkcs12(X509Certificate2 certificate, string password)
		{
			ArgumentNullException.ThrowIfNull(certificate);

			return certificate.Export(X509ContentType.Pkcs12, password);
		}

		[SuppressMessage("Usage", "CA2254:Template should be a static expression")]
		protected internal virtual AsymmetricAlgorithm GetPrivateKeyAsymmetricAlgorithm(X509Certificate2 certificate)
		{
			ArgumentNullException.ThrowIfNull(certificate);

			if(!certificate.HasPrivateKey)
			{
				if(this.Logger.IsEnabled(LogLevel.Warning))
					this.Logger.LogWarning($"The certificate \"{certificate}\" has no private-key.");

				return null;
			}

			var ecdsa = certificate.GetECDsaPrivateKey();

			if(ecdsa != null)
				return ecdsa;

			var rsa = certificate.GetRSAPrivateKey();

			if(rsa != null)
				return rsa;

			if(this.Logger.IsEnabled(LogLevel.Warning))
				this.Logger.LogWarning($"The private-key asymmetric algorithm for certificate \"{certificate}\" is not supported.");

			return null;
		}

		#endregion
	}
}