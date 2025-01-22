using ImageStoreBase.Api.Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageStoreBase.Api.Data.Entities
{
    [Table("Commands")]
    public class Command : IDateTracking
    {
        [Key]
        [MaxLength(70)]
        public string Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<CommandInFunction> CommandInFunctions { get; set; } = new List<CommandInFunction>();

        public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
