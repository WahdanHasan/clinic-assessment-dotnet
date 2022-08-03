using clinic_assessment_redone.Helpers.Misc;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace clinic_assessment_redone.Helpers.Converter
{
    public class DateOnlyConverter : JsonConverter<DateOnly>
    {
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateOnly.ParseExact(reader.GetString()!,
                         Constants.DATE_FORMAT, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Constants.DATE_FORMAT, CultureInfo.InvariantCulture));
        }
    }
}
