using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Runtime.CompilerServices;

namespace Server_app
{
    internal class Program
    {
        static TcpListener listener = null;
        static Thread connections = null;
        public static List<Tuple<ClientCommunication, Thread>> runningThreads = new List<Tuple<ClientCommunication, Thread>>();

        static void AcceptConnection()
        {
            while (true)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    ClientCommunication cc = new ClientCommunication(client);
                    Thread t = new Thread(cc.StartCommunication);
                    lock (runningThreads)
                    {
                        runningThreads.Add(new Tuple<ClientCommunication, Thread>(cc, t));
                    }
                    t.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        static void Main(string[] args)
        {
            string serverIpAddress = ConfigurationManager.AppSettings["ServerIpAddress"];
            string ServerPort = ConfigurationManager.AppSettings["ServerPort"];
            IPAddress ip = IPAddress.Parse(serverIpAddress);
            int port = int.Parse(ServerPort);

            try
            {
                listener = new TcpListener(ip, port);
                listener.Start();

                connections = new Thread(AcceptConnection);
                connections.Start();

                Console.WriteLine("Server online, waiting for connection!");

                Console.ReadLine();
                
                lock (runningThreads)
                {
                    foreach (Tuple<ClientCommunication, Thread> rt in runningThreads)
                    {
                        try
                        {
                            rt.Item2.Abort();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }

                listener.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }
    }
}
