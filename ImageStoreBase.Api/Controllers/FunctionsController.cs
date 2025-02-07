using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.FunctionDTOs;
using ImageStoreBase.Api.Filters;
using ImageStoreBase.Api.FluentValidator;
using ImageStoreBase.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageStoreBase.Api.Controllers
{
    public class FunctionsController : BaseController
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
        [FluentValidationEntityFilter<FunctionCreateRequestDTO,FunctionCreateRequestDTOValidator>("entity")]
        [ValidateEntityNotExistsFilter<Function, string>("entity")]
        public async Task<IActionResult> Create([FromBody] FunctionCreateRequestDTO entity)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var id = await _functionService.CreateAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id }, entity);
        }

        [HttpPut("{id}")]
        [ValidateEntityExistsFilter<Function, string>("id")]
        public async Task<IActionResult> Update(string id, [FromBody] FunctionUpdateRequestDTO entity)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _functionService.UpdateAsync(id, entity);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ValidateEntityExistsFilter<Function, string>("id")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _functionService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("{funcId}/commands")]
        public async Task<IActionResult> GetCommandInFunction(string funcId)
        {
            var commands = await _functionService.GetCommandInFunction(funcId);
            return Ok(commands);
        }

        [HttpPost("{funcId}/add-commands")]
        [ValidateEntityExistsFilter<Function, string>("funcId")]
        [ValidateEntityNotExistsFilter<CommandInFunction, string>("commandIds", nameof(CommandInFunction.CommandId))]
        public async Task<IActionResult> AddCommands(string funcId, [FromBody] IEnumerable<string> commandIds)
        {
            if (commandIds == null || !commandIds.Any())
                return BadRequest("ListCommandIds không được để trống.");
            var result = await _functionService.AddCommandsToFunction(funcId, commandIds);
            return NoContent();
        }

        [HttpDelete("{funcId}/remove-command/{commandId}")]
        [ValidateEntityExistsFilter<Function, string>("funcId")]
        public async Task<IActionResult> RemoveCommand(string funcId, string commandId)
        {
            var result = await _functionService.RemoveCommandInFunction(funcId, commandId);
            return NoContent();
        }
    }
}
