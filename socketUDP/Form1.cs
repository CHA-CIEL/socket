// Joris chaintron

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace socketUDP
{
    public partial class Form1 : Form
    {
        private Socket sock;
        private IPEndPoint ipeR;
        private IPEndPoint ipeD;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCreateBind_Click(object sender, EventArgs e)
        {
            var recvIP = IPAddress.Parse(txtRecvIP.Text);
            var recvPort = int.Parse(txtRecvPort.Text);
            ipeR = new IPEndPoint(recvIP, recvPort);
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sock.Bind(ipeR);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (sock != null)
            {
                sock.Close();
                sock = null;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (sock == null) return;
            var destIP = IPAddress.Parse(txtDestIP.Text);
            var destPort = int.Parse(txtDestPort.Text);
            ipeD = new IPEndPoint(destIP, destPort);
            var msg = Encoding.ASCII.GetBytes(txtSend.Text);
            sock.SendTo(msg, ipeD);
        }

        private void btnReceive_Click(object sender, EventArgs e)
        {
            if (sock == null) return;
            var buffer = new byte[1024];
            EndPoint from = new IPEndPoint(IPAddress.Any, 0);
            sock.ReceiveTimeout = 2000;
            try
            {
                int len = sock.ReceiveFrom(buffer, ref from);
                var text = Encoding.ASCII.GetString(buffer, 0, len);
                lstRecv.Items.Add(text);
            }
            catch (SocketException)
            {
            }
        }

        private void btnCls_Click(object sender, EventArgs e)
        {
            lstRecv.Items.Clear();
        }
    }
}
