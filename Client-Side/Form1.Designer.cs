namespace Client_Side
{
    partial class ClientSide
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lbDadosRecebidos = new System.Windows.Forms.ListBox();
            this.pbStatus = new System.Windows.Forms.ProgressBar();
            this.gbChat = new System.Windows.Forms.GroupBox();
            this.Enviar = new System.Windows.Forms.Button();
            this.txtMensagem = new System.Windows.Forms.TextBox();
            this.btnConectar = new System.Windows.Forms.Button();
            this.gbRPC = new System.Windows.Forms.GroupBox();
            this.btnTrabalho = new System.Windows.Forms.Button();
            this.btnSendMensagem = new System.Windows.Forms.Button();
            this.txtMensagemRPC = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDesconectar = new System.Windows.Forms.Button();
            this.btnSair = new System.Windows.Forms.Button();
            this.lblRCP = new System.Windows.Forms.Label();
            this.lblFileServer = new System.Windows.Forms.Label();
            this.btnChoosefile = new System.Windows.Forms.Button();
            this.ofdArquivo = new System.Windows.Forms.OpenFileDialog();
            this.gbCadastro = new System.Windows.Forms.GroupBox();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.txtMAC = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnProceder = new System.Windows.Forms.Button();
            this.gbChat.SuspendLayout();
            this.gbRPC.SuspendLayout();
            this.gbCadastro.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(64, 13);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(100, 20);
            this.txtIP.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Servidor:";
            // 
            // btnSend
            // 
            this.btnSend.Enabled = false;
            this.btnSend.Location = new System.Drawing.Point(293, 39);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(120, 23);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Enviar";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 70);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(37, 13);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Status";
            // 
            // lbDadosRecebidos
            // 
            this.lbDadosRecebidos.FormattingEnabled = true;
            this.lbDadosRecebidos.Location = new System.Drawing.Point(6, 45);
            this.lbDadosRecebidos.Name = "lbDadosRecebidos";
            this.lbDadosRecebidos.Size = new System.Drawing.Size(510, 212);
            this.lbDadosRecebidos.TabIndex = 3;
            // 
            // pbStatus
            // 
            this.pbStatus.Location = new System.Drawing.Point(64, 39);
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(100, 23);
            this.pbStatus.TabIndex = 4;
            // 
            // gbChat
            // 
            this.gbChat.Controls.Add(this.lbDadosRecebidos);
            this.gbChat.Controls.Add(this.Enviar);
            this.gbChat.Controls.Add(this.txtMensagem);
            this.gbChat.Enabled = false;
            this.gbChat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbChat.Location = new System.Drawing.Point(12, 145);
            this.gbChat.Name = "gbChat";
            this.gbChat.Size = new System.Drawing.Size(522, 261);
            this.gbChat.TabIndex = 5;
            this.gbChat.TabStop = false;
            this.gbChat.Text = "CHAT";
            this.gbChat.Paint += new System.Windows.Forms.PaintEventHandler(this.gbChat_Paint);
            // 
            // Enviar
            // 
            this.Enviar.Location = new System.Drawing.Point(441, 16);
            this.Enviar.Name = "Enviar";
            this.Enviar.Size = new System.Drawing.Size(75, 23);
            this.Enviar.TabIndex = 2;
            this.Enviar.Text = "Enviar";
            this.Enviar.UseVisualStyleBackColor = true;
            this.Enviar.Click += new System.EventHandler(this.Enviar_Click);
            // 
            // txtMensagem
            // 
            this.txtMensagem.Location = new System.Drawing.Point(6, 19);
            this.txtMensagem.Name = "txtMensagem";
            this.txtMensagem.Size = new System.Drawing.Size(429, 20);
            this.txtMensagem.TabIndex = 0;
            this.txtMensagem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMensagem_KeyDown);
            // 
            // btnConectar
            // 
            this.btnConectar.Location = new System.Drawing.Point(170, 12);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new System.Drawing.Size(120, 23);
            this.btnConectar.TabIndex = 2;
            this.btnConectar.Text = "Conectar no Servidor";
            this.btnConectar.UseVisualStyleBackColor = true;
            this.btnConectar.Click += new System.EventHandler(this.btnConectar_Click);
            // 
            // gbRPC
            // 
            this.gbRPC.Controls.Add(this.btnTrabalho);
            this.gbRPC.Controls.Add(this.btnSendMensagem);
            this.gbRPC.Controls.Add(this.txtMensagemRPC);
            this.gbRPC.Controls.Add(this.label2);
            this.gbRPC.Enabled = false;
            this.gbRPC.Location = new System.Drawing.Point(12, 412);
            this.gbRPC.Name = "gbRPC";
            this.gbRPC.Size = new System.Drawing.Size(522, 60);
            this.gbRPC.TabIndex = 7;
            this.gbRPC.TabStop = false;
            this.gbRPC.Text = "RPC";
            this.gbRPC.Paint += new System.Windows.Forms.PaintEventHandler(this.gbChat_Paint);
            // 
            // btnTrabalho
            // 
            this.btnTrabalho.Location = new System.Drawing.Point(401, 19);
            this.btnTrabalho.Name = "btnTrabalho";
            this.btnTrabalho.Size = new System.Drawing.Size(75, 35);
            this.btnTrabalho.TabIndex = 4;
            this.btnTrabalho.Text = "Realizar Trabalho";
            this.btnTrabalho.UseVisualStyleBackColor = true;
            this.btnTrabalho.Click += new System.EventHandler(this.btnTrabalho_Click);
            // 
            // btnSendMensagem
            // 
            this.btnSendMensagem.Location = new System.Drawing.Point(320, 19);
            this.btnSendMensagem.Name = "btnSendMensagem";
            this.btnSendMensagem.Size = new System.Drawing.Size(75, 23);
            this.btnSendMensagem.TabIndex = 3;
            this.btnSendMensagem.Text = "Enviar";
            this.btnSendMensagem.UseVisualStyleBackColor = true;
            this.btnSendMensagem.Click += new System.EventHandler(this.SendMessage_Click);
            // 
            // txtMensagemRPC
            // 
            this.txtMensagemRPC.Location = new System.Drawing.Point(214, 19);
            this.txtMensagemRPC.Name = "txtMensagemRPC";
            this.txtMensagemRPC.Size = new System.Drawing.Size(100, 20);
            this.txtMensagemRPC.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Enviar mensagem por parametro de RPC:";
            // 
            // btnDesconectar
            // 
            this.btnDesconectar.Enabled = false;
            this.btnDesconectar.Location = new System.Drawing.Point(293, 12);
            this.btnDesconectar.Name = "btnDesconectar";
            this.btnDesconectar.Size = new System.Drawing.Size(120, 23);
            this.btnDesconectar.TabIndex = 2;
            this.btnDesconectar.Text = "Desconectar-se";
            this.btnDesconectar.UseVisualStyleBackColor = true;
            this.btnDesconectar.Click += new System.EventHandler(this.btnDesconectar_Click);
            // 
            // btnSair
            // 
            this.btnSair.Location = new System.Drawing.Point(419, 12);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(39, 23);
            this.btnSair.TabIndex = 2;
            this.btnSair.Text = "Sair";
            this.btnSair.UseVisualStyleBackColor = true;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // lblRCP
            // 
            this.lblRCP.AutoSize = true;
            this.lblRCP.Location = new System.Drawing.Point(12, 92);
            this.lblRCP.Name = "lblRCP";
            this.lblRCP.Size = new System.Drawing.Size(35, 13);
            this.lblRCP.TabIndex = 8;
            this.lblRCP.Text = "label3";
            // 
            // lblFileServer
            // 
            this.lblFileServer.AutoSize = true;
            this.lblFileServer.Location = new System.Drawing.Point(12, 115);
            this.lblFileServer.Name = "lblFileServer";
            this.lblFileServer.Size = new System.Drawing.Size(35, 13);
            this.lblFileServer.TabIndex = 8;
            this.lblFileServer.Text = "label3";
            // 
            // btnChoosefile
            // 
            this.btnChoosefile.Enabled = false;
            this.btnChoosefile.Location = new System.Drawing.Point(170, 39);
            this.btnChoosefile.Name = "btnChoosefile";
            this.btnChoosefile.Size = new System.Drawing.Size(120, 23);
            this.btnChoosefile.TabIndex = 2;
            this.btnChoosefile.Text = "Escolher Arquivo";
            this.btnChoosefile.UseVisualStyleBackColor = true;
            this.btnChoosefile.Click += new System.EventHandler(this.btnChoosefile_Click);
            // 
            // ofdArquivo
            // 
            this.ofdArquivo.FileName = "openFileDialog1";
            // 
            // gbCadastro
            // 
            this.gbCadastro.Controls.Add(this.txtMAC);
            this.gbCadastro.Controls.Add(this.txtNome);
            this.gbCadastro.Controls.Add(this.label4);
            this.gbCadastro.Controls.Add(this.label3);
            this.gbCadastro.Controls.Add(this.btnProceder);
            this.gbCadastro.Location = new System.Drawing.Point(6, 68);
            this.gbCadastro.Name = "gbCadastro";
            this.gbCadastro.Size = new System.Drawing.Size(528, 404);
            this.gbCadastro.TabIndex = 4;
            this.gbCadastro.TabStop = false;
            this.gbCadastro.Text = "Cadastro";
            this.gbCadastro.Visible = false;
            // 
            // txtNome
            // 
            this.txtNome.Location = new System.Drawing.Point(49, 23);
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(100, 20);
            this.txtNome.TabIndex = 0;
            // 
            // txtMAC
            // 
            this.txtMAC.Location = new System.Drawing.Point(49, 49);
            this.txtMAC.Name = "txtMAC";
            this.txtMAC.ReadOnly = true;
            this.txtMAC.Size = new System.Drawing.Size(100, 20);
            this.txtMAC.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Nome:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "MAC:";
            // 
            // btnProceder
            // 
            this.btnProceder.Location = new System.Drawing.Point(9, 75);
            this.btnProceder.Name = "btnProceder";
            this.btnProceder.Size = new System.Drawing.Size(140, 23);
            this.btnProceder.TabIndex = 2;
            this.btnProceder.Text = "Proceder";
            this.btnProceder.UseVisualStyleBackColor = true;
            this.btnProceder.Click += new System.EventHandler(this.btnProceder_Click);
            // 
            // ClientSide
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 478);
            this.Controls.Add(this.gbCadastro);
            this.Controls.Add(this.lblFileServer);
            this.Controls.Add(this.lblRCP);
            this.Controls.Add(this.gbRPC);
            this.Controls.Add(this.gbChat);
            this.Controls.Add(this.btnSair);
            this.Controls.Add(this.btnChoosefile);
            this.Controls.Add(this.btnDesconectar);
            this.Controls.Add(this.btnConectar);
            this.Controls.Add(this.pbStatus);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtIP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ClientSide";
            this.Text = "Client";
            this.Activated += new System.EventHandler(this.ClientSide_Activated);
            this.Deactivate += new System.EventHandler(this.ClientSide_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientSide_FormClosing);
            this.Load += new System.EventHandler(this.ClientSide_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ClientSide_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ClientSide_MouseDown);
            this.gbChat.ResumeLayout(false);
            this.gbChat.PerformLayout();
            this.gbRPC.ResumeLayout(false);
            this.gbRPC.PerformLayout();
            this.gbCadastro.ResumeLayout(false);
            this.gbCadastro.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ListBox lbDadosRecebidos;
        private System.Windows.Forms.ProgressBar pbStatus;
        private System.Windows.Forms.GroupBox gbChat;
        private System.Windows.Forms.Button Enviar;
        private System.Windows.Forms.TextBox txtMensagem;
        private System.Windows.Forms.Button btnConectar;
        private System.Windows.Forms.GroupBox gbRPC;
        private System.Windows.Forms.Button btnDesconectar;
        private System.Windows.Forms.Button btnTrabalho;
        private System.Windows.Forms.Button btnSendMensagem;
        private System.Windows.Forms.TextBox txtMensagemRPC;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.Label lblRCP;
        private System.Windows.Forms.Label lblFileServer;
        private System.Windows.Forms.Button btnChoosefile;
        private System.Windows.Forms.OpenFileDialog ofdArquivo;
        private System.Windows.Forms.GroupBox gbCadastro;
        private System.Windows.Forms.TextBox txtMAC;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnProceder;
    }
}

