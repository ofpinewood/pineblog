using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Opw.Json
{
    /// <summary>
    /// Serialization settings for reading/writing json with type info
    /// </summary>
    public class TypeInfoJsonSerializerSettings : JsonSerializerSettings
    {
        public TypeInfoJsonSerializerSettings()
        {
            // dont use the ContractResolver since it will also transform dictionary keys,
            // and in this case those are ids which are case sensitive
            this.AddDefaultJsonSerializerSettings();
            TypeNameHandling = TypeNameHandling.All;
        }
    }
}
