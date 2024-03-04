using System.Security.Cryptography.X509Certificates;

namespace Application.Models.Cryptography
{
	public interface ICertificateLoader
	{
		#region Methods

		IEnumerable<ICertificate> Find(string friendlyName = null, string issuer = null, string serialNumber = null, StoreLocation? storeLocation = null, string storeName = null, string subject = null, string thumbprint = null);
		ICertificate Get(StoreLocation storeLocation, string storeName, string thumbprint);

		#endregion
	}
}