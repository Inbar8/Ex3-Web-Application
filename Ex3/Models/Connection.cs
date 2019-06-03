using System.IO;
using System.Net.Sockets;
using System.Net;

// Here is a part of the Network between the application and the simulator
namespace FlightSimulator.Model
{
    class Connect
    {
        // members of Connect class
        private TcpListener server;
        private TcpClient client;
        private BinaryReader reader;
        public bool IsRunning { get; set; } = false;
        public bool IsConnected { get; set; } = false;

        // This part was taken from your ApplicationSettingsModel 
        #region Singleton
        private static Connect m_Instance = null;

        public static Connect Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new Connect();
                }
                return m_Instance;
            }
        }
        #endregion

        // ConnectToHost function according to the ip and port given
        public void ConnectToHost(string ip, int port)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            server = new TcpListener(ep);
            if (!IsRunning)
            {
                server.Start();
            }
            IsRunning = true;
        }

        // ReadData function
        public string[] ReadData()
        {
            while (!IsConnected)
            {
                this.client = server.AcceptTcpClient();
                reader = new BinaryReader(client.GetStream());
                IsConnected = true;
            }
            string dataRead = "";
            char currentChar;
            while ((currentChar = reader.ReadChar()) != '\n')
            {
                dataRead += currentChar;
            }
            string[] temp = dataRead.Split(',');
            string[] res = { temp[0], temp[1] };
            return res;
        }

        //Disconnect function
        public void Disconnect()
        {
            if (client != null && client.Connected)
            {
                client.Close();
            }
            if (IsRunning)
            {
                server.Stop();
            }
            IsRunning = false;
            IsConnected = false;
            m_Instance = null;
        }
    }
}