using System.Collections.Concurrent;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using Application.Models.ComponentModel;

namespace Application.Models.Cryptography.Extensions
{
	public static class EnhancedKeyUsageExtension
	{
		#region Fields

		private static readonly EnumerationAttributeRetriever<DescriptionAttribute, EnhancedKeyUsage> _descriptionRetriever = new();
		private static readonly ConcurrentDictionary<EnhancedKeyUsage, ISet<string>> _descriptionsCache = new();
		private static readonly ConcurrentDictionary<string, EnhancedKeyUsage?> _extensionToEnhancedKeyUsageMap = new();

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

		public static EnhancedKeyUsage? GetByExtension(X509EnhancedKeyUsageExtension extension)
		{
			if(extension == null)
				return null;

			var extensionValues = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);

			foreach(var oid in extension.EnhancedKeyUsages)
			{
				var value = oid?.Value;

				if(value != null)
					extensionValues.Add(value);
			}

			const char separator = '|';

			var extensionKey = string.Join(separator, extensionValues);

			var mapping = _extensionToEnhancedKeyUsageMap.GetOrAdd(extensionKey, key =>
			{
				EnhancedKeyUsage? enhancedKeyUsage = null;

				foreach(var (enumeration, attribute) in _descriptionRetriever.Dictionary)
				{
					foreach(var part in key.Split(separator))
					{
						if(string.Equals(part, attribute?.Description, StringComparison.OrdinalIgnoreCase))
							enhancedKeyUsage = enhancedKeyUsage == null ? enumeration : enhancedKeyUsage.Value | enumeration;
					}
				}

				return enhancedKeyUsage;
			});

			return mapping;
		}

		#endregion
	}
}