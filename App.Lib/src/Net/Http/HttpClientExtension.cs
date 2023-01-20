using System.Text;
using Newtonsoft.Json;

namespace App.Lib.Net.Http;

public static class HttpClientExtension
{
    public static async Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient client, string requestUrl, T theObj)
    {
        var stringContent = new StringContent(
            JsonConvert.SerializeObject(theObj, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            }),
            Encoding.UTF8,
            "application/json");
        return await client.PostAsync(requestUrl, stringContent);
    }

    public static async Task<T> ReadFromJsonAsync<T>(this HttpContent httpContent)
    {
        var stringContent = await httpContent.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(stringContent);
    }
}