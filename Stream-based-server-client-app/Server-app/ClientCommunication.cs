using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server_app
{
    internal class ClientCommunication
    {
        protected StreamWriter w;
        protected StreamReader r;
        protected string connectedClient;
        public TcpClient Client;

        public ClientCommunication(TcpClient client)
        {
            this.w = new StreamWriter(client.GetStream(), Encoding.UTF8);
            this.r = new StreamReader(client.GetStream(), Encoding.UTF8);
            this.connectedClient = client.Client.RemoteEndPoint.ToString();
            this.Client = client;
        }

        public void StartCommunication()
        {
            Console.WriteLine("Client connected: {0}", connectedClient);

            w.WriteLine("Stream Server v1.0");
            w.Flush();

            bool end = false;
            bool deleteThread = true;

            try
            {
                while (!end)
                {
                    ReaderFromStream rs = new ReaderFromStream();
                    string incoming = rs.ReadLine(r, 10000);

                    if (incoming == null)
                    {
                        Console.WriteLine(connectedClient + ": TIMEOUT!");
                        end = true;
                    }
                    else
                    {
                        Console.Write("Connected clients: {0}\t", Server_app.Program.runningThreads.Count);
                        Console.WriteLine(connectedClient + ": " + incoming);

                        if (incoming == "disconnect")
                            end = true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                if (e is ThreadAbortException)
                    deleteThread = false;
            }

            if (deleteThread)
            {
                lock (Server_app.Program.runningThreads)
                {
                    Thread thisThread = Thread.CurrentThread;
                    Tuple<ClientCommunication, Thread> tp = new Tuple<ClientCommunication, Thread>(this, thisThread);
                    int i = Server_app.Program.runningThreads.IndexOf(tp);
                    if (i != -1) Server_app.Program.runningThreads.RemoveAt(i);
                }
            }

            Console.Write("Connected clients: {0}\t", Server_app.Program.runningThreads.Count);
            Console.WriteLine("Client disconnected!");
        }   
    }
}
