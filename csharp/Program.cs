using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        var server = "war.pianka.io";
        var port = 6112;
        var username = "username";
        var password = "password";
        var home = "portal";

        using (var socket = Connect(server, port))
        {
            Login(socket, username, password, home);

            Loop(socket);

            Disconnect(socket);
        }
    }

    static TcpClient Connect(string server, int port)
    {
        return new TcpClient(server, port);
    }

    static void Disconnect(TcpClient client)
    {
        client.Close();
    }

    static void Login(TcpClient client, string username, string password, string home)
    {
        Send(client, "C1");
        Send(client, "ACCT " + username);
        Send(client, "PASS " + password);
        Send(client, "HOME " + home);
        Send(client, "LOGIN");
    }

    static void Send(TcpClient client, string data)
    {
        var stream = client.GetStream();
        var buffer = Encoding.UTF8.GetBytes(data + "\r\n");
        stream.Write(buffer, 0, buffer.Length);
    }

    static void Loop(TcpClient client)
    {
        var stream = client.GetStream();
        using (var reader = new StreamReader(stream, Encoding.UTF8))
        {
            while (true)
            {
                var line = reader.ReadLine()?.Trim();
                if (line == null) break;
                
                if (line.StartsWith("CHANNEL JOIN")){
                    Send(client, "Hello, Warnet!");
                }
                
            }
        }
    }
}
