namespace Server_Side
{
    partial class ServerSide
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
            this.lblStatus = new System.Windows.Forms.Label();
            this.lbConexoes = new System.Windows.Forms.ListBox();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.Nome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MAC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSair = new System.Windows.Forms.Button();
            this.lbDadosRecebidos = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbRPC = new System.Windows.Forms.GroupBox();
            this.cbTrabalho = new System.Windows.Forms.CheckBox();
            this.txtMensagem = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.gbRPC.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(13, 13);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(35, 13);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "label1";
            // 
            // lbConexoes
            // 
            this.lbConexoes.FormattingEnabled = true;
            this.lbConexoes.Location = new System.Drawing.Point(6, 18);
            this.lbConexoes.Name = "lbConexoes";
            this.lbConexoes.Size = new System.Drawing.Size(340, 95);
            this.lbConexoes.TabIndex = 1;
            // 
            // dgvUsers
            // 
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Nome,
            this.IP,
            this.MAC});
            this.dgvUsers.Location = new System.Drawing.Point(12, 19);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(360, 168);
            this.dgvUsers.TabIndex = 2;
            // 
            // Nome
            // 
            this.Nome.DataPropertyName = "Nome";
            this.Nome.HeaderText = "Nome";
            this.Nome.Name = "Nome";
            this.Nome.ReadOnly = true;
            // 
            // IP
            // 
            this.IP.DataPropertyName = "IP";
            this.IP.HeaderText = "IP";
            this.IP.Name = "IP";
            this.IP.ReadOnly = true;
            this.IP.Width = 150;
            // 
            // MAC
            // 
            this.MAC.DataPropertyName = "MAC";
            this.MAC.HeaderText = "MAC";
            this.MAC.Name = "MAC";
            this.MAC.ReadOnly = true;
            // 
            // btnSair
            // 
            this.btnSair.Location = new System.Drawing.Point(277, 4);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(75, 22);
            this.btnSair.TabIndex = 3;
            this.btnSair.Text = "Encerrar";
            this.btnSair.UseVisualStyleBackColor = true;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // lbDadosRecebidos
            // 
            this.lbDadosRecebidos.FormattingEnabled = true;
            this.lbDadosRecebidos.Location = new System.Drawing.Point(6, 19);
            this.lbDadosRecebidos.Name = "lbDadosRecebidos";
            this.lbDadosRecebidos.Size = new System.Drawing.Size(510, 212);
            this.lbDadosRecebidos.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 150);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Usuarios conectados:";
            // 
            // gbRPC
            // 
            this.gbRPC.Controls.Add(this.cbTrabalho);
            this.gbRPC.Controls.Add(this.txtMensagem);
            this.gbRPC.Location = new System.Drawing.Point(428, 32);
            this.gbRPC.Name = "gbRPC";
            this.gbRPC.Size = new System.Drawing.Size(154, 73);
            this.gbRPC.TabIndex = 6;
            this.gbRPC.TabStop = false;
            this.gbRPC.Text = "RPC";
            this.gbRPC.Paint += new System.Windows.Forms.PaintEventHandler(this.gbRPC_Paint);
            // 
            // cbTrabalho
            // 
            this.cbTrabalho.AutoSize = true;
            this.cbTrabalho.Location = new System.Drawing.Point(6, 46);
            this.cbTrabalho.Name = "cbTrabalho";
            this.cbTrabalho.Size = new System.Drawing.Size(132, 17);
            this.cbTrabalho.TabIndex = 1;
            this.cbTrabalho.Text = "Não estou checado =(";
            this.cbTrabalho.UseVisualStyleBackColor = true;
            // 
            // txtMensagem
            // 
            this.txtMensagem.Location = new System.Drawing.Point(6, 19);
            this.txtMensagem.Name = "txtMensagem";
            this.txtMensagem.Size = new System.Drawing.Size(100, 20);
            this.txtMensagem.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbDadosRecebidos);
            this.groupBox1.Location = new System.Drawing.Point(6, 193);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(526, 242);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log de mensagens";
            this.groupBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.gbRPC_Paint);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbConexoes);
            this.groupBox2.Location = new System.Drawing.Point(15, 32);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(407, 183);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Log do Servidor";
            this.groupBox2.Paint += new System.Windows.Forms.PaintEventHandler(this.gbRPC_Paint);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Controls.Add(this.dgvUsers);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(16, 221);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(541, 446);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "CHAT";
            this.groupBox3.Paint += new System.Windows.Forms.PaintEventHandler(this.gbRPC_Paint);
            // 
            // ServerSide
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 675);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.gbRPC);
            this.Controls.Add(this.btnSair);
            this.Controls.Add(this.lblStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ServerSide";
            this.Text = "Servidor";
            this.Activated += new System.EventHandler(this.ServerSide_Activated);
            this.Deactivate += new System.EventHandler(this.ServerSide_Deactivate);
            this.Load += new System.EventHandler(this.ServerSide_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ServerSide_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ServerSide_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.gbRPC.ResumeLayout(false);
            this.gbRPC.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ListBox lbConexoes;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.ListBox lbDadosRecebidos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbRPC;
        private System.Windows.Forms.TextBox txtMensagem;
        private System.Windows.Forms.CheckBox cbTrabalho;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nome;
        private System.Windows.Forms.DataGridViewTextBoxColumn IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn MAC;
    }
}

