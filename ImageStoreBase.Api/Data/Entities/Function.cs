using ImageStoreBase.Api.Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageStoreBase.Api.Data.Entities
{
    [Table("Functions")]
    public class Function : IDateTracking
    {
        [Key]
        [MaxLength(70)]
        public string Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

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

        public Function? FunctionParent { get; set; } = null;
        public ICollection<Function> ChildFunctions { get; set; } = new List<Function>();
        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
        public ICollection<CommandInFunction> CommandInFunctions { get; set; } = new List<CommandInFunction>();
    }
}
