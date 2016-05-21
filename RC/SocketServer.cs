﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RC
{
    class SocketServer
    {
        private static Message message = new Message();

        private static void Listen()
        {
            IPHostEntry ipHost = Dns.GetHostEntry(GetLocalIPAddress());
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
                    Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);

                    // Программа приостанавливается, ожидая входящее соединение
                    Socket handler = sListener.Accept();
                    string data = null;

                    // Мы дождались клиента, пытающегося с нами соединиться
                    
                    byte[] bytes = new byte[4096];
                    int bytesRec = handler.Receive(bytes);
                    
                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    // Показываем данные на консоли

                    Console.Write("Полученный текст: " + data + "\n\n");

                    string[] args = data.Split(';');

                    switch (args[0])
                    {
                        case Command.GET_DIR_INFO:
                        {
                            DirectoryInfo dir = new DirectoryInfo(args[1]);
                            message.Directories = dir.GetDirectories();
                            message.Files = dir.GetFiles();
                            SendData(handler, message);
                        } break;
                        case Command.GET_DRIVES:
                        {
                            message.Text = CommandUtils.GetDrives();
                            SendData(handler, message);
                        } break;
                    }
                    
                    // Отправляем ответ клиенту\
                    string reply = "Спасибо за запрос в " + data.Length.ToString()
                            + " символов";

                    if (data.IndexOf("<TheEnd>") > -1)
                    {
                        Console.WriteLine("Сервер завершил соединение с клиентом.");
                        break;
                    }
                    
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

        public static void Start() 
        {
            ThreadStart threadDelegate = new ThreadStart(Listen);
            Thread thread = new Thread(threadDelegate);
            thread.Start();
        }

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

        private static void SendData(Socket handler, Message response)
        {
            byte[] msg = SerializeUtils.ObjectToByteArray(response);
            handler.Send(msg);
        }
    }
}
