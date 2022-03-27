using System.ComponentModel.DataAnnotations;

namespace Application.Models.Views.Shared.Forms
{
	public abstract class LifetimeCertificateForm : BasicCertificateForm, ILifetimeCertificateForm
	{
		#region Properties

		[Range(1, 1200, ErrorMessage = "\"Lifetime\" must be between 1 and 1200.")]
		public virtual ushort? Lifetime { get; set; }

		#endregion
	}
}