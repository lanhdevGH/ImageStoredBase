using System.ComponentModel.DataAnnotations;

namespace ImageStoreBase.Api.DTOs.CommandDTO
{
    public class CommandResponseDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
