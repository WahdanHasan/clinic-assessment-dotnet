using clinic_assessment_redone.Helpers.Misc;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace clinic_assessment_redone.Helpers.Converter
{
    public class TimeOnlyListConverter : JsonConverter<List<TimeOnly>>
    {
        public override List<TimeOnly>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<TimeOnly>? timeOnlyList = new List<TimeOnly>();

            string tempReaderToken;
            while (!reader.IsFinalBlock)
            {
                tempReaderToken = reader.GetString();

                timeOnlyList.Add(Util.stringTimeToTime(tempReaderToken));
            }
            
            return timeOnlyList;
        }

        public override void Write(Utf8JsonWriter writer, List<TimeOnly> value, JsonSerializerOptions options)
        {
            string output = "";

            foreach(TimeOnly time in value)
            {
                output += time.ToString(Constants.TIME_FORMAT, CultureInfo.InvariantCulture) + " ";
            }

            writer.WriteStringValue(output);
        }
    }
}
