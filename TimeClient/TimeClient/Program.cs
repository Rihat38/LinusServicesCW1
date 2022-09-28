using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TimeClient;

public static class Program
{
    static int port = 1303;
    static string address = "127.0.0.1";
    static void Main()
    {
        try
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
 
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(ipPoint);
 
            var data = new byte[256];
            var builder = new StringBuilder();
            int bytes;
            do
            {
                bytes = socket.Receive(data, data.Length, 0);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (socket.Available > 0);
            Console.WriteLine("ответ сервера: " + builder.ToString());
 
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        Console.Read();
    }
}