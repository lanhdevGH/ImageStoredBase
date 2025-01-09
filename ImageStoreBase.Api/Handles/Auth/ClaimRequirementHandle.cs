using ImageStoreBase.Api.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ImageStoreBase.Api.Handles.Auth
{
    public class ClaimRequirementHandle : AuthorizationHandler<ClaimRequirement>
    {
        private readonly AppDbContext _dbContext;

        public ClaimRequirementHandle(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                return Task.CompletedTask;
            }

            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRoles = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var hasPermission = _dbContext.Permissions
                .Any(rp =>
                    userRoles.Contains(rp.RoleName.ToString()) && // Convert Guid to string
                    rp.FunctionId == requirement.FunctionCode &&
                    rp.CommandId == requirement.ActionCode);

            if (hasPermission)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class ClaimRequirement : IAuthorizationRequirement
    {
        public string FunctionCode { get; }
        public string ActionCode { get; }

        public ClaimRequirement(string functionCode, string actionCode)
        {
            FunctionCode = functionCode;
            ActionCode = actionCode;
        }
    }
}
