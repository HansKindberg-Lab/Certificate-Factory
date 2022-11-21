using Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Application.Controllers
{
	public abstract class SiteController : Controller
	{
		#region Constructors

		protected SiteController(IFacade facade)
		{
			this.Facade = facade ?? throw new ArgumentNullException(nameof(facade));
			this.Logger = (facade.LoggerFactory ?? throw new ArgumentException("The logger-factory property can not be null.", nameof(facade))).CreateLogger(this.GetType());
		}

		#endregion

		#region Properties

		protected internal virtual IFacade Facade { get; }
		protected internal virtual ILogger Logger { get; }

		#endregion

		#region Methods

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