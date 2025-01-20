using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.FunctionDTOs;
using ImageStoreBase.Api.ViewModels;

namespace ImageStoreBase.Api.Services
{
    public interface IFunctionService
    {
        Task<PagedResult<Function>> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Function>> GetAllAsync();
        Task<Function> GetByIdAsync(string id);
        Task<string> CreateAsync(FunctionCreateRequestDTO product);
        Task<bool> UpdateAsync(string id, FunctionUpdateRequestDTO product);
        Task<bool> DeleteAsync(string id);
    }
}
