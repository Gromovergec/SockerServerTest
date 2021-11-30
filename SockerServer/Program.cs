using System;
using System.Linq;

namespace SockerServerTCP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SocketServer sockSrv;
            var port = 0;
            if (args.Length != 0 || int.TryParse(args.FirstOrDefault(), out port))
            {
                sockSrv = new SocketServer(port);
            } else
            {
                sockSrv = new SocketServer();
            }
            sockSrv.Start();
        }
    }
}
