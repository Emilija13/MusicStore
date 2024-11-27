using MusicStore.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Domain.DTO
{
    public class ShoppingCartDto
    {
        public List<AlbumsInCart>? Albums { get; set; }
        public double TotalPrice { get; set; }
    }
}
