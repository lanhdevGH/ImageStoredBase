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
        public Command Command { get; set; }
        
        [ForeignKey("FunctionId")]
        public Function Function { get; set; }
    }
}
