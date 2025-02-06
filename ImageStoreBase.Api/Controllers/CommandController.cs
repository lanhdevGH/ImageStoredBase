using ImageStoreBase.Api.DTOs.CommandDTO;
using ImageStoreBase.Api.Filters;
using ImageStoreBase.Api.FluentValidator;
using ImageStoreBase.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageStoreBase.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandService _commandService;

        public CommandsController(ICommandService commandService)
        {
            _commandService = commandService;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }

            var pagedResult = await _commandService.GetPagedAsync(pageNumber, pageSize);
            return Ok(pagedResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var commands = await _commandService.GetAllAsync();
            return Ok(commands);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var command = await _commandService.GetByIdAsync(id);
            if (command == null) return NotFound();
            return Ok(command);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationEntityFilter<CommandCreateRequestDTO, CommandCreateRequestDTOValidator>))]
        public async Task<IActionResult> Create([FromBody] CommandCreateRequestDTO entity)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var id = await _commandService.CreateAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id }, entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] CommandUpdateRequestDTO entity)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _commandService.UpdateAsync(id, entity);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _commandService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
