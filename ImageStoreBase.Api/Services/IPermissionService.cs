using ImageStoreBase.Api.DTOs.FunctionDTOs;
using ImageStoreBase.Api.DTOs.PermissionDTO;

namespace ImageStoreBase.Api.Services
{
    public interface IPermissionService
    {
        Task<List<PermissionScreenResponseDTO>> GetCommandViewsAsync();
        Task<List<PermissionVMDTO>> GetPermissionByRole(string roleName);
        Task<string> UpdatePermissionByRole(string roleName, IEnumerable<PermissionVMDTO> permissions);
        Task<List<FunctionResponseDTO>> GetPermissionByUser(string userId);
    }
}
