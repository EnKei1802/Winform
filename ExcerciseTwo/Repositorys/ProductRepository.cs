using ExcerciseTwo.Models;
using ExcerciseTwo.Sevices;

namespace ExcerciseTwo.Repositorys
{
    public class ProductRepository : IProductRepository
    {
        private readonly IProductService _productService;
        public ProductRepository(IProductService productService)
        {
            _productService = productService;
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAll()
        {
            return _productService.GetAll();
        }

        public Product? GetProductById(int id)
        {
            return _productService.GetProductById(id);
        }


        public bool Post(Product product)
        {
            return _productService.Post(product);
        }

        public bool Put(int id, Product product)
        {
            return _productService.Put(id, product); 
        }
    }
}
