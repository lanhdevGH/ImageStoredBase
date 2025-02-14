using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.CollectionDTOs;
using ImageStoreBase.Api.DTOs.GenericDTO;

namespace ImageStoreBase.Api.Services
{

    public interface ICollectionService
    {
        Task<PagedResult<Collection>> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Collection>> GetAllAsync();
        Task<Collection> GetByIdAsync(string id);
        Task<string> CreateAsync(CollectionCreateRequestDTO product);
        Task<bool> UpdateAsync(string id, CollectionUpdateRequestDTO product);
        Task<bool> DeleteAsync(string id);
    }
}
