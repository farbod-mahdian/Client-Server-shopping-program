using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WinClient1
{
    public class ShoppingServerHandler
    {
        public string HostName { get; set; } = "localhost";
        public int HostPort { get; set; } = 55055;

        private TcpClient m_tcpClient = null;
        private StreamReader m_reader;
        private StreamWriter m_writer;
        public bool IsClosed => null == m_tcpClient;

        public bool Start(string accountNo)
        {
            try
            {
                m_tcpClient = new TcpClient();
                m_tcpClient.Connect(HostName, HostPort);
                NetworkStream stream = m_tcpClient.GetStream();
                m_reader = new StreamReader(stream);
                m_writer = new StreamWriter(stream);


                m_writer.WriteLine($"CONNECT:{accountNo}");
                m_writer.Flush();

                string line = m_reader.ReadLine();

                if (line.StartsWith("CONNECTED"))
                {
                    return true;
                }
                return false;
            }
            catch (SocketException se)
            {
                m_tcpClient = null;
                throw new InvalidOperationException("Server Unavailable", se);
            }
        }

        private void Close()
        {
            m_tcpClient?.Close();
            m_tcpClient = null;
        }

        public void Exit()
        {
            if (!IsClosed)
            {
                try
                {
                    m_writer.WriteLine("DISCONNECT");
                    m_writer.Flush();
                }
                catch (IOException)
                {
                    // Do nothing
                }
                finally
                {
                    Close();
                }
            }
        }

        public async Task<List<string>> getProductsAsync()
        {
            List<string> products = new List<string>();
            await m_writer.WriteLineAsync("GET_PRODUCTS");
            await m_writer.FlushAsync();

            string line = await m_reader.ReadLineAsync();

            foreach (string product in line.Substring(line.IndexOf(':') + 1).Split('|'))
                products.Add(product.Split(',')[0]);

            return products;
        }

        public async Task<List<string>> getOrdersAsync()
        {
            List<string> orders = new List<string>();

            await m_writer.WriteLineAsync("GET_ORDERS");
            await m_writer.FlushAsync();
            string line = await m_reader.ReadLineAsync();

            foreach (string order in line.Substring(line.IndexOf(':') + 1).Split('|'))
                orders.Add(order);

            return orders;
        }

        public async Task<string> purchaseAsync(string productName)
        {
            await m_writer.WriteLineAsync($"PURCHASE:{productName}");
            await m_writer.FlushAsync();
            return await m_reader.ReadLineAsync();
        }
    }
}
