using System.Globalization;
using System.Text.RegularExpressions;

namespace Application.Models.Extensions
{
	public static class StringExtension
	{
		#region Fields

		public const char DefaultWildcardCharacter = '*';

		#endregion

		#region Nested types

		extension(string value)
		{
			#region Methods

			public bool Like(string pattern)
			{
				return value.Like(pattern, DefaultWildcardCharacter);
			}

			public bool Like(string pattern, char wildcardCharacter)
			{
				return value.Like(pattern, wildcardCharacter, true);
			}

			public bool Like(string pattern, bool caseInsensitive)
			{
				return value.Like(pattern, DefaultWildcardCharacter, caseInsensitive);
			}

			public bool Like(string pattern, char wildcardCharacter, bool caseInsensitive)
			{
				ArgumentNullException.ThrowIfNull(value);

				ArgumentNullException.ThrowIfNull(pattern);

				var regexOptions = RegexOptions.Compiled;

				if(caseInsensitive)
					regexOptions |= RegexOptions.IgnoreCase;

				var regexPattern = pattern.Replace(wildcardCharacter.ToString(CultureInfo.InvariantCulture), "*");
				regexPattern = "^" + Regex.Escape(regexPattern).Replace("\\*", ".*") + "$";

				return Regex.IsMatch(value, regexPattern, regexOptions);
			}

			public string ReplaceStartOfEachLine(string oldValue, string newValue)
			{
				return value.ReplaceStartOfEachLine(oldValue, newValue, Environment.NewLine);
			}

			public string ReplaceStartOfEachLine(string oldValue, string newValue, string newLine)
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
			public IEnumerable<string> SplitInParts(int partLength)
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

		#endregion
	}
}