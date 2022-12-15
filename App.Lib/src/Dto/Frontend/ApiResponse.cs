namespace App.Lib.Dto.Frontend;

public class ApiResponse<T>
{
    public T? Data { get; init; }

    public IList<ApiError> Errors { get; init; }

    public ApiResponse()
    {
        Errors = new List<ApiError>();
    }

    public ApiResponse(T? data) : this()
    {
        Data = data;
    }
}
