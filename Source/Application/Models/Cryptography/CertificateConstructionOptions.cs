using System.Security.Cryptography.X509Certificates;
using Application.Models.Collections.Generic.Extensions;

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
			var clone = (CertificateConstructionOptions)this.MemberwiseClone();

			if(this.AsymmetricAlgorithm != null)
				clone.AsymmetricAlgorithm = new string(this.AsymmetricAlgorithm);

			if(this.CertificateAuthority != null)
				clone.CertificateAuthority = new CertificateAuthorityOptions { PathLengthConstraint = this.CertificateAuthority.Clone().PathLengthConstraint };

			if(this.Subject != null)
				clone.Subject = new string(this.Subject);

			if(this.SubjectAlternativeName != null)
			{
				clone.SubjectAlternativeName = new SubjectAlternativeNameOptions();
				var subjectAlternativeNameClone = this.SubjectAlternativeName.Clone();
				clone.SubjectAlternativeName.DnsNames.Add(subjectAlternativeNameClone.DnsNames);
				clone.SubjectAlternativeName.EmailAddresses.Add(subjectAlternativeNameClone.EmailAddresses);
				clone.SubjectAlternativeName.IpAddresses.Add(subjectAlternativeNameClone.IpAddresses);
				clone.SubjectAlternativeName.Uris.Add(subjectAlternativeNameClone.Uris);
				clone.SubjectAlternativeName.UserPrincipalNames.Add(subjectAlternativeNameClone.UserPrincipalNames);
			}

			return clone;
		}

		#endregion
	}
}