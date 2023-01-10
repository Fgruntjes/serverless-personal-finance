using System.Text;
using Newtonsoft.Json;

namespace App.Lib.Net.Http;

public static class HttpClientExtension
{
    public static async Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient client, string requestUrl, T theObj)
    {
        var stringContent = new StringContent(
            JsonConvert.SerializeObject(theObj),
            Encoding.UTF8,
            "application/json");
        return await client.PostAsync(requestUrl, stringContent);
    }

    public static async Task<T> ReadFromJsonAsync<T>(this HttpContent httpContent)
    {
        return JsonConvert.DeserializeObject<T>(await httpContent.ReadAsStringAsync());
    }
}