using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.GenericDTO;
using ImageStoreBase.Api.DTOs.UserDTOs;
using Microsoft.AspNetCore.Identity;

namespace ImageStoreBase.Api.Services
{
    public interface IUserService
    {
        Task<PagedResult<User>> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(string id);
        Task<string> CreateAsync(UserCreateRequestDTO userCreateDTO);
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<bool> UpdateAsync(string id, UserUpdateRequestDTO userUpdateDTO);
        Task<bool> DeleteAsync(string id);
    }
}
