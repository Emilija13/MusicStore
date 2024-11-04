using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Domain.Identity;
using MusicStore.Domain.DTO;

namespace MusicStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<MusicStoreUser> _userManager;
        public AdminController(UserManager<MusicStoreUser> userManager)
        {
            _userManager = userManager;
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
    }
}
