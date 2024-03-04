using System.Security.Cryptography.X509Certificates;

namespace Application.Models.Cryptography
{
	public class CertificateStoreFactory(ICertificateFactory certificateFactory) : ICertificateStoreFactory
	{
		#region Properties

		protected internal virtual ICertificateFactory CertificateFactory { get; } = certificateFactory ?? throw new ArgumentNullException(nameof(certificateFactory));

		#endregion

		#region Methods

		public virtual ICertificateStore Create(StoreLocation location, StoreName? name, string registryName)
		{
			return new CertificateStore(this.CertificateFactory, location, name, registryName);
		}

		#endregion
	}
}