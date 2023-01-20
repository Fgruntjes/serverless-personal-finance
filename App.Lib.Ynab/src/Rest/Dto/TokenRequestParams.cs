using Newtonsoft.Json;

namespace App.Lib.Ynab.Rest.Dto;

public class TokenRequestParams
{
    [JsonProperty("client_id")]
    public string ClientId { get; set; }

    [JsonProperty("client_secret")]
    public string ClientSecret { get; set; }

    [JsonProperty("redirect_uri")]
    public string RedirectUri { get; set; }

    [JsonProperty("grant_type")]
    public string GrantType { get; set; }

    [JsonProperty("code")]
    public string? Code { get; set; }

    [JsonProperty("refresh_token")]
    public string? RefreshToken { get; set; }

}