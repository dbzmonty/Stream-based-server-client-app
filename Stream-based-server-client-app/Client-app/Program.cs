using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Sockets;
using System.IO;

namespace Client_app
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
                TcpClient client = new TcpClient();
                client.Connect(ip, port);

                StreamReader r = new StreamReader(client.GetStream(), Encoding.UTF8);
                string info = r.ReadLine();
                Console.WriteLine(info);

                StreamWriter w = new StreamWriter(client.GetStream(), Encoding.UTF8);
                w.WriteLine("Hello, this is a client!");
                w.Flush();

                string input = String.Empty;
                while (input != "disconnect")
                {
                    input = Console.ReadLine();
                    w.WriteLine(input);
                    w.Flush();
                }

                Console.WriteLine("Client quit!");
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
