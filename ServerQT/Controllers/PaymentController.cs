using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocketLibrary;
using System.Net.Sockets;
using System.Net;

namespace ServerQT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            IPAddress ipAddress = IPAddress.Parse(SocketLib.ServerAddress);
            int port = SocketLib.port;

            // Tạo Socket client và kết nối đến server
            Socket clientSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(new IPEndPoint(ipAddress, port));
            SocketLib sk = new SocketLib(clientSocket);
            sk.SendMsg("sell");
            return sk.ReceiveMsg();
        }
    }
}
