using ProductManagement.Models;
namespace ProductManagement.Services.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        void Add(Product product);
        int GetCount { get;  }
    }
}
