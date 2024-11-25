using MusicStore.Domain.Domain;
using MusicStore.Domain;
using MusicStore.Repository.Interface;
using MusicStore.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Domain.DTO;

namespace MusicStore.Service.Implementation
{
    public class ShoppingCartService: IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<AlbumsInCart> _albumsInShoppingCartRepository;
        private readonly IUserRepository _userRepository;
        public ShoppingCartService(IRepository<AlbumsInCart> albumsInShoppingCartRepository, IUserRepository userRepository, IRepository<ShoppingCart> shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _albumsInShoppingCartRepository = albumsInShoppingCartRepository;
            _userRepository = userRepository;
        }
        public bool AddToShoppingConfirmed(AlbumsInCart model, string userId)
        {
            var loggedInUser = _userRepository.Get(userId);

            var userShoppingCart = loggedInUser.ShoppingCart;

            if (userShoppingCart.AlbumsInCart == null)
                userShoppingCart.AlbumsInCart = new List<AlbumsInCart>(); ;

            userShoppingCart.AlbumsInCart.Add(model);
            _shoppingCartRepository.Update(userShoppingCart);
            return true;
        }

        public bool deleteProductFromShoppingCart(string userId, Guid albumId)
        {
            if (albumId != null)
            {
                var loggedInUser = _userRepository.Get(userId);

                var userShoppingCart = loggedInUser.ShoppingCart;
                var album = userShoppingCart.AlbumsInCart.Where(x => x.AlbumId == albumId).FirstOrDefault();

                userShoppingCart.AlbumsInCart.Remove(album);

                _shoppingCartRepository.Update(userShoppingCart);
                return true;
            }
            return false;
        }

        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            var loggedInUser = _userRepository.Get(userId);

            var userShoppingCart = loggedInUser?.ShoppingCart;
            var allAlbums = userShoppingCart?.AlbumsInCart?.ToList();

            var totalPrice = allAlbums.Select(x => (x.Album.Price * x.Quantity)).Sum();

            ShoppingCartDto dto = new ShoppingCartDto
            {
                Albums = allAlbums,
                TotalPrice = totalPrice
            };
            return dto;
        }

        public bool order(string userId)
        {
            if (userId != null)
            {
                var loggedInUser = _userRepository.Get(userId);

                loggedInUser.ShoppingCart.AlbumsInCart.Clear();
                _userRepository.Update(loggedInUser);
                return true;
            }
            return false;
        }

    }
}

