using Application.Models;
using Application.Models.Cryptography;
using Application.Models.Cryptography.Extensions;
using Application.Models.Views.TlsCertificate;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	public class TlsCertificateController : BasicCertificateController<TlsCertificateForm>
	{
		#region Constructors

		public TlsCertificateController(IFacade facade) : base(facade) { }

		#endregion

		#region Methods

		protected internal override ICertificate CreateCertificate(IAsymmetricAlgorithmOptions asymmetricAlgorithmOptions, TlsCertificateForm form, ICertificate issuer)
		{
			if(form == null)
				throw new ArgumentNullException(nameof(form));

			return this.Facade.CertificateFactory.CreateTlsCertificate(asymmetricAlgorithmOptions, this.Facade.CertificateStore, this.SplitOnLineBreaks(form.DnsNames), issuer, form.Lifetime, this.Logger, form.Subject, this.Facade.SystemClock);
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