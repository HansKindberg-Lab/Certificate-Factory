using Application.Models;
using Application.Models.Cryptography.Archiving;
using Application.Models.Cryptography.Archiving.Extensions;
using Application.Models.Views.Shared.Forms;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Controllers
{
	public abstract class ArchiveKindController(IFacade facade) : SiteController(facade)
	{
		#region Methods

		protected internal virtual void PopulateArchiveKindDictionary(IArchiveKindForm form)
		{
			ArgumentNullException.ThrowIfNull(form);

			var selected = form.ArchiveKind ?? ArchiveKind.All;

			foreach(var archiveKind in Enum.GetValues<ArchiveKind>())
			{
				form.ArchiveKindDictionary.Add(new SelectListItem(archiveKind.Description(), archiveKind.ToString(), archiveKind == selected), archiveKind.Example());
			}
		}

		protected internal virtual void SortModelState()
		{
			var fieldOrder = this.Facade.CertificateFormOptionsMonitor.CurrentValue.FieldOrder;
			var index = 0;
			var keyOrderMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

			foreach(var key in this.ModelState.Keys)
			{
				keyOrderMap.Add(key, fieldOrder.TryGetValue(key, out var value) ? value : index);
				index++;
			}

			var modelState = new ModelStateDictionary();

			foreach(var key in keyOrderMap.OrderBy(item => item.Value).Select(item => item.Key))
			{
				// ReSharper disable PossibleNullReferenceException
				foreach(var modelError in this.ModelState[key].Errors)
				{
					// If we keep the key we have, it will sort it with model metadata. We need another key, the index of the sorted entries, to keep the order we want.
					modelState.AddModelError((modelState.Count + 1).ToString((IFormatProvider)null), modelError.ErrorMessage);
				}
				// ReSharper restore PossibleNullReferenceException
			}

			this.ModelState.Clear();
			this.ModelState.Merge(modelState);
		}

		#endregion
	}
}