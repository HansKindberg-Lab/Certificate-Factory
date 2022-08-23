namespace Application.Models.Views.JsonConfiguredCertificates
{
	public class JsonConfiguredCertificatesViewModel
	{
		#region Fields

		private JsonConfiguredCertificatesForm _form;

		#endregion

		#region Properties

		public virtual JsonConfiguredCertificatesForm Form
		{
			get => this._form ??= new JsonConfiguredCertificatesForm();
			set => this._form = value;
		}

		#endregion
	}
}