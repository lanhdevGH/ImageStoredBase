using System.ComponentModel.DataAnnotations;

namespace ImageStoreBase.Api.DTOs.UserDTOs
{
    public class UserResponseDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
