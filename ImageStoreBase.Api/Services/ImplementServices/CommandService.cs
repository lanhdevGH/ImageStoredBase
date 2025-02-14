using AutoMapper;
using ImageStoreBase.Api.Data;
using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.CommandDTO;
using ImageStoreBase.Api.DTOs.GenericDTO;
using Microsoft.EntityFrameworkCore;

namespace ImageStoreBase.Api.Services.ImplementServices
{

    public class CommandService : ICommandService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CommandService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<CommandResponseDTO>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Commands.AsQueryable();

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => _mapper.Map<CommandResponseDTO>(p))
                .ToListAsync();

            return new PagedResult<CommandResponseDTO>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<List<CommandResponseDTO>> GetAllAsync()
        {
            return await _context.Commands.Select(p => _mapper.Map<CommandResponseDTO>(p)).ToListAsync();
        }

        public async Task<CommandResponseDTO?> GetByIdAsync(string id)
        {
            var entityVal = await _context.Commands.FindAsync(id);
            if (entityVal == null) return null;

            var result = _mapper.Map<CommandResponseDTO>(entityVal);
            return result;
        }

        public async Task<string> CreateAsync(CommandCreateRequestDTO entityValCreateDTO)
        {
            var newCommand = _mapper.Map<Command>(entityValCreateDTO);
            await _context.Commands.AddAsync(newCommand);
            await _context.SaveChangesAsync();
            return newCommand.Id.ToString();
        }

        public async Task<bool> UpdateAsync(string id, CommandUpdateRequestDTO entityValUpdateDTO)
        {
            var existingCommand = await _context.Commands.FindAsync(id);
            if (existingCommand == null) return false;

            _mapper.Map(entityValUpdateDTO, existingCommand);

            _context.Commands.Update(existingCommand);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entityVal = await _context.Commands.FindAsync(id);
            if (entityVal == null) return false;

            _context.Commands.Remove(entityVal);
            await _context.SaveChangesAsync();
            return true;
        }
    }   
}
