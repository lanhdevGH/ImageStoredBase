﻿using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.GenericDTO;
using ImageStoreBase.Api.DTOs.RoleDTOs;

namespace ImageStoreBase.Api.Services
{
    public interface IRoleService
    {
        Task<PagedResult<Role>> GetPagedAsync(int pageNumber, int pageSize);
        Task<List<Role>> GetAllAsync();
        Task<Role?> GetByIdAsync(string id);
        Task<string> CreateAsync(RoleCreateRequestDTO roleCreateDTO);
        Task<bool> AddUserToRoleAsync(string userId, string roleName);
        Task<bool> RemoveUserFromRoleAsync(string userId, string roleName);
        Task<List<string>> GetUserRolesAsync(string userId);
        Task<bool> UpdateAsync(string id, RoleUpdateRequestDTO roleUpdateDTO);
        Task<bool> DeleteAsync(string id);
    }
}
