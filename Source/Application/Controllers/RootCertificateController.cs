using Application.Models;
using Application.Models.Cryptography;
using Application.Models.Cryptography.Extensions;
using Application.Models.Views.RootCertificate;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	public class RootCertificateController(IFacade facade) : BasicCertificateController<RootCertificateForm>(facade)
	{
		#region Methods

		protected internal override ICertificate CreateCertificate(IAsymmetricAlgorithmOptions asymmetricAlgorithmOptions, RootCertificateForm form, ICertificate issuer)
		{
			ArgumentNullException.ThrowIfNull(form);

			return this.Facade.CertificateFactory.CreateRootCertificate(this.Facade.ApplicationCertificateStore, asymmetricAlgorithmOptions, form.Lifetime, this.Logger, form.Subject, this.Facade.SystemClock);
		}

		public virtual async Task<IActionResult> Index()
		{
			var model = new RootCertificateViewModel();

			this.PopulateFormLists(model.Form);

			return await Task.FromResult(this.View(model));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public virtual async Task<IActionResult> Index(RootCertificateForm form)
		{
			this.PopulateFormLists(form);

			var actionResult = this.CertificateArchiveFile("root-certificate", form) ?? this.View(new RootCertificateViewModel { Form = form });

			return await Task.FromResult(actionResult);
		}

		#endregion
	}
}