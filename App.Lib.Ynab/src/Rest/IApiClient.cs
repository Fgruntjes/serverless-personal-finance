using App.Lib.Ynab.Rest.Dto;
using Refit;

namespace App.Lib.Ynab.Rest;

public interface IApiClient
{
    [Get("/user")]
    Task<Dto.ApiResponse<UserData>> GetUser();
}