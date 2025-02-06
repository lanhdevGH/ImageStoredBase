using System.ComponentModel.DataAnnotations;

namespace ImageStoreBase.Api.DTOs.CommandDTO
{
    public class CommandCreateRequestDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
