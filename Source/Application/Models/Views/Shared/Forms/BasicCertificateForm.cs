using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Models.Views.Shared.Forms
{
	public abstract class BasicCertificateForm : IAsymmetricAlgorithmForm, IPasswordCertificateForm, ISubjectCertificateForm
	{
		#region Properties

		[Required(ErrorMessage = "\"Asymmetric algorithm\" is required.")]
		public virtual string AsymmetricAlgorithm { get; set; }

		public virtual IList<SelectListItem> AsymmetricAlgorithmList { get; } = new List<SelectListItem>();

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "\"Password\" is required.")]
		public virtual string Password { get; set; }

		[Required(ErrorMessage = "\"Subject\" is required.")]
		public virtual string Subject { get; set; }

		#endregion
	}
}