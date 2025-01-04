using ImageStoreBase.Api.Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageStoreBase.Api.Data.Entities
{
    [Table("Albums")]
    public class Album : IDateTracking
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; }
        
        [Required]
        public Guid CollectionId { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [ForeignKey("CollectionId")]
        public Collection Collection { get; set; }
        public ICollection<ImageInAlbum> ImageInAlbums { get; set; } = new List<ImageInAlbum>();
    }
}
