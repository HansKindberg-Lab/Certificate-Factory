namespace Application.Models.Cryptography
{
	public interface ICertificateConstructionHelper
	{
		#region Methods

		ICertificateOptions CreateCertificateOptions(CertificateConstructionOptions certificateConstructionOptions, ICertificate issuer = null);
		void SetDefaults(CertificateConstructionOptions certificateConstructionOptions, IEnumerable<CertificateConstructionOptions> defaults);

		#endregion
	}
}