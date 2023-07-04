using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocketLibrary;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using ServerQT.Database;
using ServerQT.Models;

namespace ServerQT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        ArrayList products = new ArrayList();

        [HttpGet]
        public IActionResult Get()
        {
            products = ProductService.Get();
            return Ok(products);
        }

        [HttpPost]
        public IActionResult Post([FromForm] ProductModel pm)
        {
            string path = Path.Combine(@"C:\Users\boong\Documents\images\", pm.File.FileName);
            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                pm.File.CopyTo(stream);
            }
            ProductService.Create(pm);
            return Ok();
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            ProductModel pm = ProductService.GetById(id);
            return Ok(pm);
        }
    }
}
