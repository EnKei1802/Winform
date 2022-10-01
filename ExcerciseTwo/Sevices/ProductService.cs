using Dapper;
using ExcerciseTwo.Connection;
using ExcerciseTwo.Models;
using System.Data.SqlClient;


namespace ExcerciseTwo.Sevices
{
    public class ProductService : IProductService
    {
        #region
        private static string cs = ConnectionClass.GetConnection();

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns></returns>
        public List<Product> GetAll()
        {
            string query = @"SELECT p.* ,c.categoryId[categoryId], c.CategoryName
                             FROM product p,category c  
                             WHERE p.categoryId = c.categoryId";
            List<Product> products;
            using (var connection = new SqlConnection(cs))
            {
                products = connection.Query<Product>(query).ToList();
            }
            return products;
        }

        /// <summary>
        /// Get product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product? GetProductById(int id)
        {
            string query = "SELECT p.*,categoryId[categoryId], c.CategoryName" +
                           "FORM product p, category c " +
                           "WHERE p.categoryId = c.categoryId and p.categoryId = @id";
            Product? product;
            using (var connection = new SqlConnection(cs))
            {
                product = connection.QueryFirstOrDefault<Product>(query, new {id = id});
            }    
            return product;
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            string query = "DELETE FROM product p WHERE p.productId = @id";
            using (var connection = new SqlConnection(cs))
            {
              int rows =  connection.Execute(query, new { id = id });
                if(rows > 0)
                    return true;
                return false;

            }    
        }

        /// <summary>
        /// Update a product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        public bool Put(int id ,Product product)
        {
            if(id != product.productId) return false;

            using (var connection = new SqlConnection(cs))
            {
                var query = @"
                            UPDATE Product 
                            SET name = @name, 
                            description = @description, 
                            price = @price,
                            createdDate = @createdDate, 
                            quantity = @quantity, 
                            type = @type,
                            photo = @photo,
                            isActive = @isActive, 
                            categoryId = @categoryId
                            WHERE productId = @productId";
                int rows = connection.Execute(query, product);
                if(rows > 0)
                    return true;
                return false;
            }    
        }

        /// <summary>
        /// Create new a product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public bool Post(Product product)
        {
            if(product == null) return false;
            using (var connection = new SqlConnection(cs))
            {
                using (var db = new SqlConnection(cs))
                {
                    string query = @"
                                INSERT INTO Product
                                (name
                                ,description
                                ,price
                                ,createdDate
                                ,quantity
                                ,type
                                ,photo
                                ,isActive
                                ,categoryId)
                                VALUES (
                                 @name
                                ,@description
                                ,@price
                                ,@createdDate
                                ,@quantity
                                ,@type
                                ,@photo
                                ,@isActive
                                ,@categoryId);
                                SELECT CAST(SCOPE_IDENTITY() as int)";
                    int rows = connection.ExecuteScalar<int>(query, product);
                    if(rows > 0)
                        return true;
                    return false;
                }
            }    
        }
        #endregion
    }
}
