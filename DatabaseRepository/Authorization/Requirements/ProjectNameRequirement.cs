using Microsoft.AspNetCore.Authorization;

namespace DatabaseRepository.Authorization.Requirements
{
    public class ProjectNameRequirement : IAuthorizationRequirement
    {
        public string ProjectName { get; set; }

        public ProjectNameRequirement(string projectName)
        {
            ProjectName = projectName;
        }
    }
}
