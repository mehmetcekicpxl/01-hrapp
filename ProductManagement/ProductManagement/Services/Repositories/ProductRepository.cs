using ProductManagement.Models;
using ProductManagement.Services.Interfaces;

namespace ProductManagement.Services.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new List<Product>();

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        public void Add(Product product)
        {
            _products.Add(product);
        }

        public int GetCount => _products.Count;
    }
}
