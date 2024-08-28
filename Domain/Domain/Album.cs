using System.ComponentModel.DataAnnotations;

namespace MusicStore.Domain.Domain
{
    public class Album : BaseEntity
    {
        [Required]
        public string AlbumName { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public string AlbumImage { get; set; }
        [Required]
        public double Rating { get; set; }
        [Required]
        public Guid? ArtistId { get; set; }
        public Artist? Artist { get; set; }
        public virtual List<Track>? Tracks { get; set; }
    }
}
