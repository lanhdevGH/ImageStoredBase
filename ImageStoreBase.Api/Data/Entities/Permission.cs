using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageStoreBase.Api.Data.Entities
{
    [Table("Permissions")]
    public class Permission
    {   
        [Required]
        public Guid RoleId { get; set; }
        
        [Required]
        [MaxLength(70)]
        public string FunctionId { get; set; }
        
        [Required]
        [MaxLength(70)]
        public string CommandId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        
        [ForeignKey("FunctionId")]
        public Function Function { get; set; }
        
        [ForeignKey("CommandId")]
        public Command Command { get; set; }
    }
}
