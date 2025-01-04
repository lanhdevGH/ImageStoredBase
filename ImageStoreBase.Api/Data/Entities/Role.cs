using System.Security;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ImageStoreBase.Api.Data.Interfaces;

namespace ImageStoreBase.Api.Data.Entities
{
    [Table("Roles")]
    public class Role : IDateTracking
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
