using AutoMapper;
using ImageStoreBase.Api.Data;
using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.CommandDTO;
using ImageStoreBase.Api.DTOs.FunctionDTOs;
using ImageStoreBase.Api.DTOs.GenericDTO;
using Microsoft.EntityFrameworkCore;

namespace ImageStoreBase.Api.Services.ImplementServices
{

    public class FunctionService : IFunctionService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FunctionService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<FunctionResponseDTO>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Functions.AsQueryable();

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => _mapper.Map<FunctionResponseDTO>(p))
                .ToListAsync();

            return new PagedResult<FunctionResponseDTO>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<IEnumerable<FunctionResponseDTO>> GetAllAsync()
        {
            return await _context.Functions.Select(p => _mapper.Map<FunctionResponseDTO>(p)).ToListAsync();
        }

        public async Task<FunctionResponseDTO> GetByIdAsync(string id)
        {
            var entityVal = await _context.Functions.FindAsync(id);
            if (entityVal == null)
            {
                throw new KeyNotFoundException("Function id not found");
            }

            var result = _mapper.Map<FunctionResponseDTO>(entityVal);
            return result;
        }

        public async Task<string> CreateAsync(FunctionCreateRequestDTO entityValCreateDTO)
        {
            var newFunction = _mapper.Map<Function>(entityValCreateDTO);
            await _context.Functions.AddAsync(newFunction);
            await _context.SaveChangesAsync();
            return newFunction.Id;
        }

        public async Task<bool> UpdateAsync(string id, FunctionUpdateRequestDTO entityValUpdateDTO)
        {
            var existingFunction = await _context.Functions.FindAsync(id);
            if (existingFunction == null) return false;

            _mapper.Map(entityValUpdateDTO, existingFunction);

            _context.Functions.Update(existingFunction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entityVal = await _context.Functions.FindAsync(id);
            if (entityVal == null) return false;

            _context.Functions.Remove(entityVal);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CommandResponseDTO>> GetCommandInFunction(string funcId)
        {
            var result = new List<CommandResponseDTO>();
            // Lấy thông tin function
            var function = await _context.Functions
                .Include(p => p.CommandInFunctions)
                .ThenInclude(c => c.Command)
                .FirstOrDefaultAsync(p => p.Id == funcId);

            if (function == null)
            {
                throw new KeyNotFoundException("Function id not found");
            }

            // Lấy danh sách command của function
            var commands = function.CommandInFunctions.Select(p => new CommandResponseDTO
            {
                Id = p.Command.Id,
                Name = p.Command.Name,
                Description = p.Command.Description,
                CreatedAt = p.Command.CreatedAt,
                UpdatedAt = p.Command.UpdatedAt
            }).ToList();
            if (commands != null)
            {
                result.AddRange(commands);
            }

            return result;
        }

        public async Task<string> AddCommandsToFunction(string functionId, IEnumerable<string> listCommandIds)
        {
            var functionCommands = listCommandIds.Select(commandId => new CommandInFunction
            {
                FunctionId = functionId,
                CommandId = commandId
            }).ToList();

            _context.CommandInFunctions.AddRange(functionCommands);
            await _context.SaveChangesAsync();

            return functionId;
        }

        public async Task<bool> RemoveCommandInFunction(string functionId, string listCommandIds)
        {
            _context.CommandInFunctions.Where(p => p.FunctionId == functionId && p.CommandId == listCommandIds)
                .ToList().ForEach(p => _context.CommandInFunctions.Remove(p));
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
