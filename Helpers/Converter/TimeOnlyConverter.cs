using clinic_assessment_redone.Helpers.Misc;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace clinic_assessment_redone.Helpers.Converter
{
    public class TimeOnlyConverter : JsonConverter<TimeOnly>
    {
        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return TimeOnly.ParseExact(reader.GetString()!,
                         Consts.TIME_FORMAT, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Consts.TIME_FORMAT, CultureInfo.InvariantCulture));
        }
    }
}
