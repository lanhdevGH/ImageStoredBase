using System.ComponentModel.DataAnnotations;

namespace ImageStoreBase.Api.DTOs.CollectionDTOs
{
    public class CollectionCreateRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid UserId { get; set; }
    }
}
