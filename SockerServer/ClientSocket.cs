using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SockerServer
{
    internal class ClientSocket
    {
        private TcpClient _tcpClient;
        private ConcurrentDictionary<IPAddress, int> _connectionDic;
        private byte[] buffer = new byte[128];
        public ClientSocket(TcpClient tcpClient, ConcurrentDictionary<IPAddress, int> connectionDic)
        {
            _tcpClient = tcpClient;
            _connectionDic = connectionDic;
        }

        public void Handler()
        {
            var num = 0;
            NetworkStream networkStream = null;
            try
            {
                var greeating = @"Hi user, enter a number for get the sum all entered number";
                buffer = Encoding.UTF8.GetBytes(greeating);
                networkStream = _tcpClient.GetStream();
                networkStream.Write(buffer, 0, buffer.Length);

                while (true)
                {
                    int bytes = 0;
                    do
                    {
                        bytes = networkStream.Read(buffer, 0, buffer.Length);
                        var recive = Encoding.UTF8.GetString(buffer, 0, bytes);
                        if (int.TryParse(recive, out num))
                        {

                        }


                    } while (networkStream.DataAvailable);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (networkStream != null)
                {
                    networkStream.Close();
                }
                if (_tcpClient != null)
                {
                    _tcpClient.Close();
                }
            }
        }

    }
}
