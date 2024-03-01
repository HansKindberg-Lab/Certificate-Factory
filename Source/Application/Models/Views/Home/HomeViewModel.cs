namespace Application.Models.Views.Home
{
	public class HomeViewModel
	{
		#region Properties

		public virtual IList<StoreCertificateViewModel> Certificates { get; } = [];

		#endregion
	}
}