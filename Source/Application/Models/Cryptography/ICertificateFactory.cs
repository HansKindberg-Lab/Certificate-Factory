namespace Application.Models.Cryptography
{
	public interface ICertificateFactory
	{
		#region Methods

		/// <summary>
		/// Create a new certificate.
		/// </summary>
		ICertificate Create(IAsymmetricAlgorithmOptions asymmetricAlgorithmOptions, ICertificateOptions certificateOptions);

		#endregion
	}
}