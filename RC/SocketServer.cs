using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RC
{
    class SocketServer
    {
        private static Thread thread;
        private static Message message = new Message();
        private frmMain form;

        public SocketServer(frmMain form)
        {
            this.form = form;
        }

        private void Listen()
        {
            IPHostEntry ipHost = Dns.GetHostEntry(this.GetLocalIPAddress());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

            // Создаем сокет Tcp/Ip
            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);
                // Начинаем слушать соединения
                while (true)
                {
                    // Программа приостанавливается, ожидая входящее соединение
                    Socket handler = sListener.Accept();
                    string data = null;

                    // Мы дождались клиента, пытающегося с нами соединиться

                    byte[] bytes = new byte[32768];
                    int bytesRec = handler.Receive(bytes);
                    
                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    // Показываем данные на консоли


                    string[] args = data.Split(';');

                    switch (args[0])
                    {
                        case Command.GET_DIR_INFO:
                        {
                            if (Directory.Exists(args[1]))
                            {
                                DirectoryInfo dir = new DirectoryInfo(args[1]);
                                message.Directories = dir.GetDirectories();
                                message.Files = dir.GetFiles();
                            }
                        } break;
                        case Command.DELETE:
                        {
                            message = new Message();
                            if (Directory.Exists(args[1]))
                            {
                                Directory.Delete(args[1], true);
                            } 
                            else
                            {
                                File.Delete(args[1]);
                            }
                        } break;
                        case Command.RENAME:
                        {
                            DirectoryInfo dir = new DirectoryInfo(args[1]);
                            string newName = dir.Parent.FullName + "\\" + args[2];
                            Directory.Move(args[1], newName);
                            message.Text = newName;
                        } break;
                        case Command.GET_DRIVES:
                        {
                            message.Text = CommandUtils.GetDrives();
                        } break;
                        case Command.CONNECT:
                        {
                            if (MessageBox.Show("Allow connect?", "Connect", MessageBoxButtons.OKCancel) == DialogResult.OK)
                            {
                               message.Text = Command.SUCCESS;
                               form.del.Invoke();
                            }
                            else
                            {
                                message.Text = Command.DENIED;
                            }
                        } break;
                    }

                    SendData(handler, message);
                    
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }
        // Устанавливаем для сокета локальную конечную точку

        public void Start() 
        {
            ThreadStart threadDelegate = new ThreadStart(Listen);
            thread = new Thread(threadDelegate);
            thread.Start();
        }

        public string GetLocalIPAddress()
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

        private static void SendData(Socket handler, Message response)
        {
            byte[] msg = SerializeUtils.ObjectToByteArray(response);
            handler.Send(msg);
        }

        public static void Stop()
        {
            thread.Abort();
        }
    }
}
