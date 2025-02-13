using AutoMapper;
using ImageStoreBase.Api.Data;
using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.FunctionDTOs;
using ImageStoreBase.Api.DTOs.PermissionDTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ImageStoreBase.Api.Services.ImplementServices
{
    public class PermissionService : IPermissionService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public PermissionService(AppDbContext context, IMapper mapper, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IEnumerable<PermissionScreenResponseDTO>> GetCommandViewsAsync()
        {
            // Bước 1: Lấy danh sách các command ID từ database
            var commandList = await _context.Commands
                .Select(c => c.Id)
                .ToListAsync(); // Lấy danh sách tất cả CommandId

            // Bước 2: Lấy dữ liệu từ database nhưng không dùng ToDictionary trong query
            var rawData = await _context.Functions
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    ParentId = x.ParentId ?? string.Empty,
                    CommandCounts = x.CommandInFunctions
                        .GroupBy(cif => cif.CommandId)
                        .Select(g => new { CommandId = g.Key, Count = g.Count() })
                        .ToList()
                })
                .OrderBy(x => x.Id)
                .ToListAsync(); // Lấy dữ liệu từ DB

            // Bước 3: Xử lý dữ liệu trên bộ nhớ (chuyển sang Dictionary)
            var result = rawData.Select(x => new PermissionScreenResponseDTO
            {
                Id = x.Id,
                Name = x.Name,
                ParentId = x.ParentId,
                Commands = commandList.ToDictionary(
                    cmdId => cmdId, // Key của Dictionary
                    cmdId => x.CommandCounts.FirstOrDefault(c => c.CommandId == cmdId)?.Count ?? 0 // Giá trị (số lần xuất hiện)
                )
            });

            return result;
        }

        public async Task<IEnumerable<PermissionVMDTO>> GetPermissionByRole(string roleName)
        {
            return await _context.Permissions
                .Where(x => x.RoleName == roleName)
                .Select(x => new PermissionVMDTO
                {
                    RoleName = x.RoleName,
                    FunctionId = x.FunctionId,
                    CommandId = x.CommandId
                }).ToListAsync();
        }

        public async Task<IEnumerable<FunctionResponseDTO>> GetPermissionByUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User is not found", nameof(userId));
            }

            var roles = await _userManager.GetRolesAsync(user);
            var data = await _context.Permissions
                        .Where(x => roles.Contains(x.RoleName) && x.CommandId == "VIEW")
                        .Select(x => _mapper.Map<FunctionResponseDTO>(x.Function))
                        .ToListAsync();
            return data;
        }

        public async Task<string> UpdatePermissionByRole(string roleName, IEnumerable<PermissionVMDTO> permissions)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Role name cannot be null or empty.", nameof(roleName));
            }

            if (permissions == null || !permissions.Any())
            {
                throw new ArgumentException("Permissions cannot be empty.", nameof(permissions));
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Xóa tất cả các permission cũ của role
                var existingPermissions = _context.Permissions.Where(x => x.RoleName == roleName);
                _context.Permissions.RemoveRange(existingPermissions);

                // Thêm danh sách permission mới
                var newPermissions = permissions.Select(x => _mapper.Map<Permission>(x));
                await _context.Permissions.AddRangeAsync(newPermissions);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return roleName;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

    }
}
