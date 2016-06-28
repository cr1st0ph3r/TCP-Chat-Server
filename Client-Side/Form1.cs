using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Windows.Forms;

namespace Client_Side
{
    public partial class ClientSide : Form
    {
        #region Delegates
        //este delegate é um tipo que segura as referencias de um metodo dentro de um objeto.
        //Ele tambem é referido como um ponteiro de metodo de tipagem segura.

        //prototipo de delegacao para mandar os dados devolta para o formulario
        delegate void AddMessage(string novaMensagem);
        #endregion

        #region Variaveis
        private string FilePath = System.Configuration.ConfigurationManager.AppSettings["FilePath"];

        //chat
        TcpChannel RPCchan;
        int portaChat = 399;
        private Socket meuSocket;                  // conexo do servidor
        private byte[] meuBuffer = new byte[256];   // Buffer para recepcao dos dados
        private event AddMessage minhaMensagem;             // Event handler de mensagem do formulario

        public string SendingFilePath = string.Empty;       // caminho de origem do arquivo a ser enviado
        private const int BufferSize = 1024;                // tamanho do buffer do pacote que ira enviar o arquivo (1kb)

        //RCP
        ObjetoRemotavel.ObjetoRemotavel objetoRemotavel;
        private int portaRCP = 8085;
        private string RCPServerName = "cr1st0ph3r";

        //File transfer
        string caminho = "";
        TcpClient clientSocket;
        int portaArquivo = 8086;

        //form active
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

        //form drag
        //itens para mover formulario
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        #endregion

        #region Metodos
        /// <summary>
        /// verifica as credenciais do usuario.
        /// </summary>
        /// <returns></returns>
        private bool VerificaCredenciais() {
            //System.Configuration.ConfigurationManager.AppSettings.Set("lang", lang);
         string nome = System.Configuration.ConfigurationManager.AppSettings["NomeUser"];
         string mac = System.Configuration.ConfigurationManager.AppSettings["MAC"];

            if (nome == "no" || mac == "no") { return false; }
            else return true;
    } 
    
        public static bool estaAtivo()
        {
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
            {
                return false;       // No window is currently activated
            }

            var procId = Process.GetCurrentProcess().Id;
            int activeProcId;
            GetWindowThreadProcessId(activatedHandle, out activeProcId);

            return activeProcId == procId;
        }
        public ClientSide()
        {
            InitializeComponent();
           
            // adiciona um evento de Handler para decopulacao do formulario atrtaves da thread de entrada
            minhaMensagem = new AddMessage(OnAddMensagem);
        }
        /// <summary>
        /// Desenha bordas nas groupboxes
        /// </summary>
        /// <param name="box"></param>
        /// <param name="g"></param>
        /// <param name="textColor"></param>
        /// <param name="borderColor"></param>
        private void DrawGroupBox(GroupBox box, Graphics g, Color textColor, Color borderColor)
        {
            if (box != null)
            {
                Brush textBrush = new SolidBrush(textColor);
                Brush borderBrush = new SolidBrush(borderColor);
                Pen borderPen = new Pen(borderBrush);
                SizeF strSize = g.MeasureString(box.Text, box.Font);
                Rectangle rect = new Rectangle(box.ClientRectangle.X,
                                               box.ClientRectangle.Y + (int)(strSize.Height / 2),
                                               box.ClientRectangle.Width - 1,
                                               box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);

                // Clear text and border
                g.Clear(this.BackColor);

                // Draw text
                g.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);

                // Drawing Border
                //Left
                g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                //Right
                g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Bottom
                g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Top1
                g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
                //Top2
                g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }
        /// <summary>
        /// Retorna o ip local.
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        #region File transfer
        private void EnableFileClient()
        {
            // TCPChannel nao foi feito para transferencia de arquivos (acima de 8192 bytes)
            // Neste caso, estabelecemos outro tipo de canal, o TCP Client
            clientSocket = new TcpClient(txtIP.Text, portaArquivo);
            lblFileServer.Text = "Conectado no servidor de arquivo em: " + txtIP.Text + ":" + portaArquivo.ToString();
        }

        /// <summary>
        /// Metodo responsavel por transformar o arquivo em um fluxo de pacotes e envia-lo ao destinatario
        /// </summary>
        /// <param name="FilePAth">Origem do arquivo</param>
        /// <param name="IPA">Ip do destinatario</param>
        /// <param name="PortN">Porta do destinatario</param>
        public void SendTCP(string FilePAth, string IPA, Int32 PortN)
        {
            byte[] SendingBuffer = null;
            TcpClient client = null;
            lblStatus.Text = "";
            NetworkStream netstream = null;
            try
            {
                client = new TcpClient(IPA, PortN);
                lblStatus.Text = "Conectado ao servidor\n";
                netstream = client.GetStream();
                FileStream Fs = new FileStream(FilePAth, FileMode.Open, FileAccess.Read);
                int NoOfPackets = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Fs.Length) / Convert.ToDouble(BufferSize)));
                pbStatus.Maximum = NoOfPackets;
                int TotalLength = (int)Fs.Length, CurrentPacketLength, counter = 0;
                for (int i = 0; i < NoOfPackets; i++)
                {
                    if (TotalLength > BufferSize)
                    {
                        CurrentPacketLength = BufferSize;
                        TotalLength = TotalLength - CurrentPacketLength;
                    }
                    else
                        CurrentPacketLength = TotalLength;
                    SendingBuffer = new byte[CurrentPacketLength];
                    Fs.Read(SendingBuffer, 0, CurrentPacketLength);
                    netstream.Write(SendingBuffer, 0, (int)SendingBuffer.Length);
                    if (pbStatus.Value >= pbStatus.Maximum)
                        pbStatus.Value = pbStatus.Minimum;
                    pbStatus.PerformStep();
                }

                lblStatus.Text = lblStatus.Text + "Sent " + Fs.Length.ToString() + " bytes to the server";
                Fs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                netstream.Close();
                client.Close();

            }
        }
        #endregion

        #region Chat
        public void OnConnect(IAsyncResult ar)
        {
            // Socket was the passed in object
            Socket sock = (Socket)ar.AsyncState;

            //Verifica se a conexao deu certo
            try
            {
                if (sock.Connected)
                {
                    SetupRecieveCallback(sock);
                }
                else
                    MessageBox.Show(this, "Não foi possivel se conectar ao servidor", "Conexão Falhou!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Erro desconhecido!");
            }
        }

        /// <summary>
        /// Pega os novos dados e envia para todas as outras conexoes ativas.
        /// </summary>
        /// <param nome="ra"></param>
        public void AoReceberDados(IAsyncResult ra)
        {
            //Socket recebido por argumento
            Socket sock = (Socket)ra.AsyncState;

            //Verifica se ha dados para receber
            try
            {
                int nBytesRec = sock.EndReceive(ra);
                if (nBytesRec > 0)
                {
                    // Decodifica e escreve os dados recebidos na string
                    string DadoRecebido = Encoding.ASCII.GetString(meuBuffer, 0, nBytesRec);
                              
                    Invoke(minhaMensagem, new string[] { DadoRecebido });
                    
                    // tenta reestabelecer o callback caso a conexao esteja utilizavel
                    SetupRecieveCallback(sock);
                }
                else
                {
                    // no caso do nao recebimento de dados
                    Console.WriteLine(sock.RemoteEndPoint+" se desconectou.");
                    sock.Shutdown(SocketShutdown.Both);
                    sock.Close();
                }
            }
            catch (Exception ex)
            {
            //MessageBox.Show(this, ex.Message, "erro generico!");
            }
        }

        /// <summary>
        /// Adiciona a mensagem recem chegada do servidor na list de mensagem.
        /// </summary>
        /// <param name="mensagem"></param>
        public void OnAddMensagem(string mensagem)
        {
            lbDadosRecebidos.Items.Add(mensagem);
        }

        public void mandaMsg(int tipoMsg,string mensagem)
        {
            // verifica a conexao
            if (meuSocket == null || !meuSocket.Connected)
            {
                MessageBox.Show(this, "Conecte para mandar a mensagem");
                return;
            }
            try
            {

                //Byte[] msg = Encoding.ASCII.GetBytes((Environment.UserName +" -> " + txtMensagem.Text).ToCharArray());
                mensagem = ("1"+ System.Configuration.ConfigurationManager.AppSettings["NomeUser"] + " -> " + mensagem);

                // Converte string para array de bytes e envia
                Byte[] byteDateLine = Encoding.ASCII.GetBytes(mensagem.ToCharArray());
                meuSocket.Send(byteDateLine, byteDateLine.Length, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Não foi possivel mandar a mensagem.");
            }
        }

        /// <summary>
        /// configuta a chamada de retorno no recebimento de dados ou no caso de perda na conexao
        /// </summary>
        /// <param name="sock"></param>
        public void SetupRecieveCallback(Socket sock)
        {
            try
            {
                //recebimento assincrono de dados
                AsyncCallback recieveData = new AsyncCallback(AoReceberDados);
                sock.BeginReceive(meuBuffer, 0, meuBuffer.Length, SocketFlags.None, recieveData, sock);
            }
            catch (Exception ex)
            {
                //no caso de falha no callback
                MessageBox.Show(this, ex.Message, "A chamada de retorno falhou!");
            }
        }

        #endregion

        #endregion

        #region Action Performed
        private void btnSend_Click(object sender, EventArgs e)
        {
            Stream fileStream = File.OpenRead(caminho);
            // Aloca o espaco de memoria para o arquivo
            byte[] fileBuffer = new byte[fileStream.Length];
            fileStream.Read(fileBuffer, 0, (int)fileStream.Length);
            // abre uma conexao TCP/IP e envia os dados           
            NetworkStream ns = clientSocket.GetStream();
            ns.Write(fileBuffer, 0, fileBuffer.GetLength(0));
            ns.Close();
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            //tem credenciais?
            if (VerificaCredenciais())
            {
                try
                {
                    //==== RPC

                    //verifico se o objeto TCP de RPC esta populado
                    if (RPCchan == null)
                    {
                        // usando o protocolo TCP
                        // neste caso ambos o servidor e cliente estao rodando na mesma maquina.
                        RPCchan = new TcpChannel();
                        //registra o canal para disponibilizar o servico.
                        ChannelServices.RegisterChannel(RPCchan);
                        // cria uma instancia do objeto remoto
                        objetoRemotavel = (ObjetoRemotavel.ObjetoRemotavel)Activator.GetObject(typeof(ObjetoRemotavel.ObjetoRemotavel),
                            "tcp://" + txtIP.Text + ":" + portaRCP.ToString() + "/" + RCPServerName);
                        //trocar o localhost pelo endereco do servidor no caso de acesso remoto
                        lblRCP.Text = "Conectado no servidor RPC em: " + txtIP.Text + ":" + portaRCP.ToString() + "/" + RCPServerName;
                        
                        objetoRemotavel.Nome = System.Configuration.ConfigurationManager.AppSettings["NomeUser"];
                        objetoRemotavel.MAC = System.Configuration.ConfigurationManager.AppSettings["MAC"];
                        objetoRemotavel.IP = GetLocalIPAddress();
                        objetoRemotavel.EnviarObjeto(objetoRemotavel);
                    }

                    //==== RPC

                    // Fecha o socket se ele estiver aberto
                    if (meuSocket != null && meuSocket.Connected)
                    {
                        meuSocket.Shutdown(SocketShutdown.Both);
                        System.Threading.Thread.Sleep(10);
                        meuSocket.Close();
                    }

                    // Cria o objeto de soquete
                    meuSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    // Define o endereco de IP e porta do servidor
                    IPEndPoint epServer = new IPEndPoint(IPAddress.Parse(txtIP.Text), portaChat);

                    //conecta no servidor (metodo sem bloqueio)
                    meuSocket.Blocking = false;
                    //cria chamada de retorno asincrona
                    AsyncCallback onconnect = new AsyncCallback(OnConnect);
                    //inicia a conexao usando o ipendpoint, a chamada assincrona e o socket em si
                    meuSocket.BeginConnect(epServer, onconnect, meuSocket);
                    //notifica
                    lblStatus.Text = "Conectado no servidor Chat em: " + txtIP.Text + ":" + portaChat.ToString();

                    //mandaMsg(2, GetMacAddress(txtIP.Text));

                    gbChat.Enabled = true;
                    gbRPC.Enabled = true;
                    btnConectar.Enabled = false;
                    btnDesconectar.Enabled = true;
                    btnSair.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Falha ao tentar se conecar com o servidor!");
                }

            }
            else {
                gbCadastro.Visible = true;
                btnConectar.Enabled = false;
                txtMAC.Text= NetworkInterface.GetAllNetworkInterfaces().Where(nic => nic.OperationalStatus == OperationalStatus.Up).Select(nic => nic.GetPhysicalAddress().ToString()).FirstOrDefault();
                txtNome.Focus();
            }
        }

        private void Enviar_Click(object sender, EventArgs e)
        {
            mandaMsg(1, txtMensagem.Text);
        }

        private void ClientSide_Load(object sender, EventArgs e)
        {
            IPHostEntry IPHost = Dns.GetHostByName(Dns.GetHostName());
            txtIP.Text = IPHost.AddressList[0].ToString();     
            txtIP.SelectAll();
        }

        private void gbChat_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.Red, Color.Blue);
        }

        private void btnDesconectar_Click(object sender, EventArgs e)
        {
            //me livro do socket do chat
            if (meuSocket != null && meuSocket.Connected)
            {
                meuSocket.Shutdown(SocketShutdown.Both);
                System.Threading.Thread.Sleep(10);
                meuSocket.Close();

            }
            // e tambem do socket do RPC
            if (RPCchan != null) {
                ChannelServices.UnregisterChannel(RPCchan);
                RPCchan = null;
                objetoRemotavel = null;
            }

            btnConectar.Enabled = true;
            btnDesconectar.Enabled = false;
            btnSair.Enabled = true;
            gbChat.Enabled = false;
            gbRPC.Enabled = false;
        }

        private void btnTrabalho_Click(object sender, EventArgs e)
        {
            //metodo ChamarMetodo, existente na biblioteca remotavel
            if (RPCchan != null) { objetoRemotavel.ChamarMetodo(); }
        }

        private void SendMessage_Click(object sender, EventArgs e)
        {
            //invoke para crossthread
            if (InvokeRequired)
            {
                txtMensagem.Invoke((Action)(() => objetoRemotavel.SetMessage(txtMensagemRPC.Text)));
            }
            //metodo SetMessage existente na biblioteca remotavel
            else objetoRemotavel.SetMessage(txtMensagemRPC.Text);
        }

        private void txtMensagem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                mandaMsg(1, txtMensagem.Text);
                txtMensagem.Text = "";
            }
        }
        
        private void btnSair_Click(object sender, EventArgs e)
        {
            //me livro do socket do chat
            if (meuSocket != null && meuSocket.Connected)
            {
                meuSocket.Shutdown(SocketShutdown.Both);
                System.Threading.Thread.Sleep(10);
                meuSocket.Close();
                btnConectar.Enabled = true;
                btnDesconectar.Enabled = false;
            }
            // e tambem do socket do RPC
            if (RPCchan != null)
            {
                ChannelServices.UnregisterChannel(RPCchan);
                RPCchan = null;
                objetoRemotavel = null;
            }
            //encerra threads: however nao é o metodo correto de encerrar aplicacoes

            this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.ClientSide_FormClosing);
            this.Close();

            Environment.Exit(1);
            //nao encerra threads:
            //Application.Exit();

        }

        private void btnChoosefile_Click(object sender, EventArgs e)
        {

            DialogResult result = ofdArquivo.ShowDialog();
            if (result == DialogResult.OK)
            {
                caminho = ofdArquivo.InitialDirectory + ofdArquivo.FileName;
                btnSend.Enabled = true;
                EnableFileClient();
            }
        }

        private void ClientSide_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void ClientSide_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void ClientSide_Paint(object sender, PaintEventArgs e)
        {
            if (estaAtivo())
            {
                e.Graphics.DrawRectangle(new Pen(Color.Red, 6), this.DisplayRectangle);
            }
            else { e.Graphics.DrawRectangle(new Pen(Color.Black, 6), this.DisplayRectangle); }
        }

        private void ClientSide_Activated(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void ClientSide_Deactivate(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void btnProceder_Click(object sender, EventArgs e)
        {
            Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);                    
            conf.AppSettings.Settings["NomeUser"].Value = txtNome.Text;
            conf.AppSettings.Settings["MAC"].Value = txtMAC.Text;
            conf.Save();
            ConfigurationManager.RefreshSection("appSettings");
            gbCadastro.Visible = false;
            btnConectar.Enabled = true;
        }

        #endregion
    }

}