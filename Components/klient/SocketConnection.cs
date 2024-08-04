using System.Net.Sockets;
using System.Net;
using System.Text;

namespace UserClient.Components.klient
{
    public class SocketConnection
    {
        private static Socket client;

        public static async Task SendToServer(string username, string password, string site)
        {
            while (true)
            {
                await SendingUser(username, password, site);
                string response = await RecieveMessageFromServer();
                if (!string.IsNullOrEmpty(response))
                {
                    Console.WriteLine($"Socket client received acknowledgment: \"{response}\"");
                    break;
                }
            }

            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        private static async Task<string> RecieveMessageFromServer()
        {
            var buffer = new byte[1_024];
            var received = await client.ReceiveAsync(buffer, SocketFlags.None);
            var response = Encoding.UTF8.GetString(buffer, 0, received);
            return response;
        }

        private static async Task SendingUser(string username, string password,string site)
        {
            var combinedMessage = $"{username}:{password}:{site}";
            var messageBytes = Encoding.UTF8.GetBytes(combinedMessage);
            await client.SendAsync(messageBytes, SocketFlags.None);
            Console.WriteLine($"Socket client sent combined message: \"{combinedMessage}\"");
        }

        public static async Task SocketClientAsync()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1"); 
            IPEndPoint ipEndPoint = new(ipAddress, 8080); 
            client = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            await client.ConnectAsync(ipEndPoint);
        }
    }
}
