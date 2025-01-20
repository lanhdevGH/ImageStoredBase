using ImageStoreBase.Api.DTOs.FunctionDTOs;
using ImageStoreBase.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageStoreBase.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class FunctionsController : ControllerBase
    {
        private readonly IFunctionService _functionService;

        public FunctionsController(IFunctionService functionService)
        {
            _functionService = functionService;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }

            var pagedResult = await _functionService.GetPagedAsync(pageNumber, pageSize);
            return Ok(pagedResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var functions = await _functionService.GetAllAsync();
            return Ok(functions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var function = await _functionService.GetByIdAsync(id);
            if (function == null) return NotFound();
            return Ok(function);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FunctionCreateRequestDTO function)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var id = await _functionService.CreateAsync(function);
            return CreatedAtAction(nameof(GetById), new { id }, function);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] FunctionUpdateRequestDTO function)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _functionService.UpdateAsync(id, function);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _functionService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
