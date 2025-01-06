using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ImageStoreBase.Api.Services;
using ImageStoreBase.Api.DTOs.Roles;

namespace ImageStoreBase.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentityRole>>> GetAllRoles()
        {
            try 
            {
                var roles = await _roleService.GetAllRolesAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi nội bộ: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IdentityRole>> GetRoleById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("ID role không được để trống");
                }

                var role = await _roleService.GetRoleByIdAsync(id);
                if (role == null)
                {
                    return NotFound($"Không tìm thấy role với id: {id}");
                }
                return Ok(role);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi nội bộ: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roleName))
                {
                    return BadRequest("Tên role không được để trống");
                }

                var role = await _roleService.CreateRoleAsync(roleName);
                return CreatedAtAction(nameof(GetRoleById), new { id = role.Id }, role);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict($"Role đã tồn tại: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi nội bộ: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("ID role không được để trống");
                }

                await _roleService.DeleteRoleAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound($"Không tìm thấy role: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi nội bộ: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(Guid id,string roleName)
        {
            try
            {
                var updatedRole = await _roleService.UpdateRoleAsync(id, roleName);
                return Ok(updatedRole);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound($"Không tìm thấy role: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict($"Tên role đã tồn tại: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi nội bộ: {ex.Message}");
            }
        }
    }
}
