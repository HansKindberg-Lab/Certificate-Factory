using Application.Models.Cryptography.Archiving;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Models.Views.Shared.Forms
{
	public interface IArchiveKindForm
	{
		#region Properties

		ArchiveKind? ArchiveKind { get; }
		IDictionary<SelectListItem, string> ArchiveKindDictionary { get; }

		#endregion
	}
}