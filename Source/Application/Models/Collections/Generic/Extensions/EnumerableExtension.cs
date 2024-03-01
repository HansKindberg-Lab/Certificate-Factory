namespace Application.Models.Collections.Generic.Extensions
{
	public static class EnumerableExtension
	{
		#region Methods

		public static string ToCommaSeparatedString(this IEnumerable<string> enumerableString)
		{
			ArgumentNullException.ThrowIfNull(enumerableString);

			return string.Join(',', enumerableString);
		}

		#endregion
	}
}