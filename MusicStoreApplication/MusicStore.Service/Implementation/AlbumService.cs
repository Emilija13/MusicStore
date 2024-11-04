using MusicStore.Domain.Domain;
using MusicStore.Repository.Interface;
using MusicStore.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Service.Implementation
{
    public class AlbumService : IAlbumService
    {

        private readonly IRepository<Album> _albumRepository;
        private readonly IRepository<Artist> _artistRepository;

        public AlbumService(IRepository<Album> albumRepository, IRepository<Artist> artistRepository)
        {
            _albumRepository = albumRepository;
            _artistRepository = artistRepository;
        }

        public void CreateNewAlbum(Album a)
        {
            var artist = _artistRepository.Get(a.ArtistId);
            a.Artist = artist;
            _albumRepository.Insert(a);
        }

        public void DeleteAlbum(Guid id)
        {
            var album = _albumRepository.Get(id);
            _albumRepository.Delete(album);
        }

        public List<Album> GetAllAlbums()
        {
            return _albumRepository.GetAll().ToList();
        }

        public Album GetDetailsForAlbum(Guid? id)
        {
            var album = _albumRepository.Get(id);
            return album;
        }

        public void UpdateExistingAlbum(Album a)
        {
            _albumRepository.Update(a);
        }
    }
}
