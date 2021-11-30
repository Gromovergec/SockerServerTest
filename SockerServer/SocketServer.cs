using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SockerServerTCP
{
    /// <summary>
    /// Init server
    /// </summary>
    internal class SocketServer
    {
        private readonly int _port;
        private readonly ConcurrentDictionary<IPEndPoint, int> _dict;
        private TcpListener _listner;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port">Listen port, default 8005</param>
        public SocketServer(int port = 8005)
        {
            _port = port;
            _dict = new ConcurrentDictionary<IPEndPoint, int>();
        }

        public void Start()
        {
            try
            {
                _listner = new TcpListener(IPAddress.Any, _port);
                _listner.Start();
                Console.WriteLine("Server start");
                while (true)
                {
                    TcpClient client = _listner.AcceptTcpClient();
                    var clientSocket = new ClientSocket(client, _dict);
                    Thread clientThread = new Thread(new ThreadStart(clientSocket.Handler));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if(_listner != null)
                {
                    _listner.Stop();
                }
            }
        }
    }
}
