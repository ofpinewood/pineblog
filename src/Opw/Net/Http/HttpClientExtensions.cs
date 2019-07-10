using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.Http
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient client, string requestUri, T value, JsonSerializerSettings jsonSerializerSettings)
        {
            var json = JsonConvert.SerializeObject(value, jsonSerializerSettings);
            var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            return client.PostAsync(requestUri, jsonContent);
        }
    }
}
