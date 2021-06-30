using IdentityModel.Client;
using IdentityServerApp.Client1.Models;
using IdentityServerApp.Client1.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServerApp.Client1.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IApiResourceHttpClient _apiResourceHttpClient;

        public LoginController(IConfiguration configuration,
                               IApiResourceHttpClient apiResourceHttpClient)
        {
            _configuration = configuration;
            _apiResourceHttpClient = apiResourceHttpClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync(_configuration["AuthServerUrl"]);
            if (disco.IsError)
            {
                // error log
            }

            var password = new PasswordTokenRequest();

            password.Address = disco.TokenEndpoint;
            password.UserName = loginViewModel.Email;
            password.Password = loginViewModel.Password;
            password.ClientId = _configuration["ClientResourceOwner:ClientId"];
            password.ClientSecret = _configuration["ClientResourceOwner:ClientSecret"];

            var token = await client.RequestPasswordTokenAsync(password);// authserverdan token aldık

            if (token.IsError)
            {
                ModelState.AddModelError("", "Email veya şifreniz yanlış");
                return View();
                //error log
            }

            var userInfoRequest = new UserInfoRequest();
            userInfoRequest.Token = token.AccessToken;
            userInfoRequest.Address = disco.UserInfoEndpoint;
            var userInfo = await client.GetUserInfoAsync(userInfoRequest);// kullanıcı bilgilerini aldık

            if (userInfo.IsError)
            {
                //error log
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                                                userInfo.Claims,
                                                CookieAuthenticationDefaults.AuthenticationScheme,
                                                "name",
                                                "role");

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var properties = new AuthenticationProperties();

            var tokens = new List<AuthenticationToken>()
            {
               new AuthenticationToken{Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
               new AuthenticationToken{Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
               new AuthenticationToken{Name=OpenIdConnectParameterNames.ExpiresIn,
                                       Value=DateTime.UtcNow.AddSeconds(token.ExpiresIn)
                                                            .ToString("O",CultureInfo.InvariantCulture)},
            };

            properties.StoreTokens(tokens);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, properties);

            return RedirectToAction("Index", "User");
        }

        public IActionResult SignUp()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(UserSaveViewModel userSaveViewModel)
        {
            if (!ModelState.IsValid) return View();

            var result = await _apiResourceHttpClient.SaveUserViewModel(userSaveViewModel);

            if (result != null)
            {
                result.ForEach(err =>
                {
                    ModelState.AddModelError("", err);

                });
                return View();
            }

            return RedirectToAction("Index");
        }
    }
}
