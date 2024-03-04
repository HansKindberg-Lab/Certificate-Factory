using System.Security.Cryptography.X509Certificates;

namespace Application.Models.Cryptography
{
	public interface ICertificateStoreLoader
	{
		#region Methods

		IEnumerable<ICertificateStore> Find(StoreLocation? location = null, string name = null);
		ICertificateStore Get(StoreLocation location, string name);
		IEnumerable<ICertificateStore> List();

		#endregion
	}
}