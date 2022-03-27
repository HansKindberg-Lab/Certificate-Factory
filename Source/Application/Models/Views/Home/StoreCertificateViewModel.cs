namespace Application.Models.Views.Home
{
	public class StoreCertificateViewModel
	{
		#region Properties

		public virtual string Issuer { get; set; }
		public virtual string KeyAlgorithm { get; set; }
		public virtual DateTime NotAfter { get; set; }
		public virtual DateTime NotBefore { get; set; }
		public virtual string SerialNumber { get; set; }
		public virtual string Subject { get; set; }
		public virtual string Thumbprint { get; set; }

		#endregion
	}
}