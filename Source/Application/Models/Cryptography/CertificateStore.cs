using System.Security.Cryptography.X509Certificates;

namespace Application.Models.Cryptography
{
	public class CertificateStore(ICertificateFactory certificateFactory, StoreLocation location, StoreName? name, string registryName) : ICertificateStore
	{
		#region Properties

		protected internal virtual ICertificateFactory CertificateFactory { get; } = certificateFactory ?? throw new ArgumentNullException(nameof(certificateFactory));
		public virtual StoreLocation Location { get; } = location;
		public virtual string Name { get; } = name != null ? name.ToString()! : registryName ?? string.Empty;
		public virtual string RegistryName { get; } = registryName;
		public virtual bool Standard { get; } = name != null;
		public virtual StoreName? StoreName { get; } = name;

		#endregion

		#region Methods

		public virtual IEnumerable<ICertificate> Certificates()
		{
			using(var store = new X509Store(this.Name, this.Location))
			{
				store.Open(OpenFlags.OpenExistingOnly);

				return store.Certificates.Select(certificate => this.CertificateFactory.Create(certificate, this));
			}
		}

		#endregion
	}
}