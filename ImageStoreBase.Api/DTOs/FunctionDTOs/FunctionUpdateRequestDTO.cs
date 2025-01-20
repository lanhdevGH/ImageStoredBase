using System.ComponentModel.DataAnnotations;

namespace ImageStoreBase.Api.DTOs.FunctionDTOs
{
    public class FunctionUpdateRequestDTO
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(70)]
        public string? ParentId { get; set; }
        public int SortOrder { get; set; }

        public string Url { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
