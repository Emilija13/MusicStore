using MusicStore.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Service.Interface
{
    public interface IUserPlaylistService
    {
        List<UserPlaylist> GetAllPlaylists(string userId);
        UserPlaylist GetDetailsForPlaylist(string userId, Guid? id);
        void CreateNewPlaylist(string userId, UserPlaylist p);
        void UpdateExistingPlaylist(UserPlaylist p);
        void DeletePlaylist(Guid id);
        bool addTrackToPlaylist(string userId, TrackInPlaylist model);
        bool RemoveTrackFromPlaylist(string userId, Guid trackInPlaylistId, Guid playlistId);
    }
}
