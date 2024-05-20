using System.Security.Cryptography.X509Certificates;

namespace Application.Models.Cryptography
{
	public class CertificateConstructionTreeOptionsFactory : ICertificateConstructionTreeOptionsFactory
	{
		#region Constructors

		public CertificateConstructionTreeOptionsFactory(ILoggerFactory loggerFactory)
		{
			this.Logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger(this.GetType());
		}

		#endregion

		#region Properties

		protected internal virtual ILogger Logger { get; }

		#endregion

		#region Methods

		public virtual CertificateConstructionTreeOptions Create(ICertificate certificate)
		{
			ArgumentNullException.ThrowIfNull(certificate);

			var constructionTree = new CertificateConstructionTreeOptions();

			var certificates = certificate.GetChain().Reverse().ToList();
			var children = constructionTree.Roots;

			foreach(var certificateNode in certificates)
			{
				var certificateConstructionOptions = new CertificateConstructionOptions();

				this.PopulateCertificateConstructionOptions(certificateNode, certificateConstructionOptions);

				var key = certificateNode.Thumbprint.ToLowerInvariant();

				var value = new CertificateConstructionNodeOptions
				{
					Certificate = certificateConstructionOptions
				};

				children.Add(key, value);

				children = value.IssuedCertificates;
			}

			foreach(var item in certificates)
			{
				item.Dispose();
			}

			return constructionTree;
		}

		protected internal virtual void PopulateCertificateConstructionOptions(ICertificate certificate, CertificateConstructionOptions certificateConstructionOptions)
		{
			ArgumentNullException.ThrowIfNull(certificate);
			ArgumentNullException.ThrowIfNull(certificateConstructionOptions);

			certificateConstructionOptions.CertificateAuthority = certificate.GetCertificateAuthority();
			certificateConstructionOptions.CrlDistributionPoint = certificate.GetCrlDistributionPoint();
			certificateConstructionOptions.EnhancedKeyUsage = certificate.GetEnhancedKeyUsage();

			var keyUsage = certificate.GetKeyUsage();
			if(keyUsage != X509KeyUsageFlags.None)
				certificateConstructionOptions.KeyUsage = keyUsage;

			certificateConstructionOptions.NotAfter = certificate.NotAfter;
			certificateConstructionOptions.NotBefore = certificate.NotBefore;
			certificateConstructionOptions.SerialNumber = new SerialNumberOptions { Value = certificate.SerialNumber };
			certificateConstructionOptions.Subject = certificate.Subject;
			certificateConstructionOptions.SubjectAlternativeName = certificate.GetSubjectAlternativeName();
		}

		#endregion
	}
}