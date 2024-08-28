using System.ComponentModel.DataAnnotations;

namespace MusicStore.Domain.Domain
{
    public class Track : BaseEntity
    {
        [Required]
        public string TrackName { get; set; }
        [Required]
        public int Minutes { get; set; }
        [Required] 
        public int Seconds { get; set; }
        [Required]
        public Guid AlbumId { get; set; }
        public virtual Album? Album { get; set; }
        [Required]
        public double Rating { get; set; }
        public virtual Artist? Artist { get; set; }
        public virtual ICollection<TrackInPlaylist>? TracksInPlaylist { get; set; }

    }
}
