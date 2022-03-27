namespace Application.Models.Cryptography.Storing.Configuration
{
	public class StoreCertificateOptions
	{
		#region Properties

		public virtual string CertificatePem { get; set; }
		public virtual string PrivateKeyPem { get; set; }

		#endregion
	}
}