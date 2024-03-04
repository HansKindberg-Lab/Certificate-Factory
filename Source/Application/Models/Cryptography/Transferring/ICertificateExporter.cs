namespace Application.Models.Cryptography.Transferring
{
	public interface ICertificateExporter
	{
		#region Methods

		ICertificateTransfer Export(ICertificate certificate, string password);

		#endregion
	}
}