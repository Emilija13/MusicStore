using MusicStore.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Service.Interface
{
    public interface ITrackService
    {
        List<Track> GetAllTracks();
        Track GetDetailsForTrack(Guid? id);
        void CreateNewTrack(Track t);
        void UpdateExistingTrack(Track t);
        void DeleteTrack(Guid id);
    }
}
