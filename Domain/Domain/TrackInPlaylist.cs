using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Domain.Domain
{
    public class TrackInPlaylist: BaseEntity
    {
        public Guid TrackId { get; set; }
        public Track? Track { get; set; }
        public Guid UserPlaylistId { get; set; }
        public UserPlaylist? UserPlaylist { get; set; }
    }
}
