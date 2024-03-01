using Application.Models;
using Application.Models.Cryptography;
using Application.Models.Cryptography.Extensions;
using Application.Models.Views.IntermediateCertificate;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	public class IntermediateCertificateController : BasicCertificateController<IntermediateCertificateForm>
	{
		#region Constructors

		public IntermediateCertificateController(IFacade facade) : base(facade) { }

		#endregion

		#region Methods

		protected internal override ICertificate CreateCertificate(IAsymmetricAlgorithmOptions asymmetricAlgorithmOptions, IntermediateCertificateForm form, ICertificate issuer)
		{
			ArgumentNullException.ThrowIfNull(form);

			return this.Facade.CertificateFactory.CreateIntermediateCertificate(asymmetricAlgorithmOptions, this.Facade.CertificateStore, issuer, form.Lifetime, this.Logger, form.Subject, this.Facade.SystemClock);
		}

		public virtual async Task<IActionResult> Index()
		{
			var model = new IntermediateCertificateViewModel();

			this.PopulateFormLists(model.Form);

			return await Task.FromResult(this.View(model));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public virtual async Task<IActionResult> Index(IntermediateCertificateForm form)
		{
			this.PopulateFormLists(form);

			var actionResult = this.CertificateArchiveFile("intermediate-certificate", form) ?? this.View(new IntermediateCertificateViewModel { Form = form });

			return await Task.FromResult(actionResult);
		}

		#endregion
	}
}