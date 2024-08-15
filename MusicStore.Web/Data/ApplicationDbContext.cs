using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicStore.Web.Models.Domain;
using MusicStore.Web.Models.Identity;

namespace MusicStore.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<MusicStoreUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Artist> Artists { get; set; }
        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Track> Tracks { get; set; }
        public virtual DbSet<UserPlaylist> UserPlaylists { get; set; }

    }
}
