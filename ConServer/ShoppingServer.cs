using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConServer
{
    public class ShoppingServer
    {
        private static readonly object m_lock = new object();
        private static readonly Random m_rnd = new Random();
        private readonly Dictionary<string, string[]> accounts = new Dictionary<string, string[]>() { { "1111", new string[] { "user1", "DOWN" } }, { "2222", new string[] { "user2", "DOWN" } }, { "3333", new string[] { "user3", "DOWN" } } };
        private Dictionary<string, int> products = new Dictionary<string, int>() { { "apple", m_rnd.Next(1, 4) }, { "orange", m_rnd.Next(1, 4) }, { "bannana", m_rnd.Next(1, 4) }, { "keyboard", m_rnd.Next(1, 4) }, { "phone", m_rnd.Next(1, 4) } };
        private List<string[]> orders = new List<string[]>();

        private readonly CancellationToken m_cancellationToken;
        public IPAddress ServerIp { get; set; } = IPAddress.Any;
        public int ServerPort { get; set; } = 55055;

        public ShoppingServer(CancellationToken cancellationToken) => m_cancellationToken = cancellationToken;

        public void Start(object threadInfo)
        {
            try
            {
                TcpListener listener = new TcpListener(ServerIp, ServerPort);
                listener.Start();
                m_cancellationToken.Register(listener.Stop);

                while (!m_cancellationToken.IsCancellationRequested)
                {
                    TcpClient tcpClient = listener.AcceptTcpClient();
                    ShoppingClientHandler handler = new ShoppingClientHandler(m_lock, accounts, products, orders, tcpClient, m_cancellationToken);
                    Thread thClientHandler = new Thread(handler.Run);
                    thClientHandler.Start();
                }
            }
            catch (SocketException) // Exception takes us out of the loop, server connection handler thread will end
            {
                // SocketException may occur when listener is started or stopped
            }
        }
    }
}
