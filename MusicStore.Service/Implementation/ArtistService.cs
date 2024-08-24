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
    public class ArtistService : IArtistService
    {
        private readonly IRepository<Artist> _artistRepository;

        public ArtistService(IRepository<Artist> artistRepository)
        {
            _artistRepository = artistRepository;
        }


        public void CreateNewArtist(Artist a)
        {
            _artistRepository.Insert(a);
        }

        public void DeleteArtist(Guid id)
        {
            var artist = _artistRepository.Get(id);
            _artistRepository.Delete(artist);
        }

        public List<Artist> GetAllArtists()
        {
            return _artistRepository.GetAll().ToList();
        }

        public Artist GetDetailsForArtist(Guid? id)
        {
            var artist = _artistRepository.Get(id);
            return artist;
        }

        public void UpdateExistingArtist(Artist a)
        {
            _artistRepository.Update(a);
        }
    }
}
