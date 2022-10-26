using Refit;

namespace App.Lib.Ynab;

public interface IApiClient : YNAB.Rest.IApiClient
{
    [Get("/user")]
    Task<YNAB.Rest.ApiResponse<UserData>> GetUser();
}