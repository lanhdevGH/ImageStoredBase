using ImageStoreBase.Api.DTOs.UserDTOs;
using ImageStoreBase.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageStoreBase.Api.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("Page number and page size must be greaBter than zero.");
            }

            var pagedResult = await _userService.GetPagedAsync(pageNumber, pageSize);
            return Ok(pagedResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateRequestDTO user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var id = await _userService.CreateAsync(user);
            return CreatedAtAction(nameof(GetById), new { id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateRequestDTO user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userService.UpdateAsync(id, user);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("change-password/{userId}")]
        public async Task<IActionResult> ChangePassword(string userId,
                                                        [FromBody] ChangePasswordRequestDTO changePasswordRequest)
        {
            var a = userId;
            var result = await _userService.ChangePasswordAsync(userId,
                                                                changePasswordRequest.CurrentPassword,
                                                                changePasswordRequest.NewPassword);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}