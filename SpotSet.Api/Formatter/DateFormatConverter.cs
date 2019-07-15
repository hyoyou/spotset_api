using System;
using System.Globalization;
using Newtonsoft.Json;
namespace SpotSet.Api.Formatter
{
    public class DateFormatConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var setlistModelEventDate = (string) reader.Value;
            var date = DateTime.ParseExact(setlistModelEventDate, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                .ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);       

            return date;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}