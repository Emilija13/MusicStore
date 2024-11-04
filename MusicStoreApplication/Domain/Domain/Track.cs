using System.ComponentModel.DataAnnotations;

namespace MusicStore.Domain.Domain
{
    public class Track : BaseEntity
    {
        [Required]
        public string TrackName { get; set; }
        [Required]
        [Range(0, 59, ErrorMessage = "Minutes must be between 0 and 59.")]
        public int Minutes { get; set; }
        [Required]
        [Range(0, 59, ErrorMessage = "Seconds must be between 0 and 59.")]
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
