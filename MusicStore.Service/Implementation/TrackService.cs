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

        public TrackService(IRepository<Track> trackRepository)
        {
            _trackRepository = trackRepository;
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
