using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConServer
{
    public class ShoppingClientHandler
    {
        private static object m_lock = new object();
        private readonly Dictionary<string, string[]> m_accounts;
        private Dictionary<string, int> m_products;
        private List<string[]> m_orders;
        private string m_accountNo;
        private string m_userName;

        private readonly TcpClient m_tcpClient;
        private readonly CancellationToken m_cancellationToken;

        public ShoppingClientHandler(object lock_, Dictionary<string, string[]> accounts, Dictionary<string, int> products, List<string[]> orders, TcpClient tcpClient, CancellationToken cancellationToken)
        {
            m_lock = lock_;
            m_accounts = accounts;
            m_products = products;
            m_tcpClient = tcpClient;
            m_orders = orders;
            m_cancellationToken = cancellationToken;
        }

        public void Run()
        {

            using (m_tcpClient)
            {
                try
                {
                    NetworkStream stream = m_tcpClient.GetStream();
                    StreamReader reader = new StreamReader(stream);
                    StreamWriter writer = new StreamWriter(stream);

                    m_cancellationToken.Register(() =>
                    {
                        Thread.Sleep(100);
                        stream.Close();
                    });

                    writer.AutoFlush = true;

                    while (!m_cancellationToken.IsCancellationRequested)
                    {
                        string line = reader.ReadLine();
                        if ("DISCONNECT" == line)
                        {
                            if (m_accountNo != null)
                                m_accounts[m_accountNo][1] = "DOWN";
                            Console.WriteLine("DISCCONECTED");
                            break;
                        }
                        else if (line.StartsWith("CONNECT"))
                        {
                            string accountNo = line.Substring(line.IndexOf(':') + 1);
                            if (accountIsFound(accountNo))
                            {
                                Console.WriteLine($"CONNECTED:{m_accounts[accountNo][0]}");
                                writer.WriteLine($"CONNECTED:{m_accounts[accountNo][0]}");
                                m_accountNo = accountNo;
                                m_userName = m_accounts[accountNo][0];
                            }
                            else
                            {
                                Console.WriteLine("CONNECT_ERROR");
                                writer.WriteLine("CONNECT_ERROR");
                            }
                        }
                        else if ("GET_PRODUCTS" == line)
                        {
                            Console.WriteLine("GET_PRODUCTS:" + string.Join("|", from product in m_products select product into handler select $"{handler.Key},{handler.Value}"));
                            writer.WriteLine("GET_PRODUCTS:" + string.Join("|", from product in m_products select product into handler select $"{handler.Key},{handler.Value}"));
                        }
                        else if ("GET_ORDERS" == line)
                        {
                            Console.WriteLine("GET_ORDERS:" + string.Join("|", from order in m_orders select order into handler select $"{handler[0]},{handler[1]},{handler[2]}"));
                            writer.WriteLine("GET_ORDERS:" + string.Join("|", from order in m_orders select order into handler select $"{handler[0]},{handler[1]},{handler[2]}"));
                        }
                        else if (line.StartsWith("PURCHASE"))
                        {
                            string productName = line.Substring(line.IndexOf(':') + 1);

                            if (m_products.ContainsKey(productName))
                            {
                                if (m_products[productName] > 0)
                                {
                                    Monitor.Enter(m_lock);
                                    m_orders.Add(new string[] { productName, "1", m_userName });
                                    m_products[productName]--;
                                    Monitor.Exit(m_lock);

                                    Console.WriteLine("DONE");
                                    writer.WriteLine("DONE");
                                }
                                else
                                {
                                    Console.WriteLine("NOT_AVAILABLE");
                                    writer.WriteLine("NOT_AVAILABLE");
                                }
                            }
                            else
                            {
                                Console.WriteLine("NOT_VALID");
                                writer.WriteLine("NOT_VALID");
                            }
                        }
                    }
                }
                catch (IOException)
                {
                    Console.WriteLine("***Network Error***");
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("***Network Error***");
                }
            }
        }

        bool accountIsFound(string accountNo)
        {
            foreach (string key in m_accounts.Keys)
            {
                if (accountNo == key)
                {
                    if (m_accounts[key][1] == "DOWN")
                    {
                        Monitor.Enter(m_lock);
                        m_accounts[key][1] = "UP";
                        Monitor.Exit(m_lock);
                        return true;
                    }
                    else
                        break;
                }
            }
            return false;
        }
    }
}
