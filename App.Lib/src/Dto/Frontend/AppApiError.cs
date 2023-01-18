namespace App.Lib.Dto.Frontend;

public class AppApiError
{
    public ErrorType Type { get; init; }
    public string Message { get; init; }
    public IDictionary<string, string> Context { get; init; }

    public AppApiError(ErrorType type, string message, IDictionary<string, string> context)
    {
        Type = type;
        Message = message;
        Context = context;
    }

    public AppApiError(ErrorType integration, string message)
        : this(integration, message, new Dictionary<string, string>())
    { }

    public AppApiError()
        : this(ErrorType.Internal, "Internal server error", new Dictionary<string, string>())
    { }
}