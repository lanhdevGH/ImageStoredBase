using System.Security;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ImageStoreBase.Api.Data.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ImageStoreBase.Api.Data.Entities
{
    [Table("Roles")]
    public class Role : IdentityRole<Guid>
    {
        public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
