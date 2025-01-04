using ImageStoreBase.Api.Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageStoreBase.Api.Data.Entities
{
    [Table("Users")]
    public class User : IDateTracking
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        
        [Required]
        public string PasswordHash { get; set; }
        
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string FullName { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
