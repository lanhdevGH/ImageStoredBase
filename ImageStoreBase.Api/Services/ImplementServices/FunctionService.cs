using AutoMapper;
using ImageStoreBase.Api.Data;
using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.FunctionDTOs;
using ImageStoreBase.Api.ViewModels;
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

        public async Task<PagedResult<Function>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Functions.AsQueryable();

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Function>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<IEnumerable<Function>> GetAllAsync()
        {
            return await _context.Functions.ToListAsync();
        }

        public async Task<Function> GetByIdAsync(string id)
        {
            var entityVal = await _context.Functions.FindAsync(id);
            return entityVal ?? throw new KeyNotFoundException("Function id not found");
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
    }
}
