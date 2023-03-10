
namespace App.Lib.Authorization;

public class AuthContextMock : IAuthContext
{
    public Guid CurrentTenant { get; }

    public AuthContextMock() : this(new Guid("98956dc5-215c-4795-b615-aa204c9a1644"))
    {
    }

    public AuthContextMock(Guid guid)
    {
        CurrentTenant = guid;
    }
}