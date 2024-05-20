using System.Text;
using Application.Models.Collections;
using Application.Models.Collections.Generic.Extensions;

namespace Application.Models.Cryptography
{
	/// <inheritdoc cref="IAuthorityInformationAccessOptions" />
	public class AuthorityInformationAccessOptions : IAuthorityInformationAccessOptions, ICloneable<AuthorityInformationAccessOptions>
	{
		#region Properties

		public virtual ICollection<Uri> CaIssuerUris { get; } = new SortedSet<Uri>(new UriComparer());
		public virtual ICollection<Uri> OcspUris { get; } = new SortedSet<Uri>(new UriComparer());

		#endregion

		#region Methods

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		IAuthorityInformationAccessOptions ICloneable<IAuthorityInformationAccessOptions>.Clone()
		{
			return this.Clone();
		}

		public virtual AuthorityInformationAccessOptions Clone()
		{
			var clone = new AuthorityInformationAccessOptions();

			clone.CaIssuerUris.Add(this.CaIssuerUris.Select(uri => uri == null ? null : new Uri(new StringBuilder(uri.OriginalString).ToString())));
			clone.OcspUris.Add(this.OcspUris.Select(uri => uri == null ? null : new Uri(new StringBuilder(uri.OriginalString).ToString())));

			return clone;
		}

		#endregion
	}
}