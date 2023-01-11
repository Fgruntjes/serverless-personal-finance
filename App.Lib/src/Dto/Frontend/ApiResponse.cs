namespace App.Lib.Dto.Frontend;

public class ApiResponse<T>
{
    public T? Data { get; init; }

    public IList<AppApiError> Errors { get; init; }

    public ApiResponse()
    {
        Errors = new List<AppApiError>();
    }

    public ApiResponse(T? data) : this()
    {
        Data = data;
    }
}
