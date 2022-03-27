using System.IO.Compression;
using System.Net.Mime;
using System.Text;
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

		public virtual IArchive Create(ICertificate certificate, string password)
		{
			if(certificate == null)
				throw new ArgumentNullException(nameof(certificate));

			return new Archive
			{
				Bytes = this.GetBytes(certificate, password),
				ContentType = MediaTypeNames.Application.Octet
			};
		}

		protected internal virtual IEnumerable<byte> GetBytes(ICertificate certificate, string password)
		{
			if(certificate == null)
				throw new ArgumentNullException(nameof(certificate));

			var certificateTransfer = this.CertificateExporter.Export(certificate, password);

			using(var memoryStream = new MemoryStream())
			{
				using(var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Update, true))
				{
					foreach(var (name, bytes) in this.GetEntries(certificateTransfer, certificate.Subject))
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

		protected internal virtual IEnumerable<byte> GetBytes(string value)
		{
			if(value == null)
				throw new ArgumentNullException(nameof(value));

			return this.Encoding.GetBytes(value);
		}

		protected internal virtual IDictionary<string, IEnumerable<byte>> GetEntries(ICertificateTransfer certificateTransfer, string subject)
		{
			if(certificateTransfer == null)
				throw new ArgumentNullException(nameof(certificateTransfer));

			var entries = new SortedDictionary<string, IEnumerable<byte>>(StringComparer.OrdinalIgnoreCase);

			const string oneLiner = "one-liner";
			subject = this.FileNameResolver.Resolve(subject);

			if(certificateTransfer.CertificatePem != null)
			{
				var fileName = $"{subject}.{nameof(certificateTransfer.CertificatePem)}";
				entries.Add($"{fileName}.crt", this.GetBytes(certificateTransfer.CertificatePem));
				entries.Add($"{fileName}.{oneLiner}.crt", this.GetBytes(this.RemoveLineBreaks(certificateTransfer.CertificatePem)));
			}

			if(certificateTransfer.EncryptedPrivateKeyPem != null)
			{
				var fileName = $"{subject}.{nameof(certificateTransfer.EncryptedPrivateKeyPem)}";
				entries.Add($"{fileName}.key", this.GetBytes(certificateTransfer.EncryptedPrivateKeyPem));
				entries.Add($"{fileName}.{oneLiner}.key", this.GetBytes(this.RemoveLineBreaks(certificateTransfer.EncryptedPrivateKeyPem)));
			}

			if(certificateTransfer.Pfx != null)
				entries.Add($"{subject}.pfx", certificateTransfer.Pfx);

			if(certificateTransfer.Pkcs12 != null)
				entries.Add($"{subject}.p12", certificateTransfer.Pkcs12);

			if(certificateTransfer.PrivateKeyPem != null)
			{
				var fileName = $"{subject}.{nameof(certificateTransfer.PrivateKeyPem)}";
				entries.Add($"{fileName}.key", this.GetBytes(certificateTransfer.PrivateKeyPem));
				entries.Add($"{fileName}.{oneLiner}.key", this.GetBytes(this.RemoveLineBreaks(certificateTransfer.PrivateKeyPem)));
			}

			// ReSharper disable InvertIf
			if(certificateTransfer.PublicKeyPem != null)
			{
				var fileName = $"{subject}.{nameof(certificateTransfer.PublicKeyPem)}";
				entries.Add($"{fileName}.key", this.GetBytes(certificateTransfer.PublicKeyPem));
				entries.Add($"{fileName}.{oneLiner}.key", this.GetBytes(this.RemoveLineBreaks(certificateTransfer.PublicKeyPem)));
			}
			// ReSharper restore InvertIf

			return entries;
		}

		protected internal virtual string RemoveLineBreaks(string value)
		{
			if(value == null)
				throw new ArgumentNullException(nameof(value));

			const string simpleNewLine = "\n";

			return value.Replace(Environment.NewLine, simpleNewLine, StringComparison.OrdinalIgnoreCase).Replace(simpleNewLine, string.Empty, StringComparison.OrdinalIgnoreCase);
		}

		#endregion
	}
}