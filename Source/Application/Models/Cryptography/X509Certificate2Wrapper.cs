using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Application.Models.Cryptography
{
	/// <inheritdoc />
	public class X509Certificate2Wrapper(X509Certificate2 certificate) : ICertificate
	{
		#region Properties

		public virtual bool Archived => this.WrappedCertificate.Archived;
		protected internal virtual bool Disposed { get; set; }
		public virtual bool HasPrivateKey => this.WrappedCertificate.HasPrivateKey;
		public virtual string Issuer => this.WrappedCertificate.Issuer;
		public virtual string KeyAlgorithm => this.WrappedCertificate.GetKeyAlgorithm();
		public virtual string KeyAlgorithmName => new Oid(this.WrappedCertificate.GetKeyAlgorithm()).FriendlyName;
		public virtual DateTime NotAfter => this.WrappedCertificate.NotAfter;
		public virtual DateTime NotBefore => this.WrappedCertificate.NotBefore;
		public virtual IEnumerable<byte> RawData => this.WrappedCertificate.RawData;
		public virtual string SerialNumber => this.WrappedCertificate.SerialNumber;
		public virtual string Subject => this.WrappedCertificate.Subject;
		public virtual string Thumbprint => this.WrappedCertificate.Thumbprint;
		public virtual X509Certificate2 WrappedCertificate { get; } = certificate ?? throw new ArgumentNullException(nameof(certificate));

		#endregion

		#region Methods

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if(this.Disposed)
				return;

			if(disposing)
				this.WrappedCertificate.Dispose();

			this.Disposed = true;
		}

		public override string ToString()
		{
			return this.WrappedCertificate.ToString();
		}

		public virtual string ToString(bool verbose)
		{
			return this.WrappedCertificate.ToString(verbose);
		}

		#endregion
	}
}