using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace App.Lib.Authorization;

public class AuthContext : IAuthContext
{
    public const string HeaderTenant = "x-app-tenant";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public Guid CurrentTenant
    {
        get
        {
            Debug.Assert(_httpContextAccessor.HttpContext != null, "_httpContextAccessor.HttpContext != null");

            // TODO implement check to see if tenant is within the claim
            var value = _httpContextAccessor.HttpContext.Request.Headers[HeaderTenant];
            if (value.IsNullOrEmpty())
            {
                throw new BadHttpRequestException("Missing `x-app-tenant` header.");
            }

            return Guid.Parse(value!);
        }
    }

    public AuthContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}