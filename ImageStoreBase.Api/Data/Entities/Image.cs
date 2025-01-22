using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageStoreBase.Api.Data.Entities
{
    [Table("Images")]
    public class Image
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Url { get; set; }
        
        [MaxLength(500)]
        public string ThumbnailUrl { get; set; }
        
        public string Metadata { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<ImageInAlbum> ImageInAlbums { get; set; } = new List<ImageInAlbum>();
    }
}
