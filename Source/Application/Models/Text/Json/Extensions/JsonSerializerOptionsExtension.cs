using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Models.Text.Json.Serialization;
using Application.Models.Text.Json.Serialization.Metadata;

namespace Application.Models.Text.Json.Extensions
{
	public static class JsonSerializerOptionsExtension
	{
		#region Fields

		private static JsonSerializerOptions _default;

		#endregion

		#region Properties

		public static JsonSerializerOptions Default
		{
			get
			{
				if(_default == null)
				{
					_default = new JsonSerializerOptions(JsonSerializerDefaults.Web);
					_default.SetDefaults();
				}

				return _default;
			}
		}

		#endregion

		#region Methods

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