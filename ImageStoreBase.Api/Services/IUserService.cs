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
        Task<User> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(UserCreateRequestDTO userCreateDTO);
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<bool> UpdateAsync(Guid id, UserUpdateRequestDTO userUpdateDTO);
        Task<bool> DeleteAsync(Guid id);
    }
}
