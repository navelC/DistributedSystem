using ServerDN.Models;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace ServerDN.Database
{
    public class ProductService
    {
        public static void Create(ProductModel productModel)
        {
            DB.getInstance().connection.Open();
            using var command = new SqlCommand();
            string queryString = @"INSERT INTO Product (productName, categoryID, quantity, price, img, idBranch) VALUES (@name, @category, @quantity, @price, @img, @branch);";
            command.CommandText = queryString;
            command.Parameters.AddWithValue("@name", productModel.ProductName);
            command.Parameters.AddWithValue("@category", productModel.CategoryID);
            command.Parameters.AddWithValue("@quantity", productModel.Quantity);
            command.Parameters.AddWithValue("@price", productModel.Price);
            command.Parameters.AddWithValue("@img", productModel.File.FileName);
            command.Parameters.AddWithValue("@branch", productModel.BranchID);
            DB.getInstance().NonQuery(command);
            DB.getInstance().connection.Close();
        }
        public static ArrayList Get()
        {   
            DB.getInstance().connection.Open();
            var command = new SqlCommand();
            string queryString = @"Select * from Link.laptop.dbo.product union select * from product;";
            command.CommandText = queryString;
            using var reader = DB.getInstance().Query(command);
            ArrayList products = new ArrayList();
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var name = reader.GetString(1);
                var category = reader.GetInt32(2);
                var quantity = reader.GetInt32(3);
                var price = reader.GetInt32(4);
                var img = reader.GetString(5);
                var branch = reader.GetInt32(6);
                products.Add(new ProductModel { Id = id, ProductName = name, Path = img,
                BranchID = branch, CategoryID = category,Price = price,Quantity=quantity });
            }
            DB.getInstance().connection.Close();
            return products;
        }
        public static void Delete(int id)
        {
            DB.getInstance().connection.Open();
            using var command = new SqlCommand();
            string queryString = @"Delete Product where id = @id";
            command.CommandText = queryString;
            command.Parameters.AddWithValue("@id", id);
            DB.getInstance().NonQuery(command);
            DB.getInstance().connection.Close();
        }
        public static int Order(int id, int quantity)
        {
            DB.getInstance().connection.Open();
            using var command = new SqlCommand();
            string queryString = "pr_reduceQuantity";
            command.CommandText = queryString;
            command.Parameters.Add(
                new SqlParameter()
                {
                    ParameterName = "@idProduct",
                    SqlDbType = SqlDbType.Int,
                    Value = id
                }
            );
            command.Parameters.Add(
                new SqlParameter()
                {
                    ParameterName = "@quantity",
                    SqlDbType = SqlDbType.Int,
                    Value = quantity
                }
            );
            int rs = DB.getInstance().callProcedure(command).RecordsAffected;
            DB.getInstance().connection.Close();
            return rs;
        }
        public static ProductModel GetById(int id)
        {
            DB.getInstance().connection.Open();
            using var command = new SqlCommand();
            string queryString = "pr_getProduct";
            command.CommandText = queryString;
            command.Parameters.Add(
                new SqlParameter()
                {
                    ParameterName = "@id",
                    SqlDbType = SqlDbType.Int,
                    Value = id
                }
            );
            using SqlDataReader reader = DB.getInstance().callProcedure(command);
            ProductModel? product = null;
            while (reader.Read())
            {
                var idp = reader.GetInt32(0);
                var name = reader.GetString(1);
                var category = reader.GetInt32(2);
                var quantity = reader.GetInt32(3);
                var price = reader.GetInt32(4);
                var img = reader.GetString(5);
                var branch = reader.GetInt32(6);
                product = new ProductModel
                {
                    Id = idp,
                    ProductName = name,
                    Path = img,
                    BranchID = branch,
                    CategoryID = category,
                    Price = price,
                    Quantity = quantity
                };
            }
            DB.getInstance().connection.Close();
            return product;
        }
    }
}
