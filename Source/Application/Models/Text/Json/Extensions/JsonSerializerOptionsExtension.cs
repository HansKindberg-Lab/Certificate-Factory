using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Models.Text.Json.Serialization;
using Application.Models.Text.Json.Serialization.Metadata;

namespace Application.Models.Text.Json.Extensions
{
	public static class JsonSerializerOptionsExtension
	{
		#region Properties

		public static JsonSerializerOptions Default { get; } = CreateJsonSerializerOptions();

		#endregion

		#region Methods

		private static JsonSerializerOptions CreateJsonSerializerOptions()
		{
			var jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
			jsonSerializerOptions.SetDefaults();

			return jsonSerializerOptions;
		}

		public static void SetDefaults(this JsonSerializerOptions options)
		{
			ArgumentNullException.ThrowIfNull(options);

			options.Converters.Add(new JsonStringEnumConverter());
			options.Converters.Add(new IpAddressConverter());
			options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
			options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
			options.PropertyNameCaseInsensitive = true;
			options.PropertyNamingPolicy = null;
			options.TypeInfoResolver = new TypeInfoResolver();
			options.WriteIndented = true;
		}

		#endregion
	}
}