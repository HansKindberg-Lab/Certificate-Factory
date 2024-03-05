namespace Application.Models.Cryptography
{
	public interface ICertificateConstructionTreeOptionsFactory
	{
		#region Methods

		CertificateConstructionTreeOptions Create(ICertificate certificate);

		#endregion
	}
}