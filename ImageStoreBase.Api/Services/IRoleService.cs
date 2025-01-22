using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.GenericDTO;
using ImageStoreBase.Api.DTOs.RoleDTOs;

namespace ImageStoreBase.Api.Services
{
    public interface IRoleService
    {
        Task<PagedResult<Role>> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(RoleCreateRequestDTO roleCreateDTO);
        Task<bool> AddUserToRoleAsync(string userId, string roleName);
        Task<bool> RemoveUserFromRoleAsync(string userId, string roleName);
        Task<IList<string>> GetUserRolesAsync(string userId);
        Task<bool> UpdateAsync(Guid id, RoleUpdateRequestDTO roleUpdateDTO);
        Task<bool> DeleteAsync(Guid id);
    }
}
