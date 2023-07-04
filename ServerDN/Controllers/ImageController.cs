using Microsoft.AspNetCore.Mvc;

namespace ServerDN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : Controller
    {
        [HttpGet("{img}")]
        public IActionResult Get(string img)
        {
            var image = System.IO.File.OpenRead("C:\\Users\\boong\\Documents\\images\\" + img);
            return File(image, "image/jpeg");
        }
    }
}
