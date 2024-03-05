using System.Security.Cryptography.X509Certificates;
using Application.Models;
using Application.Models.Views.HostCertificates;
using Application.Models.Web;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	public class HostCertificatesController(IFacade facade) : SiteController(facade)
	{
		#region Methods

		public virtual async Task<IActionResult> Index(SearchFormAction? formAction = null, string friendlyName = null, string issuer = null, string serialNumber = null, StoreLocation? storeLocation = null, string storeName = null, string subject = null, string thumbprint = null)
		{
			if(formAction is SearchFormAction.Reset)
				return this.Redirect(this.Request.Path);

			var model = new HostCertificatesViewModel
			{
				Action = formAction,
				FriendlyName = friendlyName,
				Issuer = issuer,
				SerialNumber = serialNumber,
				StoreLocation = storeLocation,
				StoreName = storeName,
				Subject = subject,
				Thumbprint = thumbprint
			};

			if(string.IsNullOrEmpty(friendlyName) && string.IsNullOrEmpty(issuer) && string.IsNullOrEmpty(serialNumber) && storeLocation == null && string.IsNullOrEmpty(storeName) && string.IsNullOrEmpty(subject) && string.IsNullOrEmpty(thumbprint))
				model.Action = null;

			if(model.Action is SearchFormAction.Search)
			{
				try
				{
					foreach(var certificate in this.Facade.CertificateLoader.Find(friendlyName, issuer, serialNumber, storeLocation, storeName, subject, thumbprint))
					{
						var identifierPrefix = certificate.Store != null ? $"{certificate.Store.Location}-{certificate.Store.Name}-" : "?-?-";
						var key = certificate.Store != null ? $"{certificate.Store.Location} / {certificate.Store.Name}" : "?";

						if(!model.Certificates.TryGetValue(key, out var certificates))
						{
							certificates = [];

							model.Certificates.Add(key, certificates);
						}

						certificates.Add(new CertificateInformation
						{
							Identifier = $"{identifierPrefix}{certificate.Thumbprint}".ToLowerInvariant(),
							StoreLocation = certificate.Store?.Location.ToString(),
							StoreName = certificate.Store?.Name,
							Subject = certificate.Subject,
							Thumbprint = certificate.Thumbprint
						});

						certificate.Dispose();
					}
				}
				catch(Exception exception)
				{
					this.Logger.LogError(exception, exception.ToString());
					model.Error = exception.ToString();
				}
			}

			return await Task.FromResult(this.View(model));
		}

		#endregion
	}
}