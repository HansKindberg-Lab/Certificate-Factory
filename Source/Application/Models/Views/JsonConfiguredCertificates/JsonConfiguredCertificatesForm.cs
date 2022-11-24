using System.ComponentModel.DataAnnotations;
using Application.Models.Cryptography.Archiving;
using Application.Models.Views.Shared.Forms;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Models.Views.JsonConfiguredCertificates
{
	public class JsonConfiguredCertificatesForm : IArchiveKindForm, IPasswordCertificateForm
	{
		#region Properties

		public virtual ArchiveKind? ArchiveKind { get; set; }
		public virtual IDictionary<SelectListItem, string> ArchiveKindDictionary { get; } = new Dictionary<SelectListItem, string>();

		[Required(ErrorMessage = "\"File\" is required.")]
		public virtual IFormFile File { get; set; }

		public virtual bool FlatArchive { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "\"Password\" is required.")]
		public virtual string Password { get; set; }

		#endregion
	}
}