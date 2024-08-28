using MusicStore.Domain.Identity;
using System.ComponentModel.DataAnnotations;

namespace MusicStore.Domain.Domain
{
    public class UserPlaylist : BaseEntity
    {
        [Required]
        public string PlaylistName { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string UserId { get; set; }
        public MusicStoreUser? User { get; set; }
        public virtual ICollection<TrackInPlaylist>? TracksInPlaylist { get; set; }
    }
}
