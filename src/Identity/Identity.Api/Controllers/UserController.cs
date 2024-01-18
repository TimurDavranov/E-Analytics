using EAnalytics.Common.Primitives;
using Identity.Api.Models;
using Identity.Data.Models;
using Identity.Infrastructure.Extensions;
using Identity.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    public class UserController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager, IConfiguration configuration) : BaseApiController
    {

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            var expiresIn = DateTime.Now.AddMinutes(30);
            var expiresRefreshToken = DateTime.Now.AddDays(7);
            var refreshToken = TokentExtension.GenerateRefreshToken();

            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (result.Succeeded)
                {
                    var tokenString = configuration.GenerateJwtToken(user, expiresIn);
                    return Ok(
                        new AuthModel
                        { 
                            AccessToken = tokenString,
                            ExpiresIn = expiresIn,
                            RefreshToken = refreshToken,
                            RefreshExpiresIn = expiresRefreshToken,
                        });
                }
            }

            return Error("Wrong credentials");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Info()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
                return Ok(
                    new UserInfoModel
                    { 
                        UserName = user.UserName,
                        EMail = user.Email,
                        PhoneNumber = user.PhoneNumber
                    });

            return Ok(null);
        }
    }
}
