using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RC
{
    class SocketClient
    {
        public String ServerIp { get; set; }
        public static String Response { get; set; }
        public static String CurrentCommand { get; set; }
        public static String Args { get; set; }

        private Socket sender;
        private IPEndPoint ipEndPoint;

        public SocketClient(String serverIp)
        {
            ServerIp = serverIp;
        }

        public Message Execute(String command)
        {
            IPHostEntry ipHost = Dns.GetHostEntry(ServerIp);
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
