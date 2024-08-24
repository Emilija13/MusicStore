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
    public class AlbumService : IAlbumService
    {

        private readonly IRepository<Album> _albumRepository;

        public AlbumService(IRepository<Album> albumRepository)
        {
            _albumRepository = albumRepository;
        }

        public void CreateNewAlbum(Album a)
        {
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
