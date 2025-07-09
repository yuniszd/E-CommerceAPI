﻿using Microsoft.AspNetCore.Authorization;

namespace E_CommerceAPI.Infrastructure.Authorization;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.HasClaim(c => c.Type == "permission" && c.Value == requirement.Permission))
        {
            context.Succeed(requirement);
        }
        else
        {
          //reject
        }

        return Task.CompletedTask;
    }
}
