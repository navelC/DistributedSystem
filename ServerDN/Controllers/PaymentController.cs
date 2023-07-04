using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocketLibrary;
using System.Net.Sockets;
using System.Net;

namespace ServerDN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            int quantity = Int32.Parse(HttpContext.Request.Query["quantity"]);
            IPAddress ipAddress = IPAddress.Parse(SocketLib.ServerAddress);
            int port = SocketLib.port;

            // Tạo Socket client và kết nối đến server
            Socket clientSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(new IPEndPoint(ipAddress, port));
            SocketLib sk = new SocketLib(clientSocket);
            sk.SendMsg("Order," + id + ',' + quantity);
            string msg = sk.ReceiveMsg();
            Console.WriteLine(msg);
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            return Ok(msg);
        }

    }
}
