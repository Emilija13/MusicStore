using MusicStore.Domain.Domain;
using MusicStore.Repository.Interface;
using MusicStore.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Service.Implementation
{
    public class UserPlaylistService : IUserPlaylistService
    {
        private readonly IRepository<UserPlaylist> _userPlaylistRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Track> _trackRepository;
        private readonly IUserPlaylistRepository _userPlRepository;


        public UserPlaylistService(IRepository<UserPlaylist> userPlaylistRepository, IUserRepository userRepository, IRepository<Track> trackRepository,IUserPlaylistRepository userPlRepository) 
        { 
            _userPlaylistRepository = userPlaylistRepository;
            _userPlRepository = userPlRepository;
            _userRepository = userRepository;
            _trackRepository = trackRepository;
        }

        public bool addTrackToPlaylist(string userId, TrackInPlaylist model)
        {
            var loggedInUser = _userRepository.Get(userId);
            var userPlaylists = loggedInUser.Playlists;

            var selectedPlaylist = userPlaylists.FirstOrDefault(p => p.Id == model.UserPlaylistId);

            if (selectedPlaylist.TracksInPlaylist == null)
            {
                selectedPlaylist.TracksInPlaylist = new List<TrackInPlaylist>();
            }

            var track = _trackRepository.Get(model.TrackId);
            model.Track = track;
            selectedPlaylist.TracksInPlaylist.Add(model);
            _userPlaylistRepository.Update(selectedPlaylist);
            return true;
        }

        public bool RemoveTrackFromPlaylist(string userId, Guid trackInPlaylistId, Guid playlistId)
        {
            var loggedInUser = _userRepository.Get(userId);
            var userPlaylists = loggedInUser.Playlists;

            var selectedPlaylist = userPlaylists.FirstOrDefault(p => p.Id == playlistId);

            var trackInPlaylist = selectedPlaylist.TracksInPlaylist.FirstOrDefault(p => p.Id == trackInPlaylistId);

            selectedPlaylist.TracksInPlaylist.Remove(trackInPlaylist);
            _userPlaylistRepository.Update(selectedPlaylist);
            return true;
        }

        public void CreateNewPlaylist(string userId, UserPlaylist p)
        {
            p.UserId = userId;
            p.User = _userRepository.Get(userId);
            _userPlaylistRepository.Insert(p);
        }

        public void DeletePlaylist(Guid id)
        {
            var playlist = _userPlaylistRepository.Get(id);
            _userPlaylistRepository.Delete(playlist);
        }

        public List<UserPlaylist> GetAllPlaylists(string userId)
        {
            var loggedInUser = _userRepository.Get(userId);
            return loggedInUser?.Playlists?.ToList();
        }
        public List<UserPlaylist> GetAllPlaylistsForExport()
        {
            return _userPlRepository.GetAllUserPlaylists();
        }
        public UserPlaylist GetDetailsForPlaylist(string userId, Guid? id)
        {
            //var loggedInUser = _userRepository.Get(userId);
            //var userPlaylists = loggedInUser.Playlists;

            var selectedPlaylist = _userPlaylistRepository.Get(id);
            return selectedPlaylist;
        }

        public void UpdateExistingPlaylist(UserPlaylist p)
        {
            _userPlaylistRepository.Update(p);
        }
    }
}
