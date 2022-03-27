namespace Application.Models.Views.ClientCertificate
{
	public class ClientCertificateViewModel
	{
		#region Fields

		private ClientCertificateForm _form;

		#endregion

		#region Properties

		public virtual ClientCertificateForm Form
		{
			get => this._form ??= new ClientCertificateForm();
			set => this._form = value;
		}

		#endregion
	}
}