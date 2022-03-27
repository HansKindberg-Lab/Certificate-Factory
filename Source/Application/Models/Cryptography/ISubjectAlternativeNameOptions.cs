using System.Net;

namespace Application.Models.Cryptography
{
	/// <inheritdoc />
	public interface ISubjectAlternativeNameOptions : ICloneable<ISubjectAlternativeNameOptions>
	{
		#region Properties

		ISet<string> DnsNames { get; }
		ISet<string> EmailAddresses { get; }
		ISet<IPAddress> IpAddresses { get; }
		ISet<Uri> Uris { get; }
		ISet<string> UserPrincipalNames { get; }

		#endregion
	}
}