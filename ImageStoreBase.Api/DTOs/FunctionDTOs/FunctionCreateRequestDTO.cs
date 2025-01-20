using System.ComponentModel.DataAnnotations;

namespace ImageStoreBase.Api.DTOs.FunctionDTOs
{
    public class FunctionCreateRequestDTO
    {
        [Required]
        [MaxLength(70)]
        public string Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(70)]
        public string? ParentId { get; set; }

        [Required]
        public int SortOrder { get; set; }

        public string Url { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; } = string.Empty;
    }
}
