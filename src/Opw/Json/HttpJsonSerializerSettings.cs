using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Opw.Json
{
    /// <summary>
    /// Serialization settings for reading/writing http request/response content
    /// </summary>
    public class HttpJsonSerializerSettings : JsonSerializerSettings
    {
        public HttpJsonSerializerSettings()
        {
            this.AddDefaultJsonSerializerSettings();
        }
    }
}
