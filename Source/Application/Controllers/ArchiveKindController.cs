using System.ComponentModel;
using System.Reflection;
using Application.Models;
using Application.Models.ComponentModel;
using Application.Models.Cryptography.Archiving;
using Application.Models.Views.Shared.Forms;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Controllers
{
	public abstract class ArchiveKindController(IFacade facade) : SiteController(facade)
	{
		#region Fields

		private static IDictionary<ArchiveKind, string> _archiveKindDescriptions;
		private static readonly object _archiveKindDescriptionsLock = new();
		private static IDictionary<ArchiveKind, string> _archiveKindExamples;
		private static readonly object _archiveKindExamplesLock = new();

		#endregion

		#region Properties

		protected internal virtual IDictionary<ArchiveKind, string> ArchiveKindDescriptions
		{
			get
			{
				// ReSharper disable All
				if(_archiveKindDescriptions == null)
				{
					lock(_archiveKindDescriptionsLock)
					{
						if(_archiveKindDescriptions == null)
						{
							var archiveKindDescriptions = new Dictionary<ArchiveKind, string>();
							var archiveKindType = typeof(ArchiveKind);

							foreach(var archiveKind in Enum.GetValues<ArchiveKind>())
							{
								var field = archiveKindType.GetField(archiveKind.ToString());
								var descriptionAttribute = (DescriptionAttribute)field.GetCustomAttribute(typeof(DescriptionAttribute), false);
								archiveKindDescriptions.Add(archiveKind, descriptionAttribute.Description);
							}

							_archiveKindDescriptions = archiveKindDescriptions;
						}
					}
				}
				// ReSharper restore All

				return _archiveKindDescriptions;
			}
		}

		protected internal virtual IDictionary<ArchiveKind, string> ArchiveKindExamples
		{
			get
			{
				// ReSharper disable All
				if(_archiveKindExamples == null)
				{
					lock(_archiveKindExamplesLock)
					{
						if(_archiveKindExamples == null)
						{
							var archiveKindExamples = new Dictionary<ArchiveKind, string>();
							var archiveKindType = typeof(ArchiveKind);

							foreach(var archiveKind in Enum.GetValues<ArchiveKind>())
							{
								var field = archiveKindType.GetField(archiveKind.ToString());
								var ExampleAttribute = (ExampleAttribute)field.GetCustomAttribute(typeof(ExampleAttribute), false);
								archiveKindExamples.Add(archiveKind, ExampleAttribute.Example);
							}

							_archiveKindExamples = archiveKindExamples;
						}
					}
				}
				// ReSharper restore All

				return _archiveKindExamples;
			}
		}

		#endregion

		#region Methods

		protected internal virtual void PopulateArchiveKindDictionary(IArchiveKindForm form)
		{
			ArgumentNullException.ThrowIfNull(form);

			var selected = form.ArchiveKind ?? ArchiveKind.All;

			foreach(var (archiveKind, description) in this.ArchiveKindDescriptions)
			{
				form.ArchiveKindDictionary.Add(new SelectListItem(description, archiveKind.ToString(), archiveKind == selected), this.ArchiveKindExamples[archiveKind]);
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