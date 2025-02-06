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

        public async Task<PagedResult<Command>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Commands.AsQueryable();

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Command>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<IEnumerable<Command>> GetAllAsync()
        {
            return await _context.Commands.ToListAsync();
        }

        public async Task<Command> GetByIdAsync(string id)
        {
            var entityVal = await _context.Commands.FindAsync(id);
            return entityVal ?? throw new KeyNotFoundException("Command id not found");
        }

        public async Task<string> CreateAsync(CommandCreateRequestDTO entityValCreateDTO)
        {
            var newCommand = _mapper.Map<Command>(entityValCreateDTO);
            await _context.Commands.AddAsync(newCommand);
            await _context.SaveChangesAsync();
            return newCommand.Id;
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
