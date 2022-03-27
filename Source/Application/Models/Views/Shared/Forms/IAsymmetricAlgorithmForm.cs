using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Models.Views.Shared.Forms
{
	public interface IAsymmetricAlgorithmForm
	{
		#region Properties

		string AsymmetricAlgorithm { get; }
		IList<SelectListItem> AsymmetricAlgorithmList { get; }

		#endregion
	}
}