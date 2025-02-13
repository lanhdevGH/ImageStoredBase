using ImageStoreBase.Api.DTOs.FunctionDTOs;
using ImageStoreBase.Api.DTOs.PermissionDTO;

namespace ImageStoreBase.Api.Services
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionScreenResponseDTO>> GetCommandViewsAsync();
        Task<IEnumerable<PermissionVMDTO>> GetPermissionByRole(string roleName);
        Task<string> UpdatePermissionByRole(string roleName, IEnumerable<PermissionVMDTO> permissions);
        Task<IEnumerable<FunctionResponseDTO>> GetPermissionByUser(string userId);
    }
}
