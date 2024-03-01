using System.Net;

namespace Application.Models.Cryptography
{
	/// <inheritdoc />
	public interface ISubjectAlternativeNameOptions : ICloneable<ISubjectAlternativeNameOptions>
	{
		#region Properties

		ISet<string> DnsNames { get; }
		ISet<string> EmailAddresses { get; }
		ICollection<IPAddress> IpAddresses { get; }
		ICollection<Uri> Uris { get; }
		ISet<string> UserPrincipalNames { get; }

		#endregion
	}
}