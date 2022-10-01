using ExcerciseTwo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcerciseTwo.Sevices
{
    public interface IProductService
    {
        List<Product> GetAll();

        Product? GetProductById(int id);
        bool Delete(int id);
        bool Put(int id, Product product);
        bool Post(Product product);
    }
}
