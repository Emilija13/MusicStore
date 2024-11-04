using Microsoft.EntityFrameworkCore;
using MusicStore.Domain.Domain;
using MusicStore.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;

        public Repository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            if (typeof(T) == typeof(Album))
            {
                var albumsWithArtists = entities as IQueryable<Album>;
                return albumsWithArtists
                    .Include(a => a.Artist)
                    .Include(a => a.Tracks) 
                    .AsEnumerable() as IEnumerable<T>;
            }
            else if (typeof(T) == typeof(UserPlaylist))
            {
                var playlists = entities as IQueryable<UserPlaylist>;
                return playlists
                    .Include(p => p.TracksInPlaylist)
                        .ThenInclude(tp => tp.Track)
                    .AsEnumerable() as IEnumerable<T>;
            }
            else if (typeof(T) == typeof(Artist))
            {
                var artists = entities as IQueryable<Artist>;
                return artists
                    .Include(p => p.Albums)
                    .AsEnumerable() as IEnumerable<T>;
            }

            return entities.AsEnumerable();
        }

        public T Get(Guid? id)
        {
            if (typeof(T) == typeof(Album))
            {
                var albums = entities as IQueryable<Album>;
                return albums
                    .Include(a => a.Artist)
                    .Include(a => a.Tracks)
                    .SingleOrDefault(a => a.Id == id) as T;
            }
            else if (typeof(T) == typeof(UserPlaylist))
            {
                var playlists = entities as IQueryable<UserPlaylist>;
                return playlists
                    .Include(p => p.TracksInPlaylist)
                        .ThenInclude(tp => tp.Track)
                            .ThenInclude(t => t.Album)
                                .ThenInclude(a => a.Artist)
                    .SingleOrDefault(p => p.Id == id) as T;      
            }

            else if (typeof(T) == typeof(Artist))
            {
                var artists = entities as IQueryable<Artist>;
                return artists
                    .Include(p => p.Albums)
                    .SingleOrDefault(p => p.Id == id) as T;
            }


            return entities.SingleOrDefault(s => s.Id == id);
        }
        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
    }
}
