using System.Net;
using System.Text;
using Application.Models.Collections;
using Application.Models.Collections.Generic.Extensions;

namespace Application.Models.Cryptography
{
	/// <inheritdoc cref="ISubjectAlternativeNameOptions" />
	public class SubjectAlternativeNameOptions : ISubjectAlternativeNameOptions, ICloneable<SubjectAlternativeNameOptions>
	{
		#region Properties

		public virtual ISet<string> DnsNames { get; } = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
		public virtual ISet<string> EmailAddresses { get; } = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
		public virtual ICollection<IPAddress> IpAddresses { get; } = new SortedSet<IPAddress>(new IpAddressComparer());
		public virtual ICollection<Uri> Uris { get; } = new SortedSet<Uri>(new UriComparer());
		public virtual ISet<string> UserPrincipalNames { get; } = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);

		#endregion

		#region Methods

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		ISubjectAlternativeNameOptions ICloneable<ISubjectAlternativeNameOptions>.Clone()
		{
			return this.Clone();
		}

		public virtual SubjectAlternativeNameOptions Clone()
		{
			var clone = new SubjectAlternativeNameOptions();

			clone.DnsNames.Add(this.DnsNames.Select(dnsName => dnsName == null ? null : new StringBuilder(dnsName).ToString()));
			clone.EmailAddresses.Add(this.EmailAddresses.Select(emailAddress => emailAddress == null ? null : new StringBuilder(emailAddress).ToString()));
			clone.IpAddresses.Add(this.IpAddresses.Select(ipAddress => ipAddress == null ? null : new IPAddress(ipAddress.GetAddressBytes())));
			clone.Uris.Add(this.Uris.Select(uri => uri == null ? null : new Uri(new StringBuilder(uri.OriginalString).ToString())));
			clone.UserPrincipalNames.Add(this.UserPrincipalNames.Select(userPrincipalName => userPrincipalName == null ? null : new StringBuilder(userPrincipalName).ToString()));

			return clone;
		}

		#endregion
	}
}