using System.ComponentModel.DataAnnotations;
using Application.Models.Views.Shared.Forms;

namespace Application.Models.Views.IntermediateCertificate
{
	public class IntermediateCertificateForm : IssuedCertificateForm
	{
		#region Properties

		[Required(ErrorMessage = "\"Issuer\" is required.")]
		public override string Issuer { get; set; }

		#endregion
	}
}