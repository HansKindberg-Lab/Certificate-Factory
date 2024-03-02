namespace Application.Models.Extensions
{
	public static class StringExtension
	{
		#region Methods

		public static string ReplaceStartOfEachLine(this string value, string oldValue, string newValue)
		{
			return value.ReplaceStartOfEachLine(oldValue, newValue, Environment.NewLine);
		}

		public static string ReplaceStartOfEachLine(this string value, string oldValue, string newValue, string newLine)
		{
			ArgumentNullException.ThrowIfNull(value);

			const StringComparison comparison = StringComparison.Ordinal;
			var lines = value.Split(newLine);

			for(var i = 0; i < lines.Length; i++)
			{
				var line = lines[i];

				if(line.StartsWith(oldValue, comparison))
				{
					var parts = line.SplitInParts(oldValue.Length).ToList();
					line = string.Empty;

					for(var j = 0; j < parts.Count; j++)
					{
						if(string.Equals(parts[j], oldValue, comparison))
						{
							line += newValue;
						}
						else
						{
							line += string.Join(string.Empty, parts.Skip(j));
							break;
						}
					}
				}

				lines[i] = line;
			}

			return string.Join(newLine, lines);
		}

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