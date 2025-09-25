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
                txtRecv.AppendText($"[OK] Bind sur {sock.LocalEndPoint}\r\n");
            }
            catch (SocketException ex)
            {
                MessageBox.Show($"Bind échoué sur {ipeR}: {ex.Message}\n\nConseils:\n- Vérifiez que le port n'est pas déjà utilisé par un autre processus.\n- Essayez un autre port.\n- Vous pouvez lancer 2 instances: chacune doit binder sur un port différent.",
                                "Erreur Bind UDP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateControlsEnabled(false);
                txtRecv.AppendText($"[ERREUR] Bind: {ex.Message}\r\n");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (sock != null)
            {
                StopReceiveLoop();
                try { sock.Close(); } catch (SocketException ex) { txtRecv.AppendText($"[ERREUR] Fermeture socket: {ex.Message}\r\n"); }
                sock = null;
                txtRecv.AppendText("[OK] Socket fermée.\r\n");
            }

            if (this.SSockUDP != null)
            {
                try { this.SSockUDP.Shutdown(SocketShutdown.Both); } catch { }
                try { this.SSockUDP.Close(); } catch { }
                this.SSockUDP = null;
            }
            UpdateControlsEnabled(false);
            if (sock == null)
            {
                txtRecv.AppendText("[INFO] Aucune socket active.\r\n");
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (sock == null)
            {
                txtRecv.AppendText("[ERREUR] Envoi impossible: socket non créée. Cliquez sur 'Créer Socket et Bind'.\r\n");
                return;
            }
            var destIP = IPAddress.Parse(txtDestIP.Text);
            var destPort = int.Parse(txtDestPort.Text);
            ipeD = new IPEndPoint(destIP, destPort);
            var msg = Encoding.ASCII.GetBytes(txtSend.Text);
            try
            {
                var local = sock.LocalEndPoint as IPEndPoint;
                if (local != null && local.Address.Equals(destIP) && local.Port == destPort)
                {
                    txtRecv.AppendText($"[INFO] Envoi vers votre propre endpoint {local}.\r\n");
                }

                sock.SendTo(msg, ipeD);
                labelIpeD.Text = $"IPeD {ipeD}";
                txtRecv.AppendText($"[OK] Envoyé {msg.Length} octets à {ipeD}.\r\n");
            }
            catch (SocketException se)
            {
                txtRecv.AppendText($"[ERREUR] Envoi: {se.Message}\r\n");
                return;
            }

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
            if (sock == null)
            {
                txtRecv.AppendText("[ERREUR] Réception impossible: socket non créée. Cliquez sur 'Créer Socket et Bind'.\r\n");
                return;
            }
            var buffer = new byte[1024];
            EndPoint from = new IPEndPoint(IPAddress.Any, 0);
            sock.ReceiveTimeout = 2000;
            try
            {
                int len = sock.ReceiveFrom(buffer, ref from);
                var text = Encoding.ASCII.GetString(buffer, 0, len);
                txtRecv.AppendText(text + Environment.NewLine);
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.TimedOut)
                    txtRecv.AppendText("[INFO] Réception: aucun message (timeout).\r\n");
                else
                    txtRecv.AppendText($"[ERREUR] Réception: {ex.Message}\r\n");
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
