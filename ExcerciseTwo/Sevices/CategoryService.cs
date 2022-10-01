using Dapper;
using ExcerciseTwo.Connection;
using ExcerciseTwo.Models;
using System.Data.SqlClient;


namespace ExcerciseTwo.Sevices
{
    public class CategoryService : ICategoryService
    {
        private string _connectionString;
        public CategoryService()
        {
            _connectionString = ConnectionClass.GetConnection();
        }

        /// <summary>
        /// Get all category
        /// </summary>
        /// <returns></returns>
        public List<Category> GetAll()
        {
            var sql = "SELECT * FROM Category";
            List<Category> list = new List<Category>();
            using (var db = new SqlConnection(_connectionString))
            {
                db.Open();
               list = db.Query<Category>(sql).ToList();
            }
            return list;
        }
    }
}
