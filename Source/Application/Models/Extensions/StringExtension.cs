namespace Application.Models.Extensions
{
	public static class StringExtension
	{
		#region Methods

		/// <summary>
		/// https://stackoverflow.com/questions/4133377/splitting-a-string-number-every-nth-character-number#answer-4133475
		/// </summary>
		public static IEnumerable<string> SplitInParts(this string value, int partLength)
		{
			ArgumentNullException.ThrowIfNull(value);

			if(partLength <= 0)
				throw new ArgumentException("Part length has to be positive.", nameof(partLength));

			for(var i = 0; i < value.Length; i += partLength)
			{
				yield return value.Substring(i, Math.Min(partLength, value.Length - i));
			}
		}

		#endregion
	}
}