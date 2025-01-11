using ImageStoreBase.Api.Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ImageStoreBase.Api.Data.Entities
{
    [Table("Users")]
    public class User : IdentityUser<Guid>, IDateTracking
    {
        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }

        public string RefreshToken { get; set; }

        public DateTime ExpiryRefreshToken { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public ICollection<Collection> Collections { get; set; } = new List<Collection>();
    }
}
