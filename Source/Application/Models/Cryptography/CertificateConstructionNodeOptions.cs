using System.Text;

namespace Application.Models.Cryptography
{
	public class CertificateConstructionNodeOptions : ICloneable<CertificateConstructionNodeOptions>
	{
		#region Properties

		public virtual CertificateConstructionOptions Certificate { get; set; }

		/// <summary>
		/// The certificates this certificate-node will be the issuer for.
		/// </summary>
		public virtual IDictionary<string, CertificateConstructionNodeOptions> IssuedCertificates { get; } = new Dictionary<string, CertificateConstructionNodeOptions>(StringComparer.OrdinalIgnoreCase);

		/// <summary>
		/// Default values for all issued-certificate-properties if a property is not set explicitly.
		/// </summary>
		public virtual CertificateConstructionOptions IssuedCertificatesDefaults { get; set; }

		#endregion

		#region Methods

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		public virtual CertificateConstructionNodeOptions Clone()
		{
			var clone = new CertificateConstructionNodeOptions
			{
				Certificate = this.Certificate?.Clone(),
				IssuedCertificatesDefaults = this.IssuedCertificatesDefaults?.Clone()
			};

			foreach(var (key, value) in this.IssuedCertificates)
			{
				clone.IssuedCertificates.Add(new StringBuilder(key).ToString(), value?.Clone());
			}

			return clone;
		}

		#endregion
	}
}