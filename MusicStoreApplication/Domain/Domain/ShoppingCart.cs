using MusicStore.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Domain.Domain
{
    public class ShoppingCart:BaseEntity
    {
        public string? OwnerId { get; set; }
        public MusicStoreUser? Owner { get; set; }
        public virtual ICollection<AlbumsInCart> AlbumsInCart { get; set; }
    }
}
