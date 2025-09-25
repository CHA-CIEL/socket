namespace socketUDP
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label labelRecv;
        private System.Windows.Forms.TextBox txtRecvIP;
        private System.Windows.Forms.TextBox txtRecvPort;
        private System.Windows.Forms.Label labelDest;
        private System.Windows.Forms.TextBox txtDestIP;
        private System.Windows.Forms.TextBox txtDestPort;
        private System.Windows.Forms.Label labelIpeR;
        private System.Windows.Forms.Label labelIpeD;
        private System.Windows.Forms.Label labelEnvoi;
        private System.Windows.Forms.TextBox txtSend;
        private System.Windows.Forms.Label labelRecp;
        private System.Windows.Forms.TextBox txtRecv;
        private System.Windows.Forms.Button btnCreateBind;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnReceive;
        private System.Windows.Forms.Button btnCls;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.labelRecv = new System.Windows.Forms.Label();
            this.txtRecvIP = new System.Windows.Forms.TextBox();
            this.txtRecvPort = new System.Windows.Forms.TextBox();
            this.labelDest = new System.Windows.Forms.Label();
            this.txtDestIP = new System.Windows.Forms.TextBox();
            this.txtDestPort = new System.Windows.Forms.TextBox();
            this.labelIpeR = new System.Windows.Forms.Label();
            this.labelIpeD = new System.Windows.Forms.Label();
            this.labelEnvoi = new System.Windows.Forms.Label();
            this.txtSend = new System.Windows.Forms.TextBox();
            this.labelRecp = new System.Windows.Forms.Label();
            this.txtRecv = new System.Windows.Forms.TextBox();
            this.btnCreateBind = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnReceive = new System.Windows.Forms.Button();
            this.btnCls = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.labelRecv.AutoSize = true;
            this.labelRecv.Location = new System.Drawing.Point(12, 15);
            this.labelRecv.Name = "labelRecv";
            this.labelRecv.Size = new System.Drawing.Size(38, 13);
            this.labelRecv.TabIndex = 0;
            this.labelRecv.Text = "Recp.";
            this.txtRecvIP.Location = new System.Drawing.Point(70, 12);
            this.txtRecvIP.Name = "txtRecvIP";
            this.txtRecvIP.Size = new System.Drawing.Size(120, 20);
            this.txtRecvIP.TabIndex = 1;
            this.txtRecvIP.Text = "127.0.0.1";
            this.txtRecvPort.Location = new System.Drawing.Point(196, 12);
            this.txtRecvPort.Name = "txtRecvPort";
            this.txtRecvPort.Size = new System.Drawing.Size(60, 20);
            this.txtRecvPort.TabIndex = 2;
            this.txtRecvPort.Text = "3031";
            this.labelDest.AutoSize = true;
            this.labelDest.Location = new System.Drawing.Point(12, 41);
            this.labelDest.Name = "labelDest";
            this.labelDest.Size = new System.Drawing.Size(35, 13);
            this.labelDest.TabIndex = 3;
            this.labelDest.Text = "Dest.";
            this.txtDestIP.Location = new System.Drawing.Point(70, 38);
            this.txtDestIP.Name = "txtDestIP";
            this.txtDestIP.Size = new System.Drawing.Size(120, 20);
            this.txtDestIP.TabIndex = 4;
            this.txtDestIP.Text = "127.0.0.1";
            this.txtDestPort.Location = new System.Drawing.Point(196, 38);
            this.txtDestPort.Name = "txtDestPort";
            this.txtDestPort.Size = new System.Drawing.Size(60, 20);
            this.txtDestPort.TabIndex = 5;
            this.txtDestPort.Text = "3032";
            this.labelIpeR.AutoSize = true;
            this.labelIpeR.Location = new System.Drawing.Point(262, 15);
            this.labelIpeR.Name = "labelIpeR";
            this.labelIpeR.Size = new System.Drawing.Size(28, 13);
            this.labelIpeR.TabIndex = 6;
            this.labelIpeR.Text = "IPeR";
            this.labelIpeD.AutoSize = true;
            this.labelIpeD.Location = new System.Drawing.Point(262, 41);
            this.labelIpeD.Name = "labelIpeD";
            this.labelIpeD.Size = new System.Drawing.Size(29, 13);
            this.labelIpeD.TabIndex = 7;
            this.labelIpeD.Text = "IPeD";
            this.labelEnvoi.AutoSize = true;
            this.labelEnvoi.Location = new System.Drawing.Point(12, 75);
            this.labelEnvoi.Name = "labelEnvoi";
            this.labelEnvoi.Size = new System.Drawing.Size(34, 13);
            this.labelEnvoi.TabIndex = 8;
            this.labelEnvoi.Text = "Envoi";
            this.txtSend.Location = new System.Drawing.Point(70, 72);
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(318, 20);
            this.txtSend.TabIndex = 9;
            this.txtSend.Text = "Bonjour UDP";
            this.labelRecp.AutoSize = true;
            this.labelRecp.Location = new System.Drawing.Point(12, 110);
            this.labelRecp.Name = "labelRecp";
            this.labelRecp.Size = new System.Drawing.Size(35, 13);
            this.labelRecp.TabIndex = 10;
            this.labelRecp.Text = "Recp.";
            this.txtRecv.Location = new System.Drawing.Point(70, 110);
            this.txtRecv.Multiline = true;
            this.txtRecv.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRecv.ReadOnly = true;
            this.txtRecv.Name = "txtRecv";
            this.txtRecv.Size = new System.Drawing.Size(318, 199);
            this.txtRecv.TabIndex = 11;
            this.btnCreateBind.Location = new System.Drawing.Point(415, 10);
            this.btnCreateBind.Name = "btnCreateBind";
            this.btnCreateBind.Size = new System.Drawing.Size(190, 23);
            this.btnCreateBind.TabIndex = 12;
            this.btnCreateBind.Text = "Créer Socket et Bind(IPeR)";
            this.btnCreateBind.UseVisualStyleBackColor = true;
            this.btnCreateBind.Click += new System.EventHandler(this.btnCreateBind_Click);
            this.btnClose.Location = new System.Drawing.Point(415, 39);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(190, 23);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Fermer Close()";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnSend.Location = new System.Drawing.Point(415, 71);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(190, 23);
            this.btnSend.TabIndex = 14;
            this.btnSend.Text = "Envoyer SendTo(IPeD)";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            this.btnReceive.Location = new System.Drawing.Point(415, 110);
            this.btnReceive.Name = "btnReceive";
            this.btnReceive.Size = new System.Drawing.Size(190, 40);
            this.btnReceive.TabIndex = 15;
            this.btnReceive.Text = "Recevoir ReceiveFrom()\r\nBloquant Timeout";
            this.btnReceive.UseVisualStyleBackColor = true;
            this.btnReceive.Click += new System.EventHandler(this.btnReceive_Click);
            this.btnCls.Location = new System.Drawing.Point(415, 315);
            this.btnCls.Name = "btnCls";
            this.btnCls.Size = new System.Drawing.Size(75, 23);
            this.btnCls.TabIndex = 16;
            this.btnCls.Text = "CLS";
            this.btnCls.UseVisualStyleBackColor = true;
            this.btnCls.Click += new System.EventHandler(this.btnCls_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 355);
            this.Controls.Add(this.btnCls);
            this.Controls.Add(this.btnReceive);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCreateBind);
            this.Controls.Add(this.txtRecv);
            this.Controls.Add(this.labelRecp);
            this.Controls.Add(this.txtSend);
            this.Controls.Add(this.labelEnvoi);
            this.Controls.Add(this.labelIpeD);
            this.Controls.Add(this.labelIpeR);
            this.Controls.Add(this.txtDestPort);
            this.Controls.Add(this.txtDestIP);
            this.Controls.Add(this.labelDest);
            this.Controls.Add(this.txtRecvPort);
            this.Controls.Add(this.txtRecvIP);
            this.Controls.Add(this.labelRecv);
            this.Name = "Form1";
            this.Text = "Communication par socket UDP";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

