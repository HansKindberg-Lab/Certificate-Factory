using System.Security.Cryptography.X509Certificates;

namespace Application.Models.Cryptography
{
	public interface ICertificateStore
	{
		#region Properties

		StoreLocation Location { get; }
		string Name { get; }
		string RegistryName { get; }
		bool Standard { get; }
		StoreName? StoreName { get; }

		#endregion

		#region Methods

		IEnumerable<ICertificate> Certificates();

		#endregion
	}
}