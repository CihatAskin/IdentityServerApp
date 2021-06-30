using IdentityServerApp.AuthServer.IdentityApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServerApp.AuthServer.IdentityApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(LocalApi.PolicyName)]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        [HttpPost]
        public async Task<IActionResult> SignUp(UserSaveViewModel model)
        {
            var appUser = new ApplicationUser();

            appUser.UserName = model.UserName;
            appUser.Email = model.Email;
            appUser.City = model.City;

            var result = await _userManager.CreateAsync(appUser, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(x=>x.Description));
            }

            return Ok("Üye Kaydedildi");
        }
    }
}
