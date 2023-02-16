using App.Lib.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace App.Lib.Tests.Authorization;

public class TestAuthorizationHandler : AuthorizationHandler<HasScopeRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        HasScopeRequirement requirement
    )
    {
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}