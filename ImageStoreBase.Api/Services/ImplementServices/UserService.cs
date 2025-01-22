using AutoMapper;
using ImageStoreBase.Api.Data;
using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.GenericDTO;
using ImageStoreBase.Api.DTOs.UserDTOs;
using ImageStoreBase.Api.MyExceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ImageStoreBase.Api.Services.ImplementServices
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserService(AppDbContext context, UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<PagedResult<User>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _userManager.Users.AsQueryable();

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<User>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            return user ?? throw new KeyNotFoundException("User id not found");
        }

        public async Task<Guid> CreateAsync(UserCreateRequestDTO userCreateDTO)
        {
            var newUser = _mapper.Map<User>(userCreateDTO);
            newUser.Id = Guid.NewGuid();
            var result = await _userManager.CreateAsync(newUser, userCreateDTO.Password);
            if (!result.Succeeded)
            {
                /// Tìm lỗi liên quan đến mật khẩu trong danh sách lỗi
                var passwordErrors = result.Errors
                    .Where(e => e.Code.Contains("Password"))
                    .Select(e => e.Description);

                if (passwordErrors.Any())
                {
                    throw new MyPasswordException(string.Join(", ", passwordErrors));
                }

                // Ném exception chung nếu lỗi không phải do mật khẩu
                var allErrors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new CreateUserException($"User creation failed: {allErrors}");
            }
            return newUser.Id;
        }

        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User id not found");
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
            {
                // Ném exception chung nếu lỗi không phải do mật khẩu
                var allErrors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ChangePasswordException($"Change password user failed: {allErrors}");
            }
            return result.Succeeded;
        }

        public async Task<bool> UpdateAsync(Guid id, UserUpdateRequestDTO userUpdateDTO)
        {
            var existingUser = await _userManager.FindByIdAsync(id.ToString());
            if (existingUser == null) return false;

            _mapper.Map(userUpdateDTO, existingUser);

            await _userManager.UpdateAsync(existingUser);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return false;

            await _userManager.DeleteAsync(user);
            return true;
        }
    }
}
