using Microsoft.AspNetCore.Mvc;

namespace Trackmeal.Controllers.Manage
{
    [Route("Manage/[controller]/[action]")]
    public class ProductsController : Controller
    {
        [Route("~/Manage/[controller]")]
        public string Index()
        {
            return "Here you can manage products";
        }

        public string New()
        {
            return "Adding new product";
        }

        [HttpGet("{id?}")]
        public string Edit(int id)
        {
            return $"Editing product with id {id}";
        }
    }
}
