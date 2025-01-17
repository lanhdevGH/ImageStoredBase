using System.ComponentModel.DataAnnotations;

namespace ImageStoreBase.Api.DTOs.UserDTOs
{
    public class UserCreateRequestDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        public string Password { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
