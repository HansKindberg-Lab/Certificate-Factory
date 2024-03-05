using System.ComponentModel;

namespace Application.Models.Cryptography
{
	[Flags]
	public enum EnhancedKeyUsage
	{
		None = 0,
		[Description("1.3.6.1.5.5.7.3.1")] ServerAuthentication = 1,
		[Description("1.3.6.1.5.5.7.3.2")] ClientAuthentication = 2,
		[Description("1.3.6.1.5.5.7.3.3")] CodeSigning = 4,
		[Description("1.3.6.1.5.5.7.3.4")] SecureEmail = 8,
		[Description("1.3.6.1.5.5.7.3.8")] TimestampSigning = 16
	}
}