using Dapper;
using ExcerciseTwo.Models;
using System.Data.SqlClient;

namespace ExcerciseTwo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string cs = @"Data Source=MSI\SQLEXPRESS;Initial Catalog=ProductManagerDB;Integrated Security=True";
            SqlConnection con = new SqlConnection(cs);
            string sql = "select * from test";
            List<Product> list = con.Query<Product>(sql).ToList();
            gridProducts.DataSource = list;
        }
    }
}