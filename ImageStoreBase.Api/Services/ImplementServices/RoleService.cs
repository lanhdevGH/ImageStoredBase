﻿using AutoMapper;
using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.GenericDTO;
using ImageStoreBase.Api.DTOs.RoleDTOs;
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

        public async Task<List<Role>> GetAllAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<Role?> GetByIdAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            return role;
        }

        public async Task<string> CreateAsync(RoleCreateRequestDTO roleCreateDTO)
        {
            var newRole = _mapper.Map<Role>(roleCreateDTO);
            newRole.Id = Guid.NewGuid();
            await _roleManager.CreateAsync(newRole);
            return newRole.Id.ToString();
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
        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var result = new List<string>();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return result;
            result.AddRange(await _userManager.GetRolesAsync(user));
            return result;
        }

        public async Task<bool> UpdateAsync(string id, RoleUpdateRequestDTO roleUpdateDTO)
        {
            var existingRole = await _roleManager.FindByIdAsync(id);
            if (existingRole == null) return false;

            _mapper.Map(roleUpdateDTO, existingRole);

            await _roleManager.UpdateAsync(existingRole);
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return false;

            await _roleManager.DeleteAsync(role);
            return true;
        }
    }
}
