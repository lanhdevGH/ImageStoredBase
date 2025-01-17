using AutoMapper;
using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.RoleDTOs;
using ImageStoreBase.Api.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ImageStoreBase.Api.Services.ImplementServices
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public RoleService(RoleManager<Role> roleManager, UserManager<User> userManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<PagedResult<Role>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _roleManager.Roles.AsQueryable();

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(p => p.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Role>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<Role> GetByIdAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            return role ?? throw new KeyNotFoundException("Role id not found");
        }

        public async Task<Guid> CreateAsync(RoleCreateRequestDTO roleCreateDTO)
        {
            var newRole = _mapper.Map<Role>(roleCreateDTO);
            newRole.Id = Guid.NewGuid();
            await _roleManager.CreateAsync(newRole);
            return newRole.Id;
        }

        // Gán vai trò cho người dùng
        public async Task<bool> AddUserToRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                throw new KeyNotFoundException("Role not found.");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        // Xóa vai trò của người dùng
        public async Task<bool> RemoveUserFromRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }

        // Lấy danh sách vai trò của người dùng
        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> UpdateAsync(Guid id, RoleUpdateRequestDTO roleUpdateDTO)
        {
            var existingRole = await _roleManager.FindByIdAsync(id.ToString());
            if (existingRole == null) return false;

            _mapper.Map(roleUpdateDTO, existingRole);

            await _roleManager.UpdateAsync(existingRole);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return false;

            await _roleManager.DeleteAsync(role);
            return true;
        }
    }
}
