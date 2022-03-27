namespace Application.Models.Cryptography.Configuration
{
	public class CertificateFactoryOptions
	{
		#region Properties

		public virtual TimeSpan DefaultCertificateLifetime { get; set; } = TimeSpan.FromDays(730); // Two years.

		/// <summary>
		/// The default serial-number size. The number of bytes a generated serial-number will have.
		/// </summary>
		public virtual byte DefaultSerialNumberSize { get; set; } = 20;

		#endregion
	}
}