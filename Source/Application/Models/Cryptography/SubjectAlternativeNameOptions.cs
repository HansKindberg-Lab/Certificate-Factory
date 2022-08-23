using System.Net;
using Application.Models.Collections;
using Application.Models.Collections.Generic.Extensions;

namespace Application.Models.Cryptography
{
	/// <inheritdoc />
	public class SubjectAlternativeNameOptions : ISubjectAlternativeNameOptions
	{
		#region Properties

		public virtual ISet<string> DnsNames { get; } = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
		public virtual ISet<string> EmailAddresses { get; } = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
		public virtual ISet<IPAddress> IpAddresses { get; } = new SortedSet<IPAddress>(new IpAddressComparer());
		public virtual ISet<Uri> Uris { get; } = new SortedSet<Uri>(new UriComparer());
		public virtual ISet<string> UserPrincipalNames { get; } = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);

		#endregion

		#region Methods

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		public virtual ISubjectAlternativeNameOptions Clone()
		{
			var clone = new SubjectAlternativeNameOptions();

			clone.DnsNames.Add(this.DnsNames.Select(dnsName => new string(dnsName)));
			clone.EmailAddresses.Add(this.EmailAddresses.Select(emailAddress => new string(emailAddress)));
			clone.IpAddresses.Add(this.IpAddresses.Select(ipAddress => new IPAddress(ipAddress.GetAddressBytes())));
			clone.Uris.Add(this.Uris.Select(uri => new Uri(new string(uri.OriginalString))));
			clone.UserPrincipalNames.Add(this.UserPrincipalNames.Select(userPrincipalName => new string(userPrincipalName)));

			return clone;
		}

		#endregion
	}
}