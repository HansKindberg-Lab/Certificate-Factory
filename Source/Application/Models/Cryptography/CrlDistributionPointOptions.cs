using System.Text;
using Application.Models.Collections;
using Application.Models.Collections.Generic.Extensions;

namespace Application.Models.Cryptography
{
	/// <inheritdoc cref="ICrlDistributionPointOptions" />
	public class CrlDistributionPointOptions : ICrlDistributionPointOptions, ICloneable<CrlDistributionPointOptions>
	{
		#region Properties

		public virtual ICollection<Uri> Uris { get; } = new SortedSet<Uri>(new UriComparer());

		#endregion

		#region Methods

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		ICrlDistributionPointOptions ICloneable<ICrlDistributionPointOptions>.Clone()
		{
			return this.Clone();
		}

		public virtual CrlDistributionPointOptions Clone()
		{
			var clone = new CrlDistributionPointOptions();

			clone.Uris.Add(this.Uris.Select(uri => uri == null ? null : new Uri(new StringBuilder(uri.OriginalString).ToString())));

			return clone;
		}

		#endregion
	}
}