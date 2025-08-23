using DatabaseRepository.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace DatabaseRepository.Authorization.Handlers
{
    public class ProjectNameHandler : AuthorizationHandler<ProjectNameRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProjectNameRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == requirement.ProjectName))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
