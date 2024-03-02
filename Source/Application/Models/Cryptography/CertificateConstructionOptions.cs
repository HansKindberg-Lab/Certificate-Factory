using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Application.Models.Cryptography
{
	public class CertificateConstructionOptions : ICloneable<CertificateConstructionOptions>
	{
		#region Properties

		public virtual string AsymmetricAlgorithm { get; set; }
		public virtual CertificateAuthorityOptions CertificateAuthority { get; set; }
		public virtual EnhancedKeyUsage? EnhancedKeyUsage { get; set; }
		public virtual HashAlgorithm? HashAlgorithm { get; set; }
		public virtual X509KeyUsageFlags? KeyUsage { get; set; }
		public virtual DateTimeOffset? NotAfter { get; set; }
		public virtual DateTimeOffset? NotBefore { get; set; }
		public virtual byte? SerialNumberSize { get; set; }
		public virtual string Subject { get; set; }
		public virtual SubjectAlternativeNameOptions SubjectAlternativeName { get; set; }

		#endregion

		#region Methods

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		public virtual CertificateConstructionOptions Clone()
		{
			var memberwiseClone = (CertificateConstructionOptions)this.MemberwiseClone();

			var clone = new CertificateConstructionOptions
			{
				AsymmetricAlgorithm = this.AsymmetricAlgorithm == null ? null : new StringBuilder(this.AsymmetricAlgorithm).ToString(),
				CertificateAuthority = this.CertificateAuthority?.Clone(),
				EnhancedKeyUsage = memberwiseClone.EnhancedKeyUsage,
				HashAlgorithm = memberwiseClone.HashAlgorithm,
				KeyUsage = memberwiseClone.KeyUsage,
				NotAfter = memberwiseClone.NotAfter,
				NotBefore = memberwiseClone.NotBefore,
				SerialNumberSize = memberwiseClone.SerialNumberSize,
				Subject = this.Subject == null ? null : new StringBuilder(this.Subject).ToString(),
				SubjectAlternativeName = this.SubjectAlternativeName?.Clone()
			};

			return clone;
		}

		#endregion
	}
}