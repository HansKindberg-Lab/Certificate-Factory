using System.Text.Json;
using Application.Models.Extensions;

namespace Application.Models.Text.Json.Extensions
{
	public static class JsonSerializerExtension
	{
		#region Fields

		private const string _defaultIndentation = "  ";

		#endregion

		#region Methods

		public static string Serialize<TValue>(TValue value, JsonSerializerOptions options = null, JsonIndentationOptions indentation = null)
		{
			var json = JsonSerializer.Serialize(value, options);

			if(options is not { WriteIndented: true } || indentation == null)
				return json;

			var indent = string.Empty;

			for(var i = 0; i < indentation.Size; i++)
			{
				indent += indentation.Character;
			}

			json = json.ReplaceStartOfEachLine(_defaultIndentation, indent);

			return json;
		}

		#endregion
	}
}