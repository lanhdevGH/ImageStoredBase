using AutoMapper;
using ImageStoreBase.Api.Data;
using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.CollectionDTOs;
using ImageStoreBase.Api.DTOs.FunctionDTOs;
using ImageStoreBase.Api.DTOs.GenericDTO;
using Microsoft.EntityFrameworkCore;

namespace ImageStoreBase.Api.Services.ImplementServices
{

    public class CollectionService : ICollectionService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CollectionService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<CollectionResponseDTO>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Collections.AsQueryable();

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => _mapper.Map<CollectionResponseDTO>(p))
                .ToListAsync();

            return new PagedResult<CollectionResponseDTO>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<List<CollectionResponseDTO>> GetAllAsync()
        {
            return await _context.Collections.Select(p => _mapper.Map<CollectionResponseDTO>(p)).ToListAsync();
        }

        public async Task<CollectionResponseDTO?> GetByIdAsync(string id)
        {
            var entityVal = await _context.Collections.FindAsync(id);
            if (entityVal == null) return null;

            var result = _mapper.Map<CollectionResponseDTO>(entityVal);
            return result;
        }

        public async Task<string> CreateAsync(CollectionCreateRequestDTO entityValCreateDTO)
        {
            var newFunction = _mapper.Map<Collection>(entityValCreateDTO);
            await _context.Collections.AddAsync(newFunction);
            await _context.SaveChangesAsync();
            return newFunction.Id.ToString();
        }

        public async Task<bool> UpdateAsync(string id, CollectionUpdateRequestDTO entityValUpdateDTO)
        {
            var existingFunction = await _context.Collections.FindAsync(id);
            if (existingFunction == null) return false;

            _mapper.Map(entityValUpdateDTO, existingFunction);

            _context.Collections.Update(existingFunction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entityVal = await _context.Collections.FindAsync(id);
            if (entityVal == null) return false;

            _context.Collections.Remove(entityVal);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
