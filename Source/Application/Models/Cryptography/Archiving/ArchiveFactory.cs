using System.IO.Compression;
using System.Net.Mime;
using System.Text;
using Application.Models.Cryptography.Archiving.Extensions;
using Application.Models.Cryptography.Transferring;
using Application.Models.IO;
using Microsoft.Extensions.Internal;

namespace Application.Models.Cryptography.Archiving
{
	/// <inheritdoc />
	public class ArchiveFactory : IArchiveFactory
	{
		#region Fields

		private static readonly Encoding _encoding = Encoding.UTF8;

		#endregion

		#region Constructors

		public ArchiveFactory(ICertificateExporter certificateExporter, IFileNameResolver fileNameResolver, ISystemClock systemClock)
		{
			this.CertificateExporter = certificateExporter ?? throw new ArgumentNullException(nameof(certificateExporter));
			this.FileNameResolver = fileNameResolver ?? throw new ArgumentNullException(nameof(fileNameResolver));
			this.SystemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
		}

		#endregion

		#region Properties

		protected internal ICertificateExporter CertificateExporter { get; }
		protected internal virtual Encoding Encoding => _encoding;
		protected internal virtual IFileNameResolver FileNameResolver { get; }
		protected internal ISystemClock SystemClock { get; }

		#endregion

		#region Methods

		public virtual IArchive Create(ICertificate certificate, ArchiveKind kind, string password)
		{
			ArgumentNullException.ThrowIfNull(certificate);

			return new Archive
			{
				Bytes = this.GetBytes(kind, certificate, password),
				ContentType = MediaTypeNames.Application.Octet
			};
		}

		public virtual IArchive Create(IDictionary<string, ICertificate> certificates, IArchiveOptions options, string password)
		{
			ArgumentNullException.ThrowIfNull(certificates);
			ArgumentNullException.ThrowIfNull(options);

			return new Archive
			{
				Bytes = this.GetBytes(options, certificates, password),
				ContentType = MediaTypeNames.Application.Octet
			};
		}

		protected internal virtual IEnumerable<byte> GetBytes(ArchiveKind archiveKind, ICertificate certificate, string password)
		{
			ArgumentNullException.ThrowIfNull(certificate);

			var certificateTransfer = this.CertificateExporter.Export(certificate, password);

			using(var memoryStream = new MemoryStream())
			{
				using(var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Update, true))
				{
					foreach(var (name, bytes) in this.GetEntries(archiveKind, certificateTransfer, certificate.Subject))
					{
						var zipArchiveEntry = zipArchive.CreateEntry(name);

						using(var zipArchiveEntryStream = zipArchiveEntry.Open())
						{
							zipArchiveEntryStream.Write(bytes.ToArray());
						}
					}
				}

				return memoryStream.ToArray();
			}
		}

		protected internal virtual IEnumerable<byte> GetBytes(IArchiveOptions archiveOptions, IDictionary<string, ICertificate> certificates, string password)
		{
			ArgumentNullException.ThrowIfNull(archiveOptions);
			ArgumentNullException.ThrowIfNull(certificates);

			using(var memoryStream = new MemoryStream())
			{
				using(var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Update, true))
				{
					foreach(var (key, certificate) in certificates)
					{
						var certificateTransfer = this.CertificateExporter.Export(certificate, password);
						var directory = this.FileNameResolver.Resolve(key);

						foreach(var (name, bytes) in this.GetEntries(archiveOptions.Kind, certificateTransfer, key))
						{
							var entryName = archiveOptions.Flat ? name : $"{directory}/{name}";

							var zipArchiveEntry = zipArchive.CreateEntry(entryName);

							using(var zipArchiveEntryStream = zipArchiveEntry.Open())
							{
								zipArchiveEntryStream.Write(bytes.ToArray());
							}
						}
					}
				}

				return memoryStream.ToArray();
			}
		}

		protected internal virtual IEnumerable<byte> GetBytes(string value)
		{
			ArgumentNullException.ThrowIfNull(value);

			return this.Encoding.GetBytes(value);
		}

		protected internal virtual IDictionary<string, IEnumerable<byte>> GetEntries(ArchiveKind archiveKind, ICertificateTransfer certificateTransfer, string key)
		{
			ArgumentNullException.ThrowIfNull(certificateTransfer);

			var entries = new SortedDictionary<string, IEnumerable<byte>>(StringComparer.OrdinalIgnoreCase);

			const string oneLiner = "one-liner";
			key = this.FileNameResolver.Resolve(key);

			if(certificateTransfer.CertificatePem != null && archiveKind.CertificatePemIncluded())
			{
				var fileName = this.GetFileName(archiveKind, key, nameof(certificateTransfer.CertificatePem));

				entries.Add($"{fileName}.crt", this.GetBytes(certificateTransfer.CertificatePem));

				if(archiveKind == ArchiveKind.All)
					entries.Add($"{fileName}.{oneLiner}.crt", this.GetBytes(this.RemoveLineBreaks(certificateTransfer.CertificatePem)));
			}

			if(certificateTransfer.EncryptedPrivateKeyPem != null && archiveKind.EncryptedPrivateKeyPemIncluded())
			{
				var fileName = this.GetFileName(archiveKind, key, nameof(certificateTransfer.EncryptedPrivateKeyPem));

				entries.Add($"{fileName}.key", this.GetBytes(certificateTransfer.EncryptedPrivateKeyPem));

				if(archiveKind == ArchiveKind.All)
					entries.Add($"{fileName}.{oneLiner}.key", this.GetBytes(this.RemoveLineBreaks(certificateTransfer.EncryptedPrivateKeyPem)));
			}

			if(certificateTransfer.Pfx != null && archiveKind.PfxIncluded())
				entries.Add($"{key}.pfx", certificateTransfer.Pfx);

			if(certificateTransfer.Pkcs12 != null && archiveKind.P12Included())
				entries.Add($"{key}.p12", certificateTransfer.Pkcs12);

			if(certificateTransfer.PrivateKeyPem != null && archiveKind.PrivateKeyPemIncluded())
			{
				var fileName = this.GetFileName(archiveKind, key, nameof(certificateTransfer.PrivateKeyPem));

				entries.Add($"{fileName}.key", this.GetBytes(certificateTransfer.PrivateKeyPem));

				if(archiveKind == ArchiveKind.All)
					entries.Add($"{fileName}.{oneLiner}.key", this.GetBytes(this.RemoveLineBreaks(certificateTransfer.PrivateKeyPem)));
			}

			// ReSharper disable InvertIf
			if(certificateTransfer.PublicKeyPem != null && archiveKind.PublicKeyPemIncluded())
			{
				var fileName = this.GetFileName(archiveKind, key, nameof(certificateTransfer.PublicKeyPem));

				entries.Add($"{fileName}.key", this.GetBytes(certificateTransfer.PublicKeyPem));

				if(archiveKind == ArchiveKind.All)
					entries.Add($"{fileName}.{oneLiner}.key", this.GetBytes(this.RemoveLineBreaks(certificateTransfer.PublicKeyPem)));
			}
			// ReSharper restore InvertIf

			return entries;
		}

		protected internal virtual string GetFileName(ArchiveKind archiveKind, string key, string suffix)
		{
			return archiveKind == ArchiveKind.All ? $"{key}.{suffix}" : key;
		}

		protected internal virtual string RemoveLineBreaks(string value)
		{
			ArgumentNullException.ThrowIfNull(value);

			const string simpleNewLine = "\n";

			return value.Replace(Environment.NewLine, simpleNewLine, StringComparison.OrdinalIgnoreCase).Replace(simpleNewLine, string.Empty, StringComparison.OrdinalIgnoreCase);
		}

		#endregion
	}
}