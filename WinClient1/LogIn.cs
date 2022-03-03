using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinClient1
{
    public partial class LogIn : Form
    {
        public string HostName { get; set; } = "localhost";
        public int HostPort { get; set; } = 55055;

        private readonly ShoppingServerHandler m_session;
        public LogIn(ShoppingServerHandler session)
        {
            InitializeComponent();
            m_session = session;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            TcpClient tcpClient = new TcpClient();
            try
            {
                btnConnect.Enabled = txtHostName.Enabled = txtAccountNo.Enabled = false;
                if (!m_session.IsClosed && txtHostName.Text != m_session.HostName)
                    m_session.Exit();
                if (m_session.IsClosed)
                {
                    m_session.HostName = txtHostName.Text;
                    if (m_session.Start(txtAccountNo.Text))
                        this.DialogResult = DialogResult.OK;
                    else
                        MessageBox.Show("Account number is incorrect!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                btnConnect.Enabled = txtHostName.Enabled = true;
                this.Close();
            }
            catch (InvalidOperationException)
            {
                if (!m_session.IsClosed)
                    MessageBox.Show("Already Connected", "Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("Server Unavailable", "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}
