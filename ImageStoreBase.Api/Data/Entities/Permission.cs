using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageStoreBase.Api.Data.Entities
{
    [Table("Permissions")]
    public class Permission
    {   
        [Required]
        [MaxLength(70)]
        public string RoleName { get; set; }
        
        [Required]
        [MaxLength(70)]
        public string FunctionId { get; set; }
        
        [Required]
        [MaxLength(70)]
        public string CommandId { get; set; }

        public virtual Role Role { get; set; }
        
        public virtual Function Function { get; set; }
        
        public Command Command { get; set; }
    }
}
