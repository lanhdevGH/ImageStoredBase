using ImageStoreBase.Api.DTOs.CommandDTO;
using ImageStoreBase.Api.DTOs.FunctionDTOs;
using ImageStoreBase.Api.DTOs.GenericDTO;

namespace ImageStoreBase.Api.Services
{
    public interface IFunctionService
    {
        Task<PagedResult<FunctionResponseDTO>> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<FunctionResponseDTO>> GetAllAsync();
        Task<FunctionResponseDTO> GetByIdAsync(string id);
        Task<string> CreateAsync(FunctionCreateRequestDTO function);
        Task<bool> UpdateAsync(string id, FunctionUpdateRequestDTO function);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<CommandResponseDTO>> GetCommandInFunction(string funcId);
        Task<string> AddCommandsToFunction(string functionId, IEnumerable<string> listCommandIds);
        Task<bool> RemoveCommandInFunction(string functionId, string listCommandIds);
    }
}
