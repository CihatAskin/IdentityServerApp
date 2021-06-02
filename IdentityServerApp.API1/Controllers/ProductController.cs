using IdentityServerApp.API1.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerApp.API1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : Controller
    {
        [Authorize("Read")]
        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = new List<Product>() {
            new Product{ Id=1,Name="Tava",Price=150,Stock=12},
            new Product{ Id=2,Name="Tencere",Price=150,Stock=10},
            new Product{ Id=3,Name="Merdiven",Price=150,Stock=20},
            new Product{ Id=4,Name="Bardak",Price=150,Stock=155}
            };
            return Ok(products);
        }

        [Authorize("UpdateOrCreate")]
        public IActionResult UpdateProduct(int id)
        {
            return Ok($"{id} Product Updated");
        }

        [Authorize("UpdateOrCreateProduct")]
        public IActionResult CreateProduct(Product product)
        {
            return Ok($"{product.Name} Product Created");
        }
    }
}
