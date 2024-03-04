using Application.Models.Cryptography;

namespace Application.Models.Views.HostCertificate
{
	public class HostCertificateViewModel
	{
		#region Properties

		public virtual ICertificate Certificate { get; set; }
		public virtual string Error { get; set; }
		public virtual IList<string> ErrorDetails { get; } = [];
		public virtual string ValidatedReturnUrl { get; set; }

		#endregion
	}
}