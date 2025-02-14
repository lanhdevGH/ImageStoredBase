using ImageStoreBase.Api.DTOs.GenericDTO;
using ImageStoreBase.Api.DTOs.UserDTOs;

namespace ImageStoreBase.Api.Services
{
    public interface IUserService
    {
        Task<PagedResult<UserResponseDTO>> GetPagedAsync(int pageNumber, int pageSize);
        Task<List<UserResponseDTO>> GetAllAsync();
        Task<UserResponseDTO?> GetByIdAsync(string id);
        Task<string> CreateAsync(UserCreateRequestDTO userCreateDTO);
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<bool> UpdateAsync(string id, UserUpdateRequestDTO userUpdateDTO);
        Task<bool> DeleteAsync(string id);
    }
}
