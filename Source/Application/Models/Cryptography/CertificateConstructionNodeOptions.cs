namespace Application.Models.Cryptography
{
	public class CertificateConstructionNodeOptions
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
	}
}