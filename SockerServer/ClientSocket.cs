using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SockerServerTCP
{
    /// <summary>
    /// Client connection obj
    /// </summary>
    internal class ClientSocket
    {
        private readonly ConcurrentDictionary<IPEndPoint, int> _connectionDic;
        private readonly Message _message;
        private TcpClient _tcpClient;
        private IPEndPoint _iPEndPoint;
        private byte[] buffer = new byte[256];

        public ClientSocket(TcpClient tcpClient, ConcurrentDictionary<IPEndPoint, int> connectionDic)
        {
            _tcpClient = tcpClient;
            _connectionDic = connectionDic;
            _message = new Message(_connectionDic);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Handler()
        {
            NetworkStream networkStream = null;
            try
            {
                var greeating = "Hi user, enter a number for get the sum all entered number \r\n";
                buffer = Encoding.UTF8.GetBytes(greeating);
                networkStream = _tcpClient.GetStream();
                networkStream.Write(buffer, 0, buffer.Length);
                _iPEndPoint = ((IPEndPoint)_tcpClient.Client.RemoteEndPoint);

                var receive = string.Empty;
                while (true)
                {
                    var sb = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = networkStream.Read(buffer, 0, buffer.Length);
                        sb.Append(Encoding.UTF8.GetString(buffer,0,bytes));
                    } while (networkStream.DataAvailable);

                    receive = $"{_message.GenerateMessage(_iPEndPoint, sb.ToString())} \r\n";
                    buffer = Encoding.UTF8.GetBytes(receive);
                    networkStream.Write(buffer, 0, buffer.Length);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                //Close stream
                if (networkStream != null)
                {
                    networkStream.Close();
                }
                //Close client
                if (_tcpClient != null)
                {
                    RemoveClient(_iPEndPoint);
                    _tcpClient.Close();
                }
            }
        }
        /// <summary>
        /// Remove client from dictionary
        /// </summary>
        /// <param name="iPEndPoint">Client IP information</param>
        private void RemoveClient(IPEndPoint iPEndPoint)
        {
            _connectionDic.TryRemove(iPEndPoint, out _);
        }
    }
}
