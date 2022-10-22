using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Server_app
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string serverIpAddress = ConfigurationManager.AppSettings["ServerIpAddress"];
            string ServerPort = ConfigurationManager.AppSettings["ServerPort"];
            IPAddress ip = IPAddress.Parse(serverIpAddress);
            int port = int.Parse(ServerPort);

            try
            {
                TcpListener listener = new TcpListener(ip, port);
                listener.Start();

                Console.WriteLine("Server online, waiting for connection!");
                TcpClient client = listener.AcceptTcpClient();

                string connectedClient = client.Client.RemoteEndPoint.ToString();
                Console.WriteLine("Client connected: {0}", connectedClient);

                StreamWriter w = new StreamWriter(client.GetStream(), Encoding.UTF8);
                w.WriteLine("Stream Server v1.0");
                w.Flush();

                StreamReader r = new StreamReader(client.GetStream(), Encoding.UTF8);
                string incoming = r.ReadLine();
                Console.WriteLine(incoming);

                Console.WriteLine("Server quit!");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }




        }
    }
}
