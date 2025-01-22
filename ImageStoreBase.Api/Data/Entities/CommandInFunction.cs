using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageStoreBase.Api.Data.Entities
{
    [Table("CommandInFunctions")]
    public class CommandInFunction
    {
        [Required]
        [MaxLength(70)]
        public string CommandId { get; set; }
        
        [Required]
        [MaxLength(70)]
        public string FunctionId { get; set; }

        [ForeignKey("CommandId")]
        public virtual Command Command { get; set; }
        
        [ForeignKey("FunctionId")]
        public virtual Function Function { get; set; }
    }
}
