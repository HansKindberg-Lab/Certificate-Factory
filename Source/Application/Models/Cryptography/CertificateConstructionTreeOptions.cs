namespace Application.Models.Cryptography
{
	public class CertificateConstructionTreeOptions
	{
		#region Properties

		/// <summary>
		/// Default values for all certificate-properties if a property is not set explicitly.
		/// </summary>
		public virtual CertificateConstructionOptions Defaults { get; set; } = new()
		{
			AsymmetricAlgorithm = "RSA:2048:Pkcs1"
		};

		/// <summary>
		/// The name for the zip-file that is created.
		/// </summary>
		public virtual string Name { get; set; } = "Certificates";

		/// <summary>
		/// The root-certificates to create.
		/// </summary>
		public virtual IDictionary<string, CertificateConstructionNodeOptions> Roots { get; } = new Dictionary<string, CertificateConstructionNodeOptions>(StringComparer.OrdinalIgnoreCase);

		/// <summary>
		/// Default values for all root-certificate-properties if a property is not set explicitly.
		/// </summary>
		public virtual CertificateConstructionOptions RootsDefaults { get; set; }

		#endregion
	}
}