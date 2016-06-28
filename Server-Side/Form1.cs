using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.Serialization.Formatters;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Server_Side
{
    public partial class ServerSide : Form, ObjetoRemotavel.IObservador
    {
        #region Variaveis
        //historico
        string path = System.Configuration.ConfigurationManager.AppSettings["Historico"];
        //lista de Sockets
        private ArrayList nSockets;
        private int porta = 8080;

        //chat
        private ArrayList m_aryClients = new ArrayList();   // List of Client Connections
        //transferencia de arquivos
        private const int BufferSize = 1024;                //tamanho do buffer dos pacotes 1024b = 1kb cada
        public string Status = string.Empty;
        public Thread T = null;                             //thread responsabvel por ficar na espera do recebimento do aruqivo
        public List<ChatUser> usuarios = new List<ChatUser>();
        //objeto remotavel
        private ObjetoRemotavel.ObjetoRemotavel objetoRemotavel;
        private int portaRCP = 8085;
        private string RPCServerName = "cr1st0ph3r";

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
        public ServerSide()
        {
            InitializeComponent();

            IniciarRCP();
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

        #region File Transfer

        /// <summary>
        /// Construtor.
        /// </summary>
        private void StartServer() {

            IPHostEntry IPHost = Dns.GetHostByName(Dns.GetHostName());
            lblStatus.Text = "O IP local desta máquina é: " + IPHost.AddressList[0].ToString();
            nSockets = new ArrayList();
            Thread thdListener = new Thread(new ThreadStart(listenerThread));
            thdListener.Start();

            //chat
            IniciaServidorChat();
            ThreadStart Ts = new ThreadStart(BeginReceive);
            T = new Thread(Ts);
            T.ApartmentState = ApartmentState.STA;//obrigo a minha thread a ser STA para habilitar o OpenfileDialog..clipboard..etc
            T.Start();
        }

        //Assim que um socket é adicionado a lista de arrays,
        // a thread de Halndler é criada.
        //Esta thread é responsavel por aceitar as conexoes de entrada.
        /// <summary>
        /// Thread responsavel por aceitar as conexoes de entrada.
        /// </summary>
        public void listenerThread()
        {
            TcpListener tcpListener = new TcpListener(porta);
            tcpListener.Start();
            objetoRemotavel = new ObjetoRemotavel.ObjetoRemotavel();
            while (true)
            {
                Socket handlerSocket = tcpListener.AcceptSocket();
                if (handlerSocket.Connected)
                {
                    //Como tudo executado neste trecho esta em outra thread,
                    //Para modificar os componentes do Form na thread mãe
                    //Eu preciso invoca-los para nao executar uma operação ilegal.
                    if (InvokeRequired){
                        lbConexoes.Invoke((Action)(() => lbConexoes.Items.Add(handlerSocket.RemoteEndPoint.ToString() + " conectado!"))); 
                    }
                    lock (this)
                    {
                        nSockets.Add(handlerSocket);
                    }
                    ThreadStart thdstHandler = new
                    ThreadStart(handlerThread);
                    Thread thdHandler = new Thread(thdstHandler);
                    thdHandler.Start();
                }
            }
        }

        //O restante do trabalho é realizado pela HandlerThread,
        //Este metodo encontra o ultimo socker usado e recupera um fluxo de dados do mesmo.
        //um vetor do mesmo tamanho do fluxo, e assim que os dados do fluxo sao obtidos,
        //estes dados sao entao transferidos para este array.
        //Com os dados em mao, a ultima tarefa é de decidir o que fazer com os dados,
        // que podem ser salvos em um arquivo de texto na maquina local por exemplo.
        /// <summary>
        /// Thread responsavel por tratar os dados do Fluxo.
        /// </summary>
        public void handlerThread()
        {
            Socket handlerSocket = (Socket)nSockets[nSockets.Count - 1];
            NetworkStream networkStream = new NetworkStream(handlerSocket);
            int thisRead = 0;
            int blockSize = 1024;
            Byte[] dataByte = new Byte[blockSize];
            lock (this)
            {
                // apenas um processo possui acesso ao arquivo de destino por vez.
                Stream fileStream = File.OpenWrite(@"C:\Users\user\Documents\Visual Studio 2015\Projects\Server-Side\ToTransfer.txt");
                while (true)
                {
                    thisRead = networkStream.Read(dataByte, 0, blockSize);
                    fileStream.Write(dataByte, 0, thisRead);
                    if (thisRead == 0) break;
                }
                fileStream.Close();
            }
            if (InvokeRequired){lbConexoes.Invoke((Action)(() => lbConexoes.Items.Add("Arquivo Escrito!")));}
         
            handlerSocket = null;
        }
        #endregion

        #region Chat

        #region Metodos de Aparencia
        /// <summary>
        /// Adiciona os dados do host que conectou.
        /// </summary>
        /// <param name="ip"></param>
        public void AddDGV(ChatUser user)
        {

            if (InvokeRequired)
            {
                DataGridViewRow row = (DataGridViewRow)dgvUsers.Rows[0].Clone();
                dgvUsers.Invoke((Action)(() => row = (DataGridViewRow)dgvUsers.Rows[0].Clone()));
                row.Cells[0].Value = user.Nome;
                row.Cells[1].Value = user.IP;
                row.Cells[2].Value = user.MAC1; 
                dgvUsers.Invoke((Action)(() => dgvUsers.Rows.Add(row)));
                dgvUsers.Invoke((Action)(() => dgvUsers.Refresh()));
            }
            else {
                DataGridViewRow row = (DataGridViewRow)dgvUsers.Rows[0].Clone();
                row.Cells[0].Value = user.Nome;
                row.Cells[1].Value = user.IP;
                row.Cells[2].Value = user.MAC1;
                dgvUsers.Rows.Add(row);
                dgvUsers.Refresh();
            }


        }
        /// <summary>
        /// Remove os dados do host que conectou.
        /// </summary>
        /// <param name="ip"></param>
        public void removeDGV(string ip)
        {

            //le apenas a porcao de ip do host
            int l = ip.IndexOf(":");
            if (l > 0) { ip = ip.Substring(0, l); }

            foreach (DataGridViewRow linha in dgvUsers.Rows)
            {
                try
                {
                    if (linha.Cells["IP"].Value.ToString() == (ip))
                    {
                        dgvUsers.Rows.Remove(linha);
                        break;
                    }
                }
                catch { }
            }
        }
        #endregion  

        #region Iniciar Servidor de Chat
        /// <summary>      
        /// Inicio do servidor de chat.
        /// </summary>
        /// <param name="args"></param>
        public void IniciaServidorChat()
        {

            ServerSide app = this;//new Form1();
            // Inicia o servidor de Chat
            Console.WriteLine("*** Servidor de Chat Inicializado *** ", DateTime.Now.ToString("G"));
            lbConexoes.Items.Add("*** Servidor de Chat Inicializado *** " + DateTime.Now.ToString("G"));


            const int nPortListen = 399;
            // Determina o endereco de IP local
            IPAddress[] aryLocalAddr = null;
            String strHostName = "";
            try
            {
                // NOTA: DNS lookups gasta muito tempo
                strHostName = Dns.GetHostName();
                IPHostEntry ipEntry = Dns.GetHostByName(strHostName);
                aryLocalAddr = ipEntry.AddressList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao tentar recuperar o IP local {0} ", ex.Message);
            }
                 
            // verifica se o ip local foi obtido
            if (aryLocalAddr == null || aryLocalAddr.Length < 1)
            {
                Console.WriteLine("Não foi possivel obeter o endereco de Ip local");
                return;
            }
            Console.WriteLine("\nAguardando conexões em : " + strHostName + "-> " + aryLocalAddr[0] + ":" + nPortListen);
            lbConexoes.Items.Add("\nAguardando conexões em : " + strHostName +"-> "+ aryLocalAddr[0] + ":"+nPortListen);
                        
            //Cria um socket listener nesta maquina, no seguinte endereço IP:
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(aryLocalAddr[0], 399));                  
            listener.Listen(10);
            
            // Cria um callback para notificar novas requisicoes de conexao.
            listener.BeginAccept(new AsyncCallback(app.OnConnectRequest), listener);




        }
        #endregion

        #region Pedido de Conexao
        /// <summary>
        /// Callback usado quando um cliente requisita conexao.
        /// Ele aceita a conexao e adiciona o cliente a lista de
        /// clientes conectados
        /// </summary>
        /// <param name="ar"></param>
        public void OnConnectRequest(IAsyncResult ar)
        {
            Socket listener = (Socket)ar.AsyncState;
            NewConnection(listener.EndAccept(ar));
            listener.BeginAccept(new AsyncCallback(OnConnectRequest), listener);
        }
        #endregion

        #region Nova Conexao
        /// <summary>
        /// adiciona a conexao na lista de clientes.
        /// </summary>
        /// <param name="sockClient">Connection to keep</param>
        public void NewConnection(Socket sockClient)
        {

            ClienteSocket cliente = new ClienteSocket(sockClient);
            m_aryClients.Add(cliente);            
            Console.WriteLine(cliente.Sock.RemoteEndPoint + " acabou de entrar.");

            //destaiva erros de cross-threads, porem nao é muito utilizado
            CheckForIllegalCrossThreadCalls = false;

            lbConexoes.Items.Add(cliente.Sock.RemoteEndPoint+" acabou de entrar.");
            string ip = "";
            int l = Convert.ToString(cliente.Sock.RemoteEndPoint).IndexOf(":");
            if (l > 0)
            {
                ip = Convert.ToString(cliente.Sock.RemoteEndPoint).Substring(0, l);
            }

            //data atual
            DateTime now = DateTime.Now;
            string nome = "";
            foreach (var user in usuarios) {
                if (user.IP == ip) {
                    nome = user.Nome;
                }
            }

            string strDateLine = "Bem-vindo " +nome +" "+ now.ToString("G") + "\n\r";

            // Coverte a mensagem para byte array e envia para o recem conectado
            Byte[] byteDateLine = System.Text.Encoding.ASCII.GetBytes(strDateLine.ToCharArray());
            cliente.Sock.Send(byteDateLine, byteDateLine.Length, 0);

            cliente.SetupRecieveCallback(this);
        }
        #endregion

        #region Recebe Dados e envia para os clientes
        /// <summary>
        /// Pega os dados recebido e manda para todas as conexoes ativas
        /// Caso nao hava dados recebidos é muito provavel que a conexao tenha falhado
        /// Dependendo do tipo de informacao recebida, ela nao sera distribuida
        /// Isso serve para comunicacao interna do programa
        /// </summary>
        /// <param name="ar"></param>
        public void OnRecievedData(IAsyncResult ar)
        {
            ClienteSocket cliente = (ClienteSocket)ar.AsyncState;
            byte[] aryRet = cliente.GetRecievedData(ar);
            string result = System.Text.Encoding.UTF8.GetString(aryRet);
            Console.WriteLine(result);
            // se nao houver dados, presume-se que o cliente esteja desconectado
            if (aryRet.Length < 1)
            {
                Console.WriteLine(cliente.Sock.RemoteEndPoint+" se desconectou.");
                removeDGV(cliente.Sock.RemoteEndPoint.ToString());
                cliente.Sock.Close();
                m_aryClients.Remove(cliente);// <---- olhar esse cara aqui
                return;
            }

            // Envia dos dados recebidos para todos os clientes conectados (incluindo quem mandou)
            if (result[0] == '1')// primeiro caracter sendo 1 identifica que a mensagem é proveniente de usuario
            {
                //remove o verificador
                aryRet = aryRet.Skip(1).ToArray();

                foreach (ClienteSocket clientSend in m_aryClients)
                {
                    try
                    {
                        clientSend.Sock.Send(aryRet);

                    }
                    catch
                    {
                        // Se o envio de dados por alguma razao falhar,
                        // a conexao é encerrada.
                        Console.WriteLine("Falha ao enviar dados para o cliente: ", cliente.Sock.RemoteEndPoint);
                        clientSend.Sock.Close();
                        m_aryClients.Remove(cliente);
                        return;
                    }
                }
                cliente.SetupRecieveCallback(this);
                lbDadosRecebidos.Items.Add(System.Text.Encoding.UTF8.GetString(aryRet)+ "   -="+DateTime.Now+"=-");
            }
            //no caso da mensagem nao for para usuarios e sim comunicacao interna do programa
            else if(result[0]=='2')
            {
                
            }
        }

 

        #endregion

        #region Recebe os Arquivos
        public void BeginReceive()
        {
            RecebeTCP(29250);
        }
        public void RecebeTCP(int portN)
        {
            TcpListener Listener = null;
            try
            {
                Listener = new TcpListener(IPAddress.Any, portN);
                Listener.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            byte[] RecData = new byte[BufferSize];
            int RecBytes;

            for (;;)
            {
                TcpClient client = null;
                NetworkStream netstream = null;
                Status = string.Empty;
                try
                {


                    string message = "Aaceitar solicitacao de recebimento de arquivo? ";
                    string caption = "Conexao de entrada";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result;


                    if (Listener.Pending())
                    {
                        client = Listener.AcceptTcpClient();
                        netstream = client.GetStream();
                        Status = "Conectado a um cliente\n";
                        result = MessageBox.Show(message, caption, buttons);

                        if (result == DialogResult.Yes)
                        {
                            string SaveFileName = string.Empty;
                            SaveFileDialog DialogSave = new SaveFileDialog();
                            DialogSave.Filter = "Todos os Arquivos (*.*)|*.*";
                            DialogSave.RestoreDirectory = true;
                            DialogSave.Title = "Aonde deseja salvar este arquivo?";
                            DialogSave.InitialDirectory = @"C:/";
                            if (DialogSave.ShowDialog() == DialogResult.OK)
                                SaveFileName = DialogSave.FileName;
                            if (SaveFileName != string.Empty)
                            {
                                int totalrecbytes = 0;
                                FileStream Fs = new FileStream(SaveFileName, FileMode.OpenOrCreate, FileAccess.Write);
                                while ((RecBytes = netstream.Read(RecData, 0, RecData.Length)) > 0)
                                {
                                    Fs.Write(RecData, 0, RecBytes);
                                    totalrecbytes += RecBytes;
                                }
                                Fs.Close();
                            }
                            netstream.Close();
                            client.Close();

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //netstream.Close();
                }
            }
        }
        #endregion

        #endregion

        #region RPC
        //Iniciador do RCP
        public void IniciarRCP() {
                
            // formatador customizado para o TcpChannel sink chain.
            BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
            provider.TypeFilterLevel = TypeFilterLevel.Full;

            //Area de objeto remotavel
            objetoRemotavel = new ObjetoRemotavel.ObjetoRemotavel();
            // Usando o protocolo TCP
            IDictionary props = new Hashtable();
            props["port"] = portaRCP;
            TcpChannel channel = new TcpChannel(props, null, provider);
            //observacao, apesar de obsoleto, o metodo RegisterChannel funciona bem
            ChannelServices.RegisterChannel(channel);
            //registra o servico remotavel
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(ObjetoRemotavel.ObjetoRemotavel), RPCServerName, WellKnownObjectMode.Singleton);
            ObjetoRemotavel.Cache.Attach(this);

        }

        //Metodo implicito na interface IObservador
        public void Notificar(string text)
        {
            //invoquerequired quando se trabalha em ambiente multithreads
            if (InvokeRequired)
            {
                txtMensagem.Invoke((Action)(() => txtMensagem.Text = text));
            }

        }
        public void RealizarTrabalho()
        {
            if (InvokeRequired)
            {
                cbTrabalho.Invoke((Action)(() => cbTrabalho.Text = "Fui checado"));
                cbTrabalho.Invoke((Action)(() => cbTrabalho.Checked = true));
            }
        }
        public void ReceberObjeto(ObjetoRemotavel.ObjetoRemotavel obj) {
            ChatUser user = new ChatUser();
            user.Nome = obj.Nome;
            user.MAC1 = obj.MAC;
            user.IP = obj.IP;
            usuarios.Add(user);

            AddDGV(user);
        }

        #endregion  

        #endregion

        #region Action Performed
        private void ServerSide_Load(object sender, EventArgs e)
        {
            StartServer();
            //carrega histirico
            using (StreamReader r = new StreamReader(path))
            {
                string linha;
                while ((linha = r.ReadLine()) != null)
                {
                    lbDadosRecebidos.Items.Add(linha);
                }
            }
        }




        private void btnSair_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Encerrando Servidor");

            //salva historico           
            StreamWriter SaveFile = new StreamWriter(path);
            foreach (var item in lbDadosRecebidos.Items)
            {
                SaveFile.WriteLine(item.ToString());
            }
            SaveFile.Close();

            // Clean up before we go home
            //listener.Close();
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            Environment.Exit(1);
        }

        private void gbRPC_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.Red, Color.Blue);
        }

        private void ServerSide_Paint(object sender, PaintEventArgs e)
        {
            if (estaAtivo())
            {
                e.Graphics.DrawRectangle(new Pen(Color.Red, 6), this.DisplayRectangle);
            }
            else { e.Graphics.DrawRectangle(new Pen(Color.Black, 6), this.DisplayRectangle); }

        }

        private void ServerSide_Activated(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void ServerSide_Deactivate(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void ServerSide_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        #endregion

    }
}
