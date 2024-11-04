using System.ComponentModel.DataAnnotations;

namespace MusicStore.Domain.Domain
{
    public class Artist : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ArtistImage { get; set; }
        public virtual List<Album>? Albums { get; set; }

    }
}
