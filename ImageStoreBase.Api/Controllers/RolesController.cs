using ImageStoreBase.Api.DTOs.RoleDTOs;
using ImageStoreBase.Api.Services;
using Microsoft.AspNetCore.Mvc;
namespace ImageStoreBase.Api.Controllers
{
    public class RolesController : BaseController
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }

            var pagedResult = await _roleService.GetPagedAsync(pageNumber, pageSize);
            return Ok(pagedResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var role = await _roleService.GetByIdAsync(id);
            if (role == null) return NotFound();
            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleCreateRequestDTO role)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var id = await _roleService.CreateAsync(role);
            return CreatedAtAction(nameof(GetById), new { id }, role);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RoleUpdateRequestDTO role)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _roleService.UpdateAsync(id, role);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _roleService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
