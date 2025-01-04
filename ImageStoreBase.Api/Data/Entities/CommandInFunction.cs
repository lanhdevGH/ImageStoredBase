using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageStoreBase.Api.Data.Entities
{
    [Table("CommandInFunctions")]
    public class CommandInFunction
    {
        [Required]
        public Guid CommandId { get; set; }
        
        [Required]
        public Guid FunctionId { get; set; }

        [ForeignKey("CommandId")]
        public Command Command { get; set; }
        
        [ForeignKey("FunctionId")]
        public Function Function { get; set; }
    }
}
