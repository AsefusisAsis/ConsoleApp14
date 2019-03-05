using System;
using System.Net;
using System.Net.Sockets;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 8888);
            server.Start();
            double z1;
            double z2;
            try
            { 
            while (true)
            {
                Console.WriteLine("Ожидание подключений... ");

                // получаем входящее подключение

                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Подключен клиент. Выполнение запроса...");

                // получаем сетевой поток для чтения и записи
                NetworkStream stream = client.GetStream();
                byte[] data2 = new byte[256];
                int bytes = stream.Read(data2, 0, data2.Length);
                string message2 = System.Text.Encoding.UTF8.GetString(data2, 0, bytes);
                    if (message2 != "exit")
                    {
                        int message = Convert.ToInt32(message2);
                        z1 = Math.Sin((Math.PI / 2) + (3 * message)) / (1 - Math.Sin(3 * message - Math.PI));
                        z2 = Math.Sin((5 / 4) * Math.PI + (3 / 2) * message) / Math.Cos((5 / 4) * Math.PI + (3 / 2) * message);
                    }else
                    {
                        
                        stream.Close();
                        // закрываем подключение
                        client.Close();
                        break;
                    }


                // сообщение для отправки клиенту
                string response = ("z1=" + z1 + "\n" + "z2=" + z2);
                // преобразуем сообщение в массив байтов
                byte[] data = System.Text.Encoding.UTF8.GetBytes(response);

                // отправка сообщения
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Отправлено сообщение: {0}", response);
                // закрываем поток
                stream.Close();
                // закрываем подключение
                client.Close();
            } }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (server != null)
                    server.Stop();
            }
            
        }
    }
}
