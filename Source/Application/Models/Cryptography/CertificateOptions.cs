using System.Security.Cryptography.X509Certificates;

namespace Application.Models.Cryptography
{
	/// <inheritdoc />
	public class CertificateOptions : ICertificateOptions
	{
		#region Properties

		public virtual ICertificateAuthorityOptions CertificateAuthority { get; set; }
		public virtual EnhancedKeyUsage EnhancedKeyUsage { get; set; }
		public virtual HashAlgorithm HashAlgorithm { get; set; } = HashAlgorithm.Sha256;
		public virtual ICertificate Issuer { get; set; }
		public virtual X509KeyUsageFlags KeyUsage { get; set; }
		public virtual DateTimeOffset? NotAfter { get; set; }
		public virtual DateTimeOffset? NotBefore { get; set; }
		public virtual byte? SerialNumberSize { get; set; }
		public virtual string Subject { get; set; }
		public virtual ISubjectAlternativeNameOptions SubjectAlternativeName { get; set; }

		#endregion

		#region Methods

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		public virtual ICertificateOptions Clone()
		{
			var clone = (CertificateOptions)this.MemberwiseClone();

			clone.CertificateAuthority = this.CertificateAuthority?.Clone();
			clone.Issuer = this.Issuer;

			if(this.Subject != null)
				clone.Subject = new string(this.Subject);

			clone.SubjectAlternativeName = this.SubjectAlternativeName?.Clone();

			return clone;
		}

		#endregion
	}
}