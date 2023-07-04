using Microsoft.AspNetCore.Mvc;
using ServerDN.Database;
using ServerDN.Models;
using System.Collections;

namespace ServerDN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(CategoryService.Get());
        }
        [HttpPost]
        public IActionResult Post(CategoryModel category)
        {
            try
            {
                CategoryService.Create(category);
                return Ok();

            }
            catch
            {
                return NotFound();
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                CategoryService.Delete(id);
                return NoContent();

            }
            catch(Exception ex)
            {
                return NotFound();
            }
        }
    }
}
