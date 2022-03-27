using Application.Models.Views.Shared.Forms;

namespace Application.Models.Views.TlsCertificate
{
	public class TlsCertificateForm : IssuedCertificateForm
	{
		#region Properties

		public virtual string DnsNames { get; set; }

		#endregion
	}
}