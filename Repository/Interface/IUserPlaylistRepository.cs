using MusicStore.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Repository.Interface
{
    public interface IUserPlaylistRepository
    {
        List<UserPlaylist> GetAllUserPlaylists();
    }
}
