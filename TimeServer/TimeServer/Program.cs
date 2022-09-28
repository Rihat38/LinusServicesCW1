using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TimeServer;

static class Program
{
    static int port = 1303;

    static void Main()
    {
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
        
        Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            listenSocket.Bind(ipPoint);

            listenSocket.Listen(10);

            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            while (true)
            {
                Socket handler = listenSocket.Accept();
                StringBuilder builder = new StringBuilder();
                int bytes; 
                byte[] data = new byte[256]; 

                do
                {
                    bytes = handler.Receive(data);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                } while (handler.Available > 0);

                Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());

                string message = DateTime.Now.ToString("g");
                data = Encoding.Unicode.GetBytes(message);
                handler.Send(data);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}