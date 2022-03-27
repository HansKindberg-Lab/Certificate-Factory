namespace Application.Models.Collections.Generic.Extensions
{
	public static class EnumerableExtension
	{
		#region Methods

		public static string ToCommaSeparatedString(this IEnumerable<string> enumerableString)
		{
			if(enumerableString == null)
				throw new ArgumentNullException(nameof(enumerableString));

			return string.Join(',', enumerableString);
		}

		#endregion
	}
}