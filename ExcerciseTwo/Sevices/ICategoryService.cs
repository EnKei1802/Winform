using ExcerciseTwo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcerciseTwo.Sevices
{
    public interface ICategoryService
    {
        List<Category> GetAll();
    }
}
