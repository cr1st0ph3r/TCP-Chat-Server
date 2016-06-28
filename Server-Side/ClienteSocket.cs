using System;
using System.Net.Sockets;


namespace Server_Side
{
    internal class ClienteSocket
    {
        private Socket m_sock;                      // Conexao para o cliente
        private byte[] m_byBuff = new byte[50];     //tamanho do buffer dos pacotes 50b

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="sock">client socket conneciton this object represents</param>
        public ClienteSocket(Socket sock)
        {
            m_sock = sock;
        }

        // Readonly access
        public Socket Sock
        {
            get { return m_sock; }
        }

        /// <summary>
        /// Setup the callback for recieved data and loss of conneciton
        /// </summary>
        /// <param name="app"></param>
        public void SetupRecieveCallback(ServerSide app)
        {
            try
            {
                AsyncCallback recieveData = new AsyncCallback(app.OnRecievedData);
                m_sock.BeginReceive(m_byBuff, 0, m_byBuff.Length, SocketFlags.None, recieveData, this);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Recieve callback setup failed! {0}", ex.Message);
            }
        }

        /// <summary>
        /// Data has been recieved so we shall put it in an array and
        /// return it.
        /// </summary>
        /// <param name="ar"></param>
        /// <returns>Array of bytes containing the received data</returns>
        public byte[] GetRecievedData(IAsyncResult ar)
        {
            int nBytesRec = 0;
            try
            {
                nBytesRec = m_sock.EndReceive(ar);
            }
            catch { }
            byte[] byReturn = new byte[nBytesRec];
            Array.Copy(m_byBuff, byReturn, nBytesRec);

            /*
			// Check for any remaining data and display it
			// This will improve performance for large packets 
			// but adds nothing to readability and is not essential
			int nToBeRead = m_sock.Available;
			if( nToBeRead > 0 )
			{
				byte [] byData = new byte[nToBeRead];
				m_sock.Receive( byData );
				// Append byData to byReturn here
			}
			*/
            return byReturn;
        }
    }
}
