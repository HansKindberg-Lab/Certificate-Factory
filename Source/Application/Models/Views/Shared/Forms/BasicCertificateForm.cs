using System.ComponentModel.DataAnnotations;
using Application.Models.Cryptography.Archiving;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Models.Views.Shared.Forms
{
	public abstract class BasicCertificateForm : IArchiveKindForm, IAsymmetricAlgorithmForm, IPasswordCertificateForm, ISubjectCertificateForm
	{
		#region Properties

		[Required(ErrorMessage = "\"Archive kind\" is required.")]
		public virtual ArchiveKind? ArchiveKind { get; set; }

		public virtual IDictionary<SelectListItem, string> ArchiveKindDictionary { get; } = new Dictionary<SelectListItem, string>();
		public virtual IList<SelectListItem> ArchiveKindList { get; } = new List<SelectListItem>();

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