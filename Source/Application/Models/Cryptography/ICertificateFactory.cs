namespace Application.Models.Cryptography
{
	public interface ICertificateFactory
	{
		#region Methods

		ICertificate Create(IAsymmetricAlgorithmOptions asymmetricAlgorithmOptions, ICertificateOptions certificateOptions);

		#endregion
	}
}