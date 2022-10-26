namespace App.Lib.Dto.Frontend;

public class ApiError
{
    public ErrorType Type { get; init; }
    public string Message { get; init; }
    public IDictionary<string, string> Context { get; init; }

    public ApiError(ErrorType type, string message, IDictionary<string, string> context)
    {
        Type = type;
        Message = message;
        Context = context;
    }

    public ApiError(ErrorType integration, string message)
        : this(integration, message, new Dictionary<string, string>())
    { }

    public ApiError()
        : this(ErrorType.Internal, "Internal server error", new Dictionary<string, string>())
    { }
}