namespace Application.Models.Views.RootCertificate
{
	public class RootCertificateViewModel
	{
		#region Fields

		private RootCertificateForm _form;

		#endregion

		#region Properties

		public virtual RootCertificateForm Form
		{
			get => this._form ??= new RootCertificateForm();
			set => this._form = value;
		}

		#endregion
	}
}