using Application.Models;
using Application.Models.Cryptography;
using Application.Models.Cryptography.Extensions;
using Application.Models.Views.ClientCertificate;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	public class ClientCertificateController : BasicCertificateController<ClientCertificateForm>
	{
		#region Constructors

		public ClientCertificateController(IFacade facade) : base(facade) { }

		#endregion

		#region Methods

		protected internal override ICertificate CreateCertificate(IAsymmetricAlgorithmOptions asymmetricAlgorithmOptions, ClientCertificateForm form, ICertificate issuer)
		{
			if(form == null)
				throw new ArgumentNullException(nameof(form));

			return this.Facade.CertificateFactory.CreateClientCertificate(asymmetricAlgorithmOptions, this.Facade.CertificateStore, issuer, form.Lifetime, this.Logger, form.Subject, this.Facade.SystemClock);
		}

		public virtual async Task<IActionResult> Index()
		{
			var model = new ClientCertificateViewModel();

			this.PopulateFormLists(model.Form);

			return await Task.FromResult(this.View(model));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public virtual async Task<IActionResult> Index(ClientCertificateForm form)
		{
			this.PopulateFormLists(form);

			var actionResult = this.CertificateArchiveFile("client-certificate", form) ?? this.View(new ClientCertificateViewModel { Form = form });

			return await Task.FromResult(actionResult);
		}

		#endregion
	}
}