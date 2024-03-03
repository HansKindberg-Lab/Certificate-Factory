using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Models.Text.Json.Serialization
{
	public class IpAddressConverter : JsonConverter<IPAddress>
	{
		#region Methods

		public override IPAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return IPAddress.TryParse(reader.GetString(), out var ipAddress) ? ipAddress : null;
		}

		public override void Write(Utf8JsonWriter writer, IPAddress value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToString());
		}

		#endregion
	}
}