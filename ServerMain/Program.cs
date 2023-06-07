using SocketLibrary;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        // Tạo địa chỉ IP và cổng lắng nghe
        IPAddress ipAddress = IPAddress.Parse(SocketLib.ServerAddress);
        int port = SocketLib.port;

        // Tạo Socket server và liên kết địa chỉ IP và cổng lắng nghe
        Socket serverSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(new IPEndPoint(ipAddress, port));
        serverSocket.Listen(10);

        Console.WriteLine("Socket server started. Listening on {0}:{1}", ipAddress, port);

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
                    switch(commnad)
                    {
                        case "sell":
                            sk.SendMsg("your sell successfully");
                            conn = false;
                            break;
                        default:
                            sk.SendMsg("your command is fail");
                            conn = false;
                            break;

                    }
                }
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }).Start();
        }

        // Đóng kết nối
    }
}
