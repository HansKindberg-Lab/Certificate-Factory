using System.Security.Cryptography.X509Certificates;
using Application.Models;
using Application.Models.Views.HostCertificate;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	public class HostCertificateController(IFacade facade) : SiteController(facade)
	{
		#region Methods

		public virtual async Task<IActionResult> Index(StoreLocation? storeLocation, string storeName, string thumbprint, string returnUrl = null)
		{
			var model = new HostCertificateViewModel();

			if(storeLocation == null)
				model.ErrorDetails.Add("Store-location required.");

			if(string.IsNullOrWhiteSpace(storeName))
				model.ErrorDetails.Add("Store-name required.");

			if(string.IsNullOrWhiteSpace(thumbprint))
				model.ErrorDetails.Add("Thumbprint required.");

			if(model.ErrorDetails.Any())
			{
				model.Error = "Invalid query.";
			}
			else
			{
				try
				{
					// ReSharper disable PossibleInvalidOperationException
					model.Certificate = this.Facade.CertificateLoader.Get(storeLocation.Value, storeName, thumbprint);
					// ReSharper restore PossibleInvalidOperationException
				}
				catch(Exception exception)
				{
					this.Logger.LogError(exception, exception.ToString());
					model.Error = exception.ToString();
				}
			}

			model.ValidatedReturnUrl = this.Url.IsLocalUrl(returnUrl) ? returnUrl : null;

			return await Task.FromResult(this.View(model));
		}

		#endregion
	}
}