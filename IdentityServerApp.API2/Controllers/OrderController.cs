using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using IdentityServerApp.API2.Model;
using Microsoft.AspNetCore.Authorization;

namespace IdentityServerApp.API2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : Controller
    {
        [Authorize]
        [HttpGet]
        public IActionResult GetOrders()
        {
            var orders = new List<Order>() {
                         new Order{ Id=1,ProductName="Tava",Date=DateTime.Now},
                         new Order{ Id=2,ProductName="Tencere",Date=DateTime.Now.AddHours(1)},
                         new Order{ Id=3,ProductName="Merdiven",Date=DateTime.Now.AddHours(2)},
                         new Order{ Id=4,ProductName="Bardak",Date=DateTime.Now.AddHours(3)}
            };
            return Ok(orders);
        }
    }
}
