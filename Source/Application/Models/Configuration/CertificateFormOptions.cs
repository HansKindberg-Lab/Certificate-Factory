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
			{ "CertificateAuthorityHasPathLengthConstraint", 110 },
			{ "CertificateAuthorityPathLengthConstraint", 120 },
			{ "DnsNames", 130 },
			{ "EmailAddresses", 140 },
			{ "IpAddresses", 150 },
			{ "Uris", 160 },
			{ "UserPrincipalNames", 170 },
			{ "Password", 180 }
		};

		#endregion
	}
}