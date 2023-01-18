namespace App.Lib.Dto.Frontend;

public class ApiResponse
{
    public IList<AppApiError> Errors { get; init; }

    public ApiResponse()
    {
        Errors = new List<AppApiError>();
    }
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; init; }

    public ApiResponse()
    {
    }

    public ApiResponse(T? data)
    {
        Data = data;
    }
}