using System.ComponentModel;

namespace Application.Models.Cryptography
{
	[Flags]
	public enum EnhancedKeyUsage
	{
		None = 0,

		[Description(EnhancedKeyUsageValues.ServerAuthentication)]
		ServerAuthentication = 1,

		[Description(EnhancedKeyUsageValues.ClientAuthentication)]
		ClientAuthentication = 2,

		[Description(EnhancedKeyUsageValues.CodeSigning)]
		CodeSigning = 4,

		[Description(EnhancedKeyUsageValues.SecureEmail)]
		SecureEmail = 8,

		[Description(EnhancedKeyUsageValues.TimestampSigning)]
		TimestampSigning = 16
	}
}