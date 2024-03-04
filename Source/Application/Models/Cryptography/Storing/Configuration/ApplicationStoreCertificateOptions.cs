namespace Application.Models.Cryptography.Storing.Configuration
{
	public class ApplicationStoreCertificateOptions
	{
		#region Properties

		public virtual string CertificatePem { get; set; }
		public virtual string PrivateKeyPem { get; set; }

		#endregion
	}
}