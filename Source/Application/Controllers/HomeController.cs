using System.Net.Mime;
using System.Text;
using Application.Models;
using Application.Models.Cryptography;
using Application.Models.Views.Home;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	public class HomeController(IFacade facade) : SiteController(facade)
	{
		#region Methods

		public virtual async Task<IActionResult> Index(string id)
		{
			IEnumerable<ICertificate> certificates = null;

			try
			{
				certificates = this.Facade.ApplicationCertificateStore.Certificates().ToArray();

				if(id != null)
				{
					var certificate = certificates.FirstOrDefault(certificate => string.Equals(certificate.Thumbprint, id, StringComparison.OrdinalIgnoreCase));

					if(certificate != null)
					{
						var certificatePem = certificate.GetCertificatePem();
						var certificatePemBytes = Encoding.UTF8.GetBytes(certificatePem);
						var file = this.File(certificatePemBytes, MediaTypeNames.Text.Plain, this.Facade.FileNameResolver.Resolve($"{certificate.Subject}.crt"));

						return file;
					}
				}

				var model = new HomeViewModel();

				foreach(var certificate in certificates)
				{
					model.Certificates.Add(new StoreCertificateViewModel
					{
						Issuer = certificate.Issuer,
						KeyAlgorithm = certificate.KeyAlgorithmName,
						NotAfter = certificate.NotAfter,
						NotBefore = certificate.NotBefore,
						SerialNumber = certificate.SerialNumber,
						Subject = certificate.Subject,
						Thumbprint = certificate.Thumbprint
					});
				}

				return await Task.FromResult(this.View(model));
			}
			finally
			{
				foreach(var certificate in certificates ?? [])
				{
					certificate.Dispose();
				}
			}
		}

		#endregion
	}
}