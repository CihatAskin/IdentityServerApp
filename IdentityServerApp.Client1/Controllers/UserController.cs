using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

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

        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
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
            refreshTokenRequest.ClientId = _configuration["Client1Mvc:ClientId"];
            refreshTokenRequest.ClientSecret = _configuration["Client1Mvc:ClientSecret"];
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
