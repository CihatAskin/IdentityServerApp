using IdentityModel.Client;
using IdentityServerApp.Client1.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Net.Http;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServerApp.Client1.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");

            return RedirectToAction("Index", "Home");
            //await HttpContext.SignOutAsync("oidc"); auth server dan da çıkış yapması için
        }

        public async Task<IActionResult> GetRefreshToken()
        {
            var httpClient = new HttpClient();

            var disco = await httpClient.GetDiscoveryDocumentAsync("https://localhost:5001");//IdentityServer
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
            }

            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            var refreshTokenRequest = new RefreshTokenRequest();
            refreshTokenRequest.ClientId = _configuration["ClientResourceOwner:ClientId"];
            refreshTokenRequest.ClientSecret = _configuration["ClientResourceOwner:ClientSecret"];
            refreshTokenRequest.RefreshToken = refreshToken;
            refreshTokenRequest.Address = disco.TokenEndpoint;
            //username ve password u cookie den alır.

            var token = await httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            if (token.IsError)
            {
                //yönlendirme yap
            }

            var tokens = new List<AuthenticationToken>()
            {
               new AuthenticationToken{Name=OpenIdConnectParameterNames.IdToken,Value=token.IdentityToken },
               new AuthenticationToken{Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
               new AuthenticationToken{Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
               new AuthenticationToken{Name=OpenIdConnectParameterNames.ExpiresIn,
                                       Value=DateTime.UtcNow.AddSeconds(token.ExpiresIn)
                                                            .ToString("O",CultureInfo.InvariantCulture)},
            };

            var authenticationResult = await HttpContext.AuthenticateAsync();

            var properties = authenticationResult.Properties;
            properties.StoreTokens(tokens);

            await HttpContext.SignInAsync("Cookies", authenticationResult.Principal, properties);

            return RedirectToAction("index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminAction()
        {

            return View();
        }

        [Authorize(Roles = "Customer,Admin")]
        public IActionResult CustomerAction() => View();
    }
}
