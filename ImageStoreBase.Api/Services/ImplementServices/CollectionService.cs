using AutoMapper;
using ImageStoreBase.Api.Data;
using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.CollectionDTOs;
using ImageStoreBase.Api.ViewModels;
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

        public async Task<PagedResult<Collection>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Collections.AsQueryable();

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Collection>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<IEnumerable<Collection>> GetAllAsync()
        {
            return await _context.Collections.ToListAsync();
        }

        public async Task<Collection> GetByIdAsync(Guid id)
        {
            var collection = await _context.Collections.FindAsync(id);
            return collection ?? throw new KeyNotFoundException("Collection not found");
        }

        public async Task<Guid> CreateAsync(CollectionCreateRequestDTO collectionCreateDTO)
        {
            var newCollection = _mapper.Map<Collection>(collectionCreateDTO);
            newCollection.Id = Guid.NewGuid();
            await _context.Collections.AddAsync(newCollection);
            await _context.SaveChangesAsync();
            return newCollection.Id;
        }

        public async Task<bool> UpdateAsync(Guid id, CollectionUpdateRequestDTO collectionUpdateDTO)
        {
            var existingCollection = await _context.Collections.FindAsync(id);
            if (existingCollection == null) return false;

            _mapper.Map(collectionUpdateDTO, existingCollection);

            _context.Collections.Update(existingCollection);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var collection = await _context.Collections.FindAsync(id);
            if (collection == null) return false;

            _context.Collections.Remove(collection);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
