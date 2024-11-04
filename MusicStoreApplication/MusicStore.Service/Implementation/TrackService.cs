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
    public class TrackService : ITrackService
    {
        private readonly IRepository<Track> _trackRepository;
        private readonly IRepository<Album> _albumRepository;
        private readonly IRepository<UserPlaylist> _userPlaylistRepository;

        public TrackService(IRepository<Track> trackRepository, IRepository<Album> albumRepository, IRepository<UserPlaylist> userPlaylistRepository)
        {
            _trackRepository = trackRepository;
            _albumRepository = albumRepository;
            _userPlaylistRepository = userPlaylistRepository;
        }

        public List<int> CalculateTotalDuration(Guid id)
        {
            var album = _albumRepository.Get(id);
            var playlist = _userPlaylistRepository.Get(id);

            int totalHours = 0;
            int totalMinutes = 0;
            int totalSeconds = 0;

            if (album != null)
            {
                foreach (var track in album.Tracks)
                {
                    totalMinutes += track.Minutes;
                    totalSeconds += track.Seconds;
                }
            }
            else if (playlist != null)
            {
                foreach (var trackInPlaylist in playlist.TracksInPlaylist)
                {
                    totalMinutes += trackInPlaylist.Track.Minutes;
                    totalSeconds += trackInPlaylist.Track.Seconds;
                }
            }
            else
            {
                throw new ArgumentException("Invalid ID: The ID does not correspond to an album or a playlist.");
            }

            if (totalSeconds >= 60)
            {
                totalMinutes += totalSeconds / 60;
                totalSeconds = totalSeconds % 60;
            }

            if (totalMinutes >= 60)
            {
                totalHours += totalMinutes / 60;
                totalMinutes = totalMinutes % 60;
            }

            return new List<int> { totalHours, totalMinutes, totalSeconds };
        }



        public void CreateNewTrack(Track t)
        {
            _trackRepository.Insert(t);
        }

        public void DeleteTrack(Guid id)
        {
            var track = _trackRepository.Get(id);
            _trackRepository.Delete(track);
        }

        public List<Track> GetAllTracks()
        {
            return _trackRepository.GetAll().ToList();
        }

        public Track GetDetailsForTrack(Guid? id)
        {
            return _trackRepository.Get(id);
        }

        public void UpdateExistingTrack(Track t)
        {
            _trackRepository.Update(t);
        }
    }
}
