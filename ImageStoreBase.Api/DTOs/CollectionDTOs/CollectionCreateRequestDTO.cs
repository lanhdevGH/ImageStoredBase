using System.ComponentModel.DataAnnotations;

namespace ImageStoreBase.Api.DTOs.CollectionDTOs
{
    public class CollectionCreateRequestDTO
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}
