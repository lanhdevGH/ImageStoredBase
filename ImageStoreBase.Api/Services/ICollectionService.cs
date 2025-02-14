using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.CollectionDTOs;
using ImageStoreBase.Api.DTOs.GenericDTO;

namespace ImageStoreBase.Api.Services
{

    public interface ICollectionService
    {
        Task<PagedResult<CollectionResponseDTO>> GetPagedAsync(int pageNumber, int pageSize);
        Task<List<CollectionResponseDTO>> GetAllAsync();
        Task<CollectionResponseDTO?> GetByIdAsync(string id);
        Task<string> CreateAsync(CollectionCreateRequestDTO product);
        Task<bool> UpdateAsync(string id, CollectionUpdateRequestDTO product);
        Task<bool> DeleteAsync(string id);
    }
}
