using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Application.Models.Cryptography.Extensions;

namespace Application.Models.Cryptography
{
	/// <inheritdoc />
	public class X509Certificate2Wrapper : ICertificate
	{
		#region Fields

		private const string _certificatePemLabel = "CERTIFICATE";

		#endregion

		#region Constructors

		public X509Certificate2Wrapper(X509Certificate2 certificate, ILoggerFactory loggerFactory)
		{
			this.WrappedCertificate = certificate ?? throw new ArgumentNullException(nameof(certificate));
			this.LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
			this.Logger = loggerFactory.CreateLogger(this.GetType());
		}

		#endregion

		#region Properties

		public virtual bool Archived => this.WrappedCertificate.Archived;
		protected internal virtual string CertificatePemLabel => _certificatePemLabel;
		protected internal virtual bool Disposed { get; set; }
		public virtual string FriendlyName => this.WrappedCertificate.FriendlyName;
		public virtual bool HasPrivateKey => this.WrappedCertificate.HasPrivateKey;
		public virtual string Issuer => this.WrappedCertificate.Issuer;
		public virtual string KeyAlgorithm => this.WrappedCertificate.GetKeyAlgorithm();
		public virtual string KeyAlgorithmName => new Oid(this.WrappedCertificate.GetKeyAlgorithm()).FriendlyName;
		protected internal virtual ILogger Logger { get; }
		protected internal virtual ILoggerFactory LoggerFactory { get; }
		public virtual DateTime NotAfter => this.WrappedCertificate.NotAfter;
		public virtual DateTime NotBefore => this.WrappedCertificate.NotBefore;
		public virtual IEnumerable<byte> RawData => this.WrappedCertificate.RawData;
		public virtual string SerialNumber => this.WrappedCertificate.SerialNumber;
		public virtual Oid SignatureAlgorithm => this.WrappedCertificate.SignatureAlgorithm;
		public virtual ICertificateStore Store { get; set; }
		public virtual string Subject => this.WrappedCertificate.Subject;
		public virtual string Thumbprint => this.WrappedCertificate.Thumbprint;
		public virtual int Version => this.WrappedCertificate.Version;
		public virtual X509Certificate2 WrappedCertificate { get; }

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

		public virtual IEnumerable<byte> Export(X509ContentType contentType, string password)
		{
			return this.WrappedCertificate.Export(contentType, password);
		}

		public virtual CertificateAuthorityOptions GetCertificateAuthorityInformation()
		{
			var basicConstraintsExtension = this.WrappedCertificate.Extensions.OfType<X509BasicConstraintsExtension>().FirstOrDefault();

			if(basicConstraintsExtension is not { CertificateAuthority: true })
				return null;

			return new CertificateAuthorityOptions
			{
				PathLengthConstraint = basicConstraintsExtension.HasPathLengthConstraint ? basicConstraintsExtension.PathLengthConstraint : null
			};
		}

		public virtual string GetCertificatePem()
		{
			return new string(PemEncoding.Write(this.CertificatePemLabel, (this.RawData ?? []).ToArray()));
		}

		public virtual IEnumerable<ICertificate> GetChain()
		{
			using(var chain = this.GetChainInternal())
			{
				return chain.ChainElements.Select(element => new X509Certificate2Wrapper(element.Certificate, this.LoggerFactory));
			}
		}

		protected internal virtual X509Chain GetChainInternal()
		{
			var chain = new X509Chain();

			chain.Build(this.WrappedCertificate);

			return chain;
		}

		public virtual EnhancedKeyUsage? GetEnhancedKeyUsage()
		{
			return EnhancedKeyUsageExtension.GetByExtension(this.WrappedCertificate.Extensions.OfType<X509EnhancedKeyUsageExtension>().FirstOrDefault());
		}

		public virtual X509KeyUsageFlags GetKeyUsage()
		{
			var keyUsageExtension = this.WrappedCertificate.Extensions.OfType<X509KeyUsageExtension>().FirstOrDefault();

			return keyUsageExtension?.KeyUsages ?? X509KeyUsageFlags.None;
		}

		public virtual IEnumerable<byte> GetPfx(string password)
		{
			return this.Export(X509ContentType.Pfx, password);
		}

		public virtual IEnumerable<byte> GetPkcs12(string password)
		{
			return this.Export(X509ContentType.Pkcs12, password);
		}

		public virtual AsymmetricAlgorithm GetPrivateKeyAsymmetricAlgorithm()
		{
			if(!this.WrappedCertificate.HasPrivateKey)
			{
				if(this.Logger.IsEnabled(LogLevel.Warning))
					this.Logger.LogWarning($"The certificate \"{this.WrappedCertificate}\" has no private-key.");

				return null;
			}

			var ecdsa = this.WrappedCertificate.GetECDsaPrivateKey();

			if(ecdsa != null)
				return ecdsa;

			var rsa = this.WrappedCertificate.GetRSAPrivateKey();

			if(rsa != null)
				return rsa;

			if(this.Logger.IsEnabled(LogLevel.Warning))
				this.Logger.LogWarning($"The private-key asymmetric algorithm for certificate \"{this.WrappedCertificate}\" is not supported.");

			return null;
		}

		public virtual SubjectAlternativeNameOptions GetSubjectAlternativeNameInformation()
		{
			var subjectAlternativeNameExtension = this.WrappedCertificate.Extensions.OfType<X509SubjectAlternativeNameExtension>().FirstOrDefault();

			if(subjectAlternativeNameExtension == null || (!subjectAlternativeNameExtension.EnumerateDnsNames().Any() && !subjectAlternativeNameExtension.EnumerateIPAddresses().Any()))
				return null;

			var subjectAlternativeNameInformation = new SubjectAlternativeNameOptions();

			foreach(var dnsName in subjectAlternativeNameExtension.EnumerateDnsNames())
			{
				subjectAlternativeNameInformation.DnsNames.Add(dnsName);
			}

			foreach(var ipAddress in subjectAlternativeNameExtension.EnumerateIPAddresses())
			{
				subjectAlternativeNameInformation.IpAddresses.Add(ipAddress);
			}

			return subjectAlternativeNameInformation;
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