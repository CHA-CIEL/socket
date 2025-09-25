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
using System.Threading;

namespace socketUDP
{
    public partial class Form1 : Form
    {
        private Socket sock;
        private Socket SSockUDP;
        private IPEndPoint ipeR;
        private IPEndPoint ipeD;
        private CancellationTokenSource recvCts;
        private SocketAsyncEventArgs recvArgs;
        private byte[] recvBuffer;
        private bool recvActive;
        public Form1()
        {
            InitializeComponent();
            UpdateControlsEnabled(false);
        }

        private void btnCreateBind_Click(object sender, EventArgs e)
        {
            var recvIP = IPAddress.Parse(txtRecvIP.Text);
            var recvPort = int.Parse(txtRecvPort.Text);
            ipeR = new IPEndPoint(recvIP, recvPort);
            if (sock != null)
            {
                StopReceiveLoop();
                try { sock.Close(); } catch { }
                sock = null;
            }
            try
            {
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                sock.ExclusiveAddressUse = false;
                sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                sock.Bind(ipeR);
                labelIpeR.Text = $"IPeR {sock.LocalEndPoint}";
                StartReceiveLoop();
                UpdateControlsEnabled(true);
            }
            catch (SocketException ex)
            {
                MessageBox.Show($"Bind échoué sur {ipeR}: {ex.Message}\n\nConseils:\n- Vérifiez que le port n'est pas déjà utilisé par un autre processus.\n- Essayez un autre port.\n- Vous pouvez lancer 2 instances: chacune doit binder sur un port différent.",
                                "Erreur Bind UDP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateControlsEnabled(false);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (sock != null)
            {
                StopReceiveLoop();
                try { sock.Close(); } catch { }
                sock = null;
            }

            if (this.SSockUDP != null)
            {
                try { this.SSockUDP.Shutdown(SocketShutdown.Both); } catch { }
                try { this.SSockUDP.Close(); } catch { }
                this.SSockUDP = null;
            }
            UpdateControlsEnabled(false);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (sock == null) return;
            var destIP = IPAddress.Parse(txtDestIP.Text);
            var destPort = int.Parse(txtDestPort.Text);
            ipeD = new IPEndPoint(destIP, destPort);
            var msg = Encoding.ASCII.GetBytes(txtSend.Text);
            sock.SendTo(msg, ipeD);
            labelIpeD.Text = $"IPeD {ipeD}";

            try
            {
                var local = sock.LocalEndPoint as IPEndPoint;
                if (local != null && local.Address.Equals(destIP) && local.Port == destPort)
                {
                    var buffer = new byte[2048];
                    EndPoint from = new IPEndPoint(IPAddress.Any, 0);
                    while (sock != null && sock.Poll(0, SelectMode.SelectRead) && sock.Available > 0)
                    {
                        int len = sock.ReceiveFrom(buffer, ref from);
                        var text = Encoding.ASCII.GetString(buffer, 0, len);
                        txtRecv.AppendText(text + Environment.NewLine);
                    }
                }
            }
            catch { }
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
                txtRecv.AppendText(text + Environment.NewLine);
            }
            catch (SocketException)
            {
            }
        }

        private void btnCls_Click(object sender, EventArgs e)
        {
            txtRecv.Clear();
        }

        private void UpdateControlsEnabled(bool socketOpen)
        {
            btnCreateBind.Enabled = !socketOpen;
            btnSend.Enabled = socketOpen;
            btnReceive.Enabled = socketOpen;
            btnClose.Enabled = socketOpen;
            btnCls.Enabled = socketOpen;
            txtRecvIP.Enabled = !socketOpen;
            txtRecvPort.Enabled = !socketOpen;
        }

        private void StartReceiveLoop()
        {
            StopReceiveLoop(); 
            if (sock == null) return;

            recvBuffer = new byte[4096];
            recvArgs = new SocketAsyncEventArgs();
            recvArgs.RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            recvArgs.SetBuffer(recvBuffer, 0, recvBuffer.Length);
            recvArgs.Completed += RecvArgs_Completed;
            recvActive = true;

            bool pending;
            try
            {
                pending = sock.ReceiveFromAsync(recvArgs);
            }
            catch (ObjectDisposedException) { return; }
            catch (SocketException) { return; }

            if (!pending)
            {
                RecvArgs_Completed(sock, recvArgs);
            }
        }

        private void RecvArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (!recvActive) return;
            if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
            {
                var text = Encoding.ASCII.GetString(e.Buffer, e.Offset, e.BytesTransferred);
                try
                {
                    this.BeginInvoke((Action)(() =>
                    {
                        txtRecv.AppendText(text + Environment.NewLine);
                    }));
                }
                catch { }
            }

            try
            {
                e.RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                e.SetBuffer(0, recvBuffer.Length);
                bool pending = sock != null && sock.ReceiveFromAsync(e);
                if (!pending)
                {
                    RecvArgs_Completed(sock, e);
                }
            }
            catch
            {
                recvActive = false;
            }
        }

        private void StopReceiveLoop()
        {
            recvActive = false;
            try
            {
                if (recvArgs != null)
                {
                    recvArgs.Completed -= RecvArgs_Completed;
                    recvArgs.Dispose();
                    recvArgs = null;
                }
            }
            catch { }
        }
    }
}
