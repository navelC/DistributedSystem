using ServerDN.Models;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace ServerDN.Database
{
    public class CategoryService
    {
        public static void Create(CategoryModel category)
        {
            DB.getInstance().connection.Open();
            using var command = new SqlCommand();
            string queryString = @"INSERT INTO Category (categoryName) VALUES (@name);";
            command.CommandText = queryString;
            command.Parameters.AddWithValue("@name", category.Name);
            DB.getInstance().NonQuery(command);
            DB.getInstance().connection.Close();
        }
        public static ArrayList Get()
        {
            DB.getInstance().connection.Open();
            var command = new SqlCommand();
            string queryString = @"Select * from Category;";
            command.CommandText = queryString;
            using var reader = DB.getInstance().Query(command);
            ArrayList categories= new ArrayList();
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var name = reader.GetString(1);
                categories.Add(new CategoryModel { Id = id, Name = name });
            }
            DB.getInstance().connection.Close();
            return categories;
        }
        public static void Delete(int id)
        {
            DB.getInstance().connection.Open();
            using var command = new SqlCommand();
            string queryString = @"Delete category where id = @id";
            command.CommandText = queryString;
            command.Parameters.AddWithValue("@id", id);
            DB.getInstance().NonQuery(command);
            DB.getInstance().connection.Close();
        }
    }
}
