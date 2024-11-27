using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Domain.Identity;
using MusicStore.Domain.DTO;
using MusicStore.Service.Interface;
using MusicStore.Domain.Domain;

namespace MusicStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<MusicStoreUser> _userManager;

        private readonly IAlbumService _albumService;
        private readonly IArtistService _artistService;

        public AdminController(UserManager<MusicStoreUser> userManager, IAlbumService albumService, IArtistService artistService)
        {
            _userManager = userManager;
            _albumService = albumService;
            _artistService = artistService;
        }

        [HttpPost("[action]")]
        public bool ImportAllUsers(List<UserRegistrationDto> model)
        {
            bool status = true;

            foreach (var item in model)
            {
                var userCheck = _userManager.FindByEmailAsync(item.Email).Result;

                if (userCheck == null)
                {
                    var user = new MusicStoreUser
                    {
                        UserName = item.Email,
                        NormalizedUserName = item.Email,
                        Email = item.Email,
                        EmailConfirmed = true
                    };

                    var result = _userManager.CreateAsync(user, item.Password).Result;
                    status = status && result.Succeeded;
                }
                else
                {
                    continue;
                }
            }
            return status;
        }

        [HttpGet("albums")]
        public async Task<IActionResult> ExportAllAlbums()
        {
            var albums = _albumService.GetAllAlbums();
            return Ok(albums);
        }

        [HttpGet("artists")]
        public async Task<IActionResult> ExportAllArtists()
        {
            var artists = _artistService.GetAllArtists();
            return Ok(artists);
        }
    }
}
