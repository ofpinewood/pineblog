using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Opw.AspNetCore.Testing.Net.Http
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsAsync<T>(this HttpContent content)
        {
            var str = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(str);
        }

        public static StringContent ToJsonContent(this object obj)
        {
            var str = JsonConvert.SerializeObject(obj);
            return new StringContent(str, Encoding.UTF8, "application/json");
        }
    }
}
