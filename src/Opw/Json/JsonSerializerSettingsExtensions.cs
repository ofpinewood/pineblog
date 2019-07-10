using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Opw.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Opw.Json
{
    public static class JsonSerializerSettingsExtensions
    {
        public static void AddDefaultJsonSerializerSettings(this JsonSerializerSettings serializerSettings)
        {
            serializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter() { NamingStrategy = new CamelCaseNamingStrategy() });
            serializerSettings.Converters.Add(new CultureInfoConverter());
            serializerSettings.Converters.Add(new GuidConverter());
            serializerSettings.NullValueHandling = NullValueHandling.Ignore;
            serializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }
    }
}
