using System.ComponentModel.DataAnnotations;

namespace MusicStore.Web.Models.Domain
{
    public class Track : BaseEntity
    {
        [Required]
        public string TrackName { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public Guid AlbumId { get; set; }
        public virtual Album Album { get; set; }
        [Required]
        public double Rating { get; set; }

        public virtual Artist? Artist { get; set; }

    }
}
