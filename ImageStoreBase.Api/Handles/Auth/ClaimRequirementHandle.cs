using ImageStoreBase.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ImageStoreBase.Api.Handles.Auth
{
    public class ClaimRequirementHandle : AuthorizationHandler<ClaimRequirement>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ClaimRequirementHandle(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                return Task.CompletedTask;
            }

            var expirationClaim = context.User.Claims.FirstOrDefault(c => c.Type == "exp");
            if (expirationClaim == null || !long.TryParse(expirationClaim.Value, out var expTimestamp))
            {
                return Task.CompletedTask; // Token không hợp lệ
            }

            var expirationDate = DateTimeOffset.FromUnixTimeSeconds(expTimestamp).UtcDateTime;
            if (DateTime.UtcNow > expirationDate)
            {
                return Task.CompletedTask; // Token đã hết hạn
            }

            var userRoles = context.User.Claims
                .Where(c => string.Equals(c.Type, nameof(ClaimTypes.Role), StringComparison.OrdinalIgnoreCase))
                .Select(c => c.Value).ToList();

            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            AppDbContext _appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();


            var hasPermission = _appDbContext.Permissions
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
