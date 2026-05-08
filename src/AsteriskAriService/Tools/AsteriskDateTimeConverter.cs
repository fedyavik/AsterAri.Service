using System.Text.Json;
using System.Text.Json.Serialization;

namespace AsteriskAriService.Tools
{
    public class AsteriskDateTimeConverter: JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (value == null)
                throw new JsonException();
            var fixedValue = value.Insert(value.Length - 2, ":");

            return DateTime.Parse(fixedValue);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("O"));
        }
    }
}