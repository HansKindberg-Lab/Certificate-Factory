using System.ComponentModel.DataAnnotations;
using Application.Models.Cryptography;
using Application.Models.Views.Shared.Forms;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Models.Views.Certificate
{
	public class CertificateForm : BasicCertificateForm, IIssuedCertificateForm
	{
		#region Properties

		public virtual bool CertificateAuthorityEnabled { get; set; }
		public virtual string DnsNames { get; set; }
		public virtual string EmailAddresses { get; set; }

		/// <summary>
		/// To handle multiple check-boxes.
		/// </summary>
		public virtual IList<string> EnhancedKeyUsage { get; } = new List<string>();

		public virtual IList<SelectListItem> EnhancedKeyUsageList { get; } = new List<SelectListItem>();
		public virtual HashAlgorithm HashAlgorithm { get; set; } = HashAlgorithm.Sha256;
		public virtual IList<SelectListItem> HashAlgorithmList { get; } = new List<SelectListItem>();
		public virtual string IpAddresses { get; set; }
		public virtual string Issuer { get; set; }
		public virtual IList<SelectListItem> IssuerList { get; } = new List<SelectListItem>();

		/// <summary>
		/// To handle multiple check-boxes.
		/// </summary>
		public virtual IList<string> KeyUsage { get; } = new List<string>();

		public virtual IList<SelectListItem> KeyUsageList { get; } = new List<SelectListItem>();
		public virtual DateTimeOffset? NotAfter { get; set; }
		public virtual DateTimeOffset? NotBefore { get; set; }

		/// <summary>
		/// Has to do with certificate authorities (CA).
		/// </summary>
		[Range(0, 1000, ErrorMessage = "\"Path-length constraint\" must be between 0 and 1000.")]
		public virtual int? PathLengthConstraint { get; set; }

		public virtual string Uris { get; set; }
		public virtual string UserPrincipalNames { get; set; }

		#endregion
	}
}