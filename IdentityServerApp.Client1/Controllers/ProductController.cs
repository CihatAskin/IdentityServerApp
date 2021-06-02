﻿using IdentityModel.Client;
using IdentityServerApp.Client1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServerApp.Client1.Controllers
{
    public class ProductController : Controller
    {
        private readonly IConfiguration _configuration;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var products = new List<Product>();
            using (var client = new HttpClient())
            {
                var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");//IdentityServer

                if (disco.IsError)
                {
                    Console.WriteLine(disco.Error);
                }

                var request = new ClientCredentialsTokenRequest();
                request.ClientId = _configuration["Client:ClientId"];
                request.ClientSecret = _configuration["Client:ClientSecret"];
                request.Address = disco.TokenEndpoint;

                var token = await client.RequestClientCredentialsTokenAsync(request);//acces Token

                if (token.IsError)
                {
                    Console.WriteLine(token.Error);
                }

                client.SetBearerToken(token.AccessToken);
                var response = await client.GetAsync("https://localhost:5007/api/product/getproducts");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    products = JsonConvert.DeserializeObject<List<Product>>(content);
                }
                else
                {

                }
            }

            return View(products);
        }
    }
}
