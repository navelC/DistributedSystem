using SocketLibrary;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Program
{
    static Queue<Support> queue = new Queue<Support>();
    static string connStr = "Data Source=DESKTOP-43JS6AD\\MSSQLSERVER01;Initial Catalog=laptop;Integrated Security=True";
    static  SqlConnection connection;
    class Support
    {
        public int quantity;
        public int id;
        public SocketLib socket;
    }
    static void Main(string[] args)
    {
        connection = new SqlConnection(connStr);
        // Tạo địa chỉ IP và cổng lắng nghe
        IPAddress ipAddress = IPAddress.Parse(SocketLib.ServerAddress);
        int port = SocketLib.port;

        // Tạo Socket server và liên kết địa chỉ IP và cổng lắng nghe
        Socket serverSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(new IPEndPoint(ipAddress, port));
        serverSocket.Listen(10);

        Console.WriteLine("Socket server started. Listening on {0}:{1}", ipAddress, port);
        new Thread(() => {
            while (true) { 
                Thread.Sleep(100);
                ExcuteQueue();
            }
        }).Start();
        while (true)
        {
            // Chấp nhận kết nối từ client
            Socket clientSocket = serverSocket.Accept();
            Console.WriteLine("Client connected: {0}", clientSocket.RemoteEndPoint);

            new Thread(() =>
            {
                SocketLib sk = new SocketLib(clientSocket);
                bool conn = true;
                while(conn)
                {
                    string commnad = sk.ReceiveMsg();
                    string[] arr = commnad.Split(',');
                    switch(arr[0])
                    {
                        case "Order":
                            queue.Enqueue(new Support { socket = sk, id= Int32.Parse(arr[1]), quantity= Int32.Parse(arr[2]) });
                            Console.WriteLine(arr[0]);
                            conn = false;
                            break;
                        default:
                            sk.SendMsg("your command is fail");
                            conn = false;
                            break;
                    }
                }
            }).Start();
        }
        static void ExcuteQueue()
        {
            while (queue.Count > 0)
            {
                Support sp = queue.Dequeue();
                connection.Open();
                using var command = new SqlCommand();
                string queryString = "pr_reduceQuantity";
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = queryString;
                command.Parameters.Add(
                    new SqlParameter()
                    {
                        ParameterName = "@idProduct",
                        SqlDbType = SqlDbType.Int,
                        Value = sp.id
                    }
                );
                command.Parameters.Add(
                    new SqlParameter()
                    {
                        ParameterName = "@quantity",
                        SqlDbType = SqlDbType.Int,
                        Value = sp.quantity
                    }
                );
                SqlDataReader reader = command.ExecuteReader();
                int rs = reader.RecordsAffected;
                connection.Close();
                sp.socket.SendMsg(rs.ToString());

            }
        }
        // Đóng kết nối
    }
}
