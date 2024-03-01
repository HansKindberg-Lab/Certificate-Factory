using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Models.Views.Shared.Forms
{
	public abstract class IssuedCertificateForm : LifetimeCertificateForm, IIssuedCertificateForm
	{
		#region Properties

		public virtual string Issuer { get; set; }
		public virtual IList<SelectListItem> IssuerList { get; } = [];

		#endregion
	}
}