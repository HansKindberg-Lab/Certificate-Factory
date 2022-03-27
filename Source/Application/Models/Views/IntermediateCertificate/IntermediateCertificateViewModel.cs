namespace Application.Models.Views.IntermediateCertificate
{
	public class IntermediateCertificateViewModel
	{
		#region Fields

		private IntermediateCertificateForm _form;

		#endregion

		#region Properties

		public virtual IntermediateCertificateForm Form
		{
			get => this._form ??= new IntermediateCertificateForm();
			set => this._form = value;
		}

		#endregion
	}
}