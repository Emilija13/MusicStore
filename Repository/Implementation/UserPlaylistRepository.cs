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
    public class UserPlaylistRepository : IUserPlaylistRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<UserPlaylist> entities;

        public UserPlaylistRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<UserPlaylist>();
        }
        public List<UserPlaylist> GetAllUserPlaylists()
        {
            return entities
                            .Include(z => z.TracksInPlaylist)
                            .Include(z => z.User)
                            .Include("TracksInPlaylist.Track")
                            .ToList();
        }

    }
}
