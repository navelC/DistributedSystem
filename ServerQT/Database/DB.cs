using System.Data.Common;
using System.Data.SqlClient;

namespace ServerQT.Database
{
    public class DB
    {
        string connStr = "Data Source=DESKTOP-43JS6AD\\MSSQLSERVER02;Initial Catalog=laptop;Integrated Security=True";
        public SqlConnection connection;
        private static DB? instance = null;
        public static DB getInstance()
        {
            if(instance == null)
                instance = new DB();
            return instance;
        }
        private DB()
        {
            connection = new SqlConnection(connStr);

        }
        public SqlDataReader Query(SqlCommand command)
        {
            command.Connection = connection;
            var reader = command.ExecuteReader();
            return reader;

        }
        public int NonQuery(SqlCommand command)
        {
            command.Connection = connection;
            var reader = command.ExecuteNonQuery();
            return reader;

        }
        public SqlDataReader callProcedure(SqlCommand command)
        {
            command.Connection = connection;
            command.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataReader reader =  command.ExecuteReader();
            return reader;
        }
    }
}
