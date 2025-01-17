using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.CollectionDTOs;
using ImageStoreBase.Api.ViewModels;

namespace ImageStoreBase.Api.Services
{

    public interface ICollectionService
    {
        Task<PagedResult<Collection>> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Collection>> GetAllAsync();
        Task<Collection> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(CollectionCreateRequestDTO product);
        Task<bool> UpdateAsync(Guid id, CollectionUpdateRequestDTO product);
        Task<bool> DeleteAsync(Guid id);
    }
}
