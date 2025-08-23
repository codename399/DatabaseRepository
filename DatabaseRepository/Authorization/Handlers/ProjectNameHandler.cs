using DatabaseRepository.Authorization.Requirements;
using DatabaseRepository.Constants;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DatabaseRepository.Authorization.Handlers
{
    public class ProjectNameHandler : AuthorizationHandler<ProjectNameRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProjectNameRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Role))
            {
                return Task.CompletedTask;
            }

            if (!context.User.HasClaim(c => c.Type == requirement.ProjectName))
            {
                return Task.CompletedTask;
            }

            string role = context.User.FindFirst(c => c.Type == ClaimTypes.Role)?.Value ?? string.Empty;
            string isAdmin = context.User.FindFirst(c => c.Type == requirement.ProjectName)?.Value.ToLower() ?? "false";

            if (Convert.ToBoolean(isAdmin) == false || role == BaseConstant.Admin)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
