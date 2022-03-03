using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinClient1
{
    public partial class Form1 : Form
    {
        private readonly ShoppingServerHandler m_session;
        public Form1(ShoppingServerHandler session)
        {
            InitializeComponent();
            m_session = session;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            if (Connect())
                tmrUpdate.Enabled = true;
            else
                Application.Exit();

            foreach (string product in await m_session.getProductsAsync())
                comboProducts.Items.Add(product);
        }
        private bool Connect() => new LogIn(m_session).ShowDialog(this) == DialogResult.OK;

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_session.Exit();
        }

        private async void btnBuy_Click(object sender, EventArgs e)
        {
            string reply_ = await m_session.purchaseAsync(comboProducts.Text);

            if ("NOT_AVAILABLE" == reply_)
            {
                MessageBox.Show("The product is not available (i.e., is already purchased by another client) and cannot be purchase.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if ("NOT_VALID" == reply_)
                MessageBox.Show("The specified product is not valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            listOrders.Items.Clear();

            foreach (string order in await m_session.getOrdersAsync())
                listOrders.Items.Add(order);
        }

        private async void tmrUpdate_Tick(object sender, EventArgs e)
        {
            try
            {
                tmrUpdate.Enabled = false;

                listOrders.Items.Clear();

                foreach (string order in await m_session.getOrdersAsync())
                    listOrders.Items.Add(order);

                tmrUpdate.Enabled = true;
            }
            catch (InvalidOperationException)
            {
                tmrUpdate.Enabled = false;
                MessageBox.Show("Server Unavailable", "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("Server Unavailable", "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
    }
}
