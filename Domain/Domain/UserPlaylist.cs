using MusicStore.Domain.Identity;
using System.ComponentModel.DataAnnotations;

namespace MusicStore.Domain.Domain
{
    public class UserPlaylist : BaseEntity
    {
        [Required]
        public string PlaylistName { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string UserId { get; set; }
        public virtual MusicStoreUser User { get; set; }
        public virtual ICollection<Track>? Tracks { get; set; }
    }
}
