using Application.Models;
using Application.Models.Cryptography;
using Application.Models.Cryptography.Extensions;
using Application.Models.Views.TlsCertificate;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	public class TlsCertificateController(IFacade facade) : BasicCertificateController<TlsCertificateForm>(facade)
	{
		#region Methods

		protected internal override ICertificate CreateCertificate(IAsymmetricAlgorithmOptions asymmetricAlgorithmOptions, TlsCertificateForm form, ICertificate issuer)
		{
			ArgumentNullException.ThrowIfNull(form);

			return this.Facade.CertificateFactory.CreateTlsCertificate(this.Facade.ApplicationCertificateStore, asymmetricAlgorithmOptions, this.SplitOnLineBreaks(form.DnsNames), issuer, form.Lifetime, this.Logger, form.Subject, this.Facade.SystemClock);
		}

		public virtual async Task<IActionResult> Index()
		{
			var model = new TlsCertificateViewModel();

			this.PopulateFormLists(model.Form);

			return await Task.FromResult(this.View(model));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public virtual async Task<IActionResult> Index(TlsCertificateForm form)
		{
			this.PopulateFormLists(form);

			var actionResult = this.CertificateArchiveFile("tls-certificate", form) ?? this.View(new TlsCertificateViewModel { Form = form });

			return await Task.FromResult(actionResult);
		}

		#endregion
	}
}