
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

// Here is a part of the Network between the application and the simulator
namespace Ex3.Models
{
    class Connect
    {
        // members of Connect class
        private TcpClient tcpClient;
        private NetworkStream networkStream;
        private BinaryWriter binaryWriter;
        private bool isConnect;

        // This part was taken from your ApplicationSettingsModel 
        #region Singleton instance of connect 
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

        #region Connection Issues
        // connected function - boolean returned value
        public bool connected() {
            return tcpClient.Connected;
        }

        // ConnectToHost function according to the ip and port given
        public void ConnectToHost(string ip, int port)
        {
            if (!isConnect)
            {
                tcpClient = new TcpClient();
                Task task = new Task(() =>
                {
                    while (!tcpClient.Connected)
                    {
                        try
                        {
                            tcpClient.Connect(ip, port);
                            isConnect = true;
                        }
                        catch (SocketException)
                        {
                            continue;
                        }
                    }
                    networkStream = tcpClient.GetStream();
                    binaryWriter = new BinaryWriter(networkStream);
                });
                task.Start();
                task.Wait();
            }

        }

        //Disconnect function - to close and dispose the tcpClient
        public void Disconnect()
        {
            tcpClient.Close();
        }
        #endregion

        #region Transfer Data Issues
        // ReadData function
        public string ReadData()
        {
            byte[] data = new byte[256];
            string responseData = string.Empty;
            int bytes = networkStream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            return responseData;
        }

        // WriteData function
        public double WriteData(string text)
        {
            Random r = new Random();
            if (tcpClient != null && tcpClient.Connected)
            {
                binaryWriter.Write(System.Text.Encoding.ASCII.GetBytes(text));
            }
            string test = ReadData();
            string[] splitted = test.Split('\'');
            string[] words = splitted[1].Split('.');
            double number = 0;
            if (words.Length > 1)
            {
                number = Convert.ToDouble(words[1]) % 100;
            }
            return Convert.ToDouble(splitted[1]) + number;
        }
        #endregion
    }
}