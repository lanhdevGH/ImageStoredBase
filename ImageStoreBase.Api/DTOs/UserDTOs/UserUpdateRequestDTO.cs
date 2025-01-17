using System.ComponentModel.DataAnnotations;

namespace ImageStoreBase.Api.DTOs.UserDTOs
{
    public class UserUpdateRequestDTO
    {
        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }
    }
}
