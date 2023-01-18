using Newtonsoft.Json;

namespace App.Lib.Tests;

public static class HttpContentExtensions
{
    public static async Task<T?> ReadFromJsonAsync<T>(
        this HttpContent content,
        JsonSerializerSettings? settings = null,
        CancellationToken cancellationToken = default)
    {
        var json = await content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<T>(json, settings);
    }
}