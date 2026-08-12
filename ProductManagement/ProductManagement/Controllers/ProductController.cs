using Microsoft.AspNetCore.Mvc;
using ProductManagement.Models;
using ProductManagement.Services.Interfaces;

namespace ProductManagement.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            return View(products);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Product product)
        {
            _productRepository.Add(product);
            return RedirectToAction("Index");
        }
       
        
    }
}
