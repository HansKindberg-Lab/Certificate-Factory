using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Application.Models.Cryptography
{
	/// <inheritdoc cref="ICertificateOptions" />
	public class CertificateOptions : ICertificateOptions, ICloneable<CertificateOptions>
	{
		#region Properties

		public virtual ICertificateAuthorityOptions CertificateAuthority { get; set; }
		public virtual ICrlDistributionPointOptions CrlDistributionPoint { get; set; }
		public virtual EnhancedKeyUsage EnhancedKeyUsage { get; set; }
		public virtual HashAlgorithm HashAlgorithm { get; set; } = HashAlgorithm.Sha256;
		public virtual ICertificate Issuer { get; set; }
		public virtual X509KeyUsageFlags KeyUsage { get; set; }
		public virtual DateTimeOffset? NotAfter { get; set; }
		public virtual DateTimeOffset? NotBefore { get; set; }
		public virtual ISerialNumberOptions SerialNumber { get; set; }
		public virtual string Subject { get; set; }
		public virtual ISubjectAlternativeNameOptions SubjectAlternativeName { get; set; }

		#endregion

		#region Methods

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		ICertificateOptions ICloneable<ICertificateOptions>.Clone()
		{
			return this.Clone();
		}

		public virtual CertificateOptions Clone()
		{
			var memberwiseClone = (CertificateOptions)this.MemberwiseClone();

			return new CertificateOptions
			{
				CertificateAuthority = this.CertificateAuthority?.Clone(),
				CrlDistributionPoint = this.CrlDistributionPoint?.Clone(),
				EnhancedKeyUsage = memberwiseClone.EnhancedKeyUsage,
				HashAlgorithm = memberwiseClone.HashAlgorithm,
				Issuer = memberwiseClone.Issuer, // At the moment we do not clone the issuer. Don't know if we have to. Don't think so.
				KeyUsage = memberwiseClone.KeyUsage,
				NotAfter = memberwiseClone.NotAfter,
				NotBefore = memberwiseClone.NotBefore,
				SerialNumber = this.SerialNumber?.Clone(),
				Subject = this.Subject == null ? null : new StringBuilder(this.Subject).ToString(),
				SubjectAlternativeName = this.SubjectAlternativeName?.Clone()
			};
		}

		#endregion
	}
}