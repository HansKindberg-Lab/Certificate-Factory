using System.Security.Cryptography.X509Certificates;
using Application.Models;
using Application.Models.Cryptography;
using Application.Models.Views.HostCertificate;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	public class HostCertificateController(IFacade facade) : SiteController(facade)
	{
		#region Methods

		protected internal virtual CertificateInformation CreateCertificateInformation(ICertificate certificate)
		{
			return new CertificateInformation
			{
				Archived = certificate.Archived,
				FriendlyName = certificate.FriendlyName,
				HasPrivateKey = certificate.HasPrivateKey,
				Issuer = certificate.Issuer,
				KeyAlgorithm = certificate.KeyAlgorithm,
				KeyAlgorithmName = certificate.KeyAlgorithmName,
				NotAfter = certificate.NotAfter,
				NotBefore = certificate.NotBefore,
				SerialNumber = certificate.SerialNumber,
				SignatureAlgorithm = certificate.SignatureAlgorithm?.FriendlyName,
				Store = certificate.Store?.ToString(),
				StringRepresentation = certificate.ToString(),
				Subject = certificate.Subject,
				Thumbprint = certificate.Thumbprint,
				Version = certificate.Version
			};
		}

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
				ICertificate certificate = null;

				try
				{
					// ReSharper disable PossibleInvalidOperationException
					certificate = this.Facade.CertificateLoader.Get(storeLocation.Value, storeName, thumbprint);
					// ReSharper restore PossibleInvalidOperationException

					model.Certificate = this.CreateCertificateInformation(certificate);
				}
				catch(Exception exception)
				{
					this.Logger.LogError(exception, exception.ToString());
					model.Error = exception.ToString();
				}
				finally
				{
					certificate?.Dispose();
				}
			}

			model.ValidatedReturnUrl = this.Url.IsLocalUrl(returnUrl) ? returnUrl : null;

			return await Task.FromResult(this.View(model));
		}

		#endregion
	}
}