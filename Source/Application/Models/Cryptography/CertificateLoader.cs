using System.Security.Cryptography.X509Certificates;
using Application.Models.Extensions;

namespace Application.Models.Cryptography
{
	public class CertificateLoader(ICertificateFactory factory, ICertificateStoreLoader storeLoader) : ICertificateLoader
	{
		#region Properties

		protected internal virtual ICertificateFactory Factory { get; } = factory ?? throw new ArgumentNullException(nameof(factory));
		protected internal virtual ICertificateStoreLoader StoreLoader { get; } = storeLoader ?? throw new ArgumentNullException(nameof(storeLoader));

		#endregion

		#region Methods

		public virtual IEnumerable<ICertificate> Find(string friendlyName = null, string issuer = null, string serialNumber = null, StoreLocation? storeLocation = null, string storeName = null, string subject = null, string thumbprint = null)
		{
			if(friendlyName == null && issuer == null && serialNumber == null && storeLocation == null && storeName == null && subject == null && thumbprint == null)
				return [];

			var result = new List<ICertificate>();

			var locations = new List<StoreLocation>();

			if(storeLocation == null)
				locations.AddRange([StoreLocation.CurrentUser, StoreLocation.LocalMachine]);
			else
				locations.Add(storeLocation.Value);

			var stores = this.StoreLoader.List().ToArray();

			foreach(var store in stores)
			{
				if(!locations.Contains(store.Location))
					continue;

				if(storeName != null && !store.Name.Like(storeName))
					continue;

				using(var x509Store = new X509Store(store.Name, store.Location))
				{
					x509Store.Open(OpenFlags.OpenExistingOnly);

					foreach(var certificate in x509Store.Certificates)
					{
						if(!this.Include(certificate, friendlyName, issuer, serialNumber, subject, thumbprint))
							continue;

						result.Add(this.Factory.Create(certificate, store));
					}
				}
			}

			return result;
		}

		public virtual ICertificate Get(StoreLocation storeLocation, string storeName, string thumbprint)
		{
			var store = this.StoreLoader.Get(storeLocation, storeName);

			if(store == null)
				return null;

			using(var x509Store = new X509Store(storeName, storeLocation))
			{
				x509Store.Open(OpenFlags.OpenExistingOnly);

				var certificate = x509Store.Certificates.FirstOrDefault(certificate => string.Equals(certificate.Thumbprint, thumbprint, StringComparison.OrdinalIgnoreCase));

				return certificate != null ? this.Factory.Create(certificate, store) : null;
			}
		}

		protected internal virtual bool Include(X509Certificate2 certificate, string friendlyName = null, string issuer = null, string serialNumber = null, string subject = null, string thumbprint = null)
		{
			if(certificate == null)
				return false;

			if(friendlyName != null && !certificate.FriendlyName.Like(friendlyName))
				return false;

			if(issuer != null && !certificate.Issuer.Like(issuer))
				return false;

			if(serialNumber != null && !certificate.SerialNumber.Like(serialNumber))
				return false;

			if(subject != null && !certificate.Subject.Like(subject))
				return false;

			if(thumbprint != null && !certificate.Thumbprint.Like(thumbprint))
				return false;

			return true;
		}

		#endregion
	}
}