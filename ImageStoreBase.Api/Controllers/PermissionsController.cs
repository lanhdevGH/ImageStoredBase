using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.PermissionDTO;
using ImageStoreBase.Api.Filters;
using ImageStoreBase.Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ImageStoreBase.Api.Controllers
{
    public class PermissionsController : BaseController
    {
        private readonly IPermissionService _permissionService;

        public PermissionsController(IPermissionService permissionService, UserManager<User> userManager)
        {
            _permissionService = permissionService;
        }

        [HttpGet("command-view")]
        public async Task<IActionResult> GetCommandViews()
        {
            var result = await _permissionService.GetCommandViewsAsync();
            return Ok(result);
        }

        [HttpGet("get-by-role/{roleName}")]
        public async Task<IActionResult> GetPermissionByRole([FromQuery] string roleName)
        {
            var result = await _permissionService.GetPermissionByRole(roleName);
            return Ok(result);
        }

        [HttpPut("update-by-role/{roleName}")]
        public async Task<IActionResult> PutPermissionByRole(string roleName, [FromBody] IEnumerable<PermissionVMDTO> permissions)
        {
            var result = await _permissionService.GetPermissionByRole(roleName);
            return Ok(result);
        }

        [HttpGet("get-by-user/{userid}")]
        [ValidateEntityExistsFilter<User>("userid")]
        public async Task<IActionResult> GetPermissionByUser(string userid)
        {
            var result = await _permissionService.GetPermissionByRole(userid);
            return Ok(result);
        }
    }
}
