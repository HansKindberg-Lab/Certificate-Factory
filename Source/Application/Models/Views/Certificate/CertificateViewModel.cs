namespace Application.Models.Views.Certificate
{
	public class CertificateViewModel
	{
		#region Fields

		private CertificateForm _form;

		#endregion

		#region Properties

		public virtual CertificateForm Form
		{
			get => this._form ??= new CertificateForm();
			set => this._form = value;
		}

		#endregion
	}
}