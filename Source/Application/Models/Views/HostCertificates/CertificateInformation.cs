namespace Application.Models.Views.HostCertificates
{
	public class CertificateInformation
	{
		#region Properties

		public virtual string Identifier { get; set; }
		public virtual string StoreLocation { get; set; }
		public virtual string StoreName { get; set; }
		public virtual string Subject { get; set; }
		public virtual string Thumbprint { get; set; }

		#endregion
	}
}