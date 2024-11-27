using MusicStore.Domain.Domain;
using MusicStore.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Service.Interface
{
    public interface IShoppingCartService
    {
        bool AddToShoppingConfirmed(AlbumsInCart model, string userId);
        ShoppingCartDto getShoppingCartInfo(string userId);
        bool deleteProductFromShoppingCart(string userId, Guid albumId);
        bool order(string userId);

    }
}
