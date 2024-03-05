using System.Collections.Concurrent;
using System.ComponentModel;
using Application.Models.ComponentModel;

namespace Application.Models.Cryptography.Extensions
{
	public static class EnhancedKeyUsageExtension
	{
		#region Fields

		private static readonly EnumerationAttributeRetriever<DescriptionAttribute, EnhancedKeyUsage> _descriptionRetriever = new();
		private static readonly ConcurrentDictionary<EnhancedKeyUsage, ISet<string>> _descriptionsCache = new();

		#endregion

		#region Methods

		public static ISet<string> Descriptions(this EnhancedKeyUsage enhancedKeyUsage)
		{
			return _descriptionsCache.GetOrAdd(enhancedKeyUsage, key =>
			{
				var descriptions = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);

				foreach(var value in Enum.GetValues<EnhancedKeyUsage>())
				{
					if(!enhancedKeyUsage.HasFlag(value))
						continue;

					var description = _descriptionRetriever.GetAttribute(value)?.Description;

					if(description != null)
						descriptions.Add(description);
				}

				return descriptions;
			});
		}

		#endregion
	}
}