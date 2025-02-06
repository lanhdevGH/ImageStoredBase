using System.ComponentModel.DataAnnotations;

namespace ImageStoreBase.Api.DTOs.FunctionDTOs
{
    public class FunctionResponseDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string? ParentId { get; set; }
        public string? ParentName { get; set; } // Tên FunctionParent
        public int SortOrder { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<FunctionResponseDTO> ChildFunctions { get; set; } = new();
    }
}
