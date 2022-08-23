using System.ComponentModel.DataAnnotations;
using Application.Models.Views.Shared.Forms;

namespace Application.Models.Views.JsonConfiguredCertificates
{
	public class JsonConfiguredCertificatesForm : IPasswordCertificateForm
	{
		#region Properties

		[Required(ErrorMessage = "\"File\" is required.")]
		public virtual IFormFile File { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "\"Password\" is required.")]
		public virtual string Password { get; set; }

		#endregion
	}
}