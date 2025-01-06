using AutoMapper;
using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ImageStoreBase.Api.Services
{
    public class RoleService
    {
        private readonly IMapper _mapper;
        private readonly RoleManager<Role> _roleManager;

        public RoleService(RoleManager<Role> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<Role> CreateRoleAsync(string roleName)
        {
            var role = new Role { Name = roleName };
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                throw new Exception($"Không thể tạo role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            return role;
        }

        public async Task<RoleResponse?> GetRoleByIdAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            return _mapper.Map<RoleResponse>(role);
        }

        public async Task<List<RoleResponse>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return _mapper.Map<List<RoleResponse>>(roles);
        }

        public async Task<Role> UpdateRoleAsync(Guid id, string name)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                throw new Exception($"Không tìm thấy role với Id: {id}");
            }
            role.Name = name;
            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                throw new Exception($"Không thể cập nhật role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            return role;
        }

        public async Task DeleteRoleAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    throw new Exception($"Không thể xóa role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
