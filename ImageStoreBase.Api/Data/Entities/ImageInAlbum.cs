using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageStoreBase.Api.Data.Entities
{
    [Table("ImageInAlbums")]
    public class ImageInAlbum
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public Guid AlbumId { get; set; }
        
        [Required]
        public Guid ImageId { get; set; }
        
        public Guid? CustomerId { get; set; }
        public bool? CustomerSelected { get; set; }
        
        [MaxLength(500)]
        public string? CustomerNotes { get; set; }

        [ForeignKey("AlbumId")]
        public virtual Album Album { get; set; }
        
        [ForeignKey("ImageId")]
        public virtual Image Image { get; set; }
    }
}
