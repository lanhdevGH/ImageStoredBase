using ImageStoreBase.Api.Data.Interfaces;
using Newtonsoft.Json;
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
        public string Url { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]  
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public virtual Function? FunctionParent { get; set; } = null;
        public virtual ICollection<Function> ChildFunctions { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
        public virtual ICollection<CommandInFunction> CommandInFunctions { get; set; }
    }
}
