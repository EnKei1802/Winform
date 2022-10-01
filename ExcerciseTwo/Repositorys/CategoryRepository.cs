using ExcerciseTwo.Models;
using ExcerciseTwo.Sevices;

namespace ExcerciseTwo.Repositorys
{
    public class CategoryRepository : ICategoryRepository
    {
        private ICategoryService _categoryService;
        public CategoryRepository(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public List<Category> GetAll()
        {
            return _categoryService.GetAll();
        }
    }
}
