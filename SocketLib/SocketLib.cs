using System.Net.Sockets;
using System.Text;

namespace SocketLibrary
{
    public class SocketLib
    {
        Socket socket;
        public static string ServerAddress = "127.0.0.1";
        public static int port = 8888;
        public SocketLib(Socket socket)
        {
            this.socket = socket;
        }
        public string ReceiveMsg()
        {
            byte[] buffer = new byte[1024];
            int bytesRead = socket.Receive(buffer);
            return Encoding.ASCII.GetString(buffer, 0, bytesRead);
        }
        public void SendMsg(string msg)
        {
            byte[] data = Encoding.ASCII.GetBytes(msg);
            socket.Send(data);
        }
    }
}