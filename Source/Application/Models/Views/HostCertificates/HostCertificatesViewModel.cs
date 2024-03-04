using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using Application.Models.Cryptography;
using Application.Models.Web;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Models.Views.HostCertificates
{
	public class HostCertificatesViewModel
	{
		#region Fields

		private IList<SelectListItem> _storeLocations;

		#endregion

		#region Properties

		public virtual SearchFormAction? Action { get; set; }
		public virtual IList<ICertificate> Certificates { get; } = [];
		public virtual string Error { get; set; }

		[Display(Name = "Friendly name")]
		public virtual string FriendlyName { get; set; }

		[Display(Name = "Issuer")]
		public virtual string Issuer { get; set; }

		[Display(Name = "Serial number")]
		public virtual string SerialNumber { get; set; }

		[Display(Name = "Store-location")]
		public virtual StoreLocation? StoreLocation { get; set; }

		public virtual IList<SelectListItem> StoreLocations => this._storeLocations ??= this.CreateStoreLocationSelection(this.StoreLocation);

		[Display(Name = "Store-name")]
		public virtual string StoreName { get; set; }

		[Display(Name = "Subject")]
		public virtual string Subject { get; set; }

		[Display(Name = "Thumbprint")]
		public virtual string Thumbprint { get; set; }

		#endregion

		#region Methods

		protected internal virtual IList<SelectListItem> CreateStoreLocationSelection(StoreLocation? storeLocation)
		{
			var selection = new List<SelectListItem> { new() };
			selection.AddRange(Enum.GetValues<StoreLocation>().Select(location => new SelectListItem(location.ToString(), location.ToString(), location == storeLocation)));

			selection.Sort((first, second) => string.Compare(first.Text, second.Text, StringComparison.OrdinalIgnoreCase));

			return selection;
		}

		#endregion
	}
}