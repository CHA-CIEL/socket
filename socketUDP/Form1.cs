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
        private IPEndPoint ipeR;
        private IPEndPoint ipeD;
        private int receiveTimeoutMs = 10000;
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
                try { sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, receiveTimeoutMs); } catch { }
                UpdateControlsEnabled(true);
                txtRecv.AppendText($"[OK] Bind sur {sock.LocalEndPoint}\r\n");
                txtRecv.AppendText($"[INFO] Timeout réception réglé à {receiveTimeoutMs} ms.\r\n");
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
                try { sock.Close(); } catch (SocketException ex) { txtRecv.AppendText($"[ERREUR] Fermeture socket: {ex.Message}\r\n"); }
                sock = null;
                txtRecv.AppendText("[OK] Socket fermée.\r\n");
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
                sock.SendTo(msg, ipeD);
                labelIpeD.Text = $"IPeD {ipeD}";
                txtRecv.AppendText($"[OK] Envoyé {msg.Length} octets à {ipeD}.\r\n");
                try { sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, receiveTimeoutMs); } catch { }
                txtRecv.AppendText($"[INFO] Timeout réception réglé à {receiveTimeoutMs} ms.\r\n");
            }
            catch (SocketException se)
            {
                txtRecv.AppendText($"[ERREUR] Envoi: {se.Message}\r\n");
                return;
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

            byte[] buffer = new byte[2048];
            EndPoint from = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                int len = sock.ReceiveFrom(buffer, ref from);
                string text = Encoding.ASCII.GetString(buffer, 0, len);
                txtRecv.AppendText($"[REÇU de {from}] {text}\r\n");
            }
            catch (SocketException se)
            {
                if (se.SocketErrorCode == SocketError.TimedOut)
                {
                    txtRecv.AppendText("[TIMEOUT] Aucun message reçu dans le délai imparti.\r\n");
                }
                else
                {
                    txtRecv.AppendText($"[ERREUR] Réception: {se.Message}\r\n");
                }
            }
            catch (Exception ex)
            {
                txtRecv.AppendText($"[ERREUR] Réception inattendue: {ex.Message}\r\n");
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
    }
}
