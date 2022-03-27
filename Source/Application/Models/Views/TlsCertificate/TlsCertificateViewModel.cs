namespace Application.Models.Views.TlsCertificate
{
	public class TlsCertificateViewModel
	{
		#region Fields

		private TlsCertificateForm _form;

		#endregion

		#region Properties

		public virtual TlsCertificateForm Form
		{
			get => this._form ??= new TlsCertificateForm();
			set => this._form = value;
		}

		#endregion
	}
}