using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Models.Views.Shared.Forms
{
	public interface IIssuedCertificateForm
	{
		#region Properties

		string Issuer { get; }
		IList<SelectListItem> IssuerList { get; }

		#endregion
	}
}