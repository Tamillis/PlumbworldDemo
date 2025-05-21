using Microsoft.AspNetCore.Mvc;
using PlumbworldDemo.Models.Products;
using PlumbworldDemo.Services;

namespace PlumbworldDemo.Controllers
{
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        public ProductsController(IProductsService service)
        {
            _service = service;
        }
        private IProductsService _service;

        public IActionResult Index(string q, string active, string inactive)
        {
            var vm = new ProductsViewModel();

            var query = _service.Products;

            if (q != null) query = query.Where(p => p.Name.ToLower().Contains(q.ToLower()));
            if (active != null && active == "on") query = query.Where(p => p.IsActive);
            if (inactive != null && inactive == "on") query = query.Where(p => !p.IsActive);

            //and store the query parameters in the ViewBag to set the filter inputs accordingly
            ViewData["q"] = q;
            ViewData["active"] = active;
            ViewData["inactive"] = inactive;

            vm.Products = query.ToList();
            return View(vm);
        }

        //GET
        [Route("Update/{id}")]
        [Route("Create")]
        public IActionResult Update(int? id)
        {
            Product p;
            if (id == null || (int)id == 0)
            {
                //This is actually called via /Products/Create
                //Use a blank model as a basis for creating a new product
                p = new Product() { };
            }
            else
            {
                //Returns the update view with actual product data, unless of course it is again a blank
                p = _service.GetProduct((int)id) ?? new Product();
            }
            return View(p);
        }

        [HttpPost]
        [Route("Update/{id}")]
        [Route("Create")]
        public IActionResult Update(Product p)
        {
            // new product
            if (p.Id == 0) _service.AddProduct(p);
            // update product
            else _service.UpdateProduct(p);

            return RedirectToAction("Index");
        }

        //GET
        [Route("[action]/{id:int}")]
        public IActionResult Delete(int id)
        {
            var p = _service.GetProduct(id);
            return View(p);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.DeleteProduct(id);
            return RedirectToAction("Index");
        }
    }
}
