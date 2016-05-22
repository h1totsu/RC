using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RC
{
    class SocketClient
    {
        public String ServerIp { get; set; }

        private Socket sender;
        private IPEndPoint ipEndPoint;

        public SocketClient(String serverIp)
        {
            ServerIp = serverIp;
        }

        public Message Execute(String command)
        {
            IPHostEntry ipHost = null;
            try
            {
                ipHost = Dns.GetHostEntry(ServerIp);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot connect to IP");
                return null;
            }
            IPAddress ipAddr = ipHost.AddressList[0];
            ipEndPoint = new IPEndPoint(ipAddr, 11000);

            sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            sender.Connect(ipEndPoint);

            byte[] bytes = new byte[32768];
            byte[] msg = Encoding.UTF8.GetBytes(command);

            // Отправляем данные через сокет
            int bytesSent = sender.Send(msg);

            // Получаем ответ от сервера
            int bytesRec = sender.Receive(bytes);

            Object reply = SerializeUtils.ByteArrayToObject(bytes);
            return (Message)reply;
        }

        public void Shutdown()
        {
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
    }
}
