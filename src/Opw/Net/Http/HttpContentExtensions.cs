using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.Http
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsAsync<T>(this HttpContent httpContent, JsonSerializerSettings jsonSerializerSettings)
        {
            var s = await httpContent.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(s, jsonSerializerSettings);
        }
    }
}
