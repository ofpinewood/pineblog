using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Opw.Json.Converters
{
    public class GuidConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Guid)) || (objectType == typeof(Guid?));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                Guid guid;
                if (Guid.TryParse(reader.Value.ToString(), out guid))
                    return guid;

                if (reader.TokenType == JsonToken.String)
                {
                    var shortGuid = new ShortGuid(reader.Value.ToString());
                    return shortGuid.Guid;
                }
            }
            catch (Exception ex)
            {
                throw new JsonSerializationException($"Error converting value {reader.Value} to type '{objectType}'.", ex);
            }

            throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing guid.");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var shortGuid = new ShortGuid((Guid)value);
            if (shortGuid.Guid.Equals(Guid.Empty))
            {
                writer.WriteNull();
                return;
            }

            writer.WriteValue(shortGuid.Value);
        }
    }
}
