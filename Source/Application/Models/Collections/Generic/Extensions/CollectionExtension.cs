namespace Application.Models.Collections.Generic.Extensions
{
	public static class CollectionExtension
	{
		#region Methods

		public static void Add<T>(this ICollection<T> collection, IEnumerable<T> items)
		{
			ArgumentNullException.ThrowIfNull(collection);

			foreach(var item in items ?? Enumerable.Empty<T>())
			{
				collection.Add(item);
			}
		}

		#endregion
	}
}