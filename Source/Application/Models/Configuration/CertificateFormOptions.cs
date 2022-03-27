namespace Application.Models.Configuration
{
	public class CertificateFormOptions
	{
		#region Properties

		public virtual IDictionary<string, int> FieldOrder { get; } = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
		{
			{ "AsymmetricAlgorithm", 10 },
			{ "Subject", 20 },
			{ "Issuer", 30 },
			{ "Lifetime", 40 },
			{ "NotBefore", 50 },
			{ "NotAfter", 60 },
			{ "HashAlgorithm", 70 },
			{ "KeyUsage", 80 },
			{ "EnhancedKeyUsage", 90 },
			{ "CertificateAuthorityEnabled", 100 },
			{ "PathLengthConstraint", 110 },
			{ "DnsNames", 120 },
			{ "EmailAddresses", 130 },
			{ "IpAddresses", 140 },
			{ "Uris", 150 },
			{ "UserPrincipalNames", 160 },
			{ "Password", 170 }
		};

		#endregion
	}
}