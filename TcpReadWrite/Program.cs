using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string ipAddress = "127.0.0.1";
            int port = 10023;

            TcpClient tcpClient = new TcpClient();

            tcpClient.Connect(ipAddress, port);

            tcpClient.NoDelay = false;

            NetworkStream netStream = tcpClient.GetStream();

            string output = "";

            string[] commands = new string[] {
                "username",
                "password",
                "commandName param1 param2",
                "exit"
            };

            WriteLine(netStream, String.Join("\n", commands) + "\n");

            output = ReadLine(tcpClient, netStream, output);

            tcpClient.Close();
        }

        private static void WriteLine(NetworkStream netStream, string write)
        {
            if (netStream.CanWrite)
            {
                byte[] writeData = Encoding.ASCII.GetBytes(write);

                netStream.Write(writeData, 0, writeData.Length);

                // 需等待資料真的已寫入 NetworkStream
                Thread.Sleep(3000);

                Console.WriteLine();
                Console.WriteLine("Write: " + write);
                Console.WriteLine("-------------------------");
            }
        }

        private static string ReadLine(TcpClient tcpClient, NetworkStream netStream,
            string output)
        {
            if (netStream.CanRead)
            {
                byte[] bytes = new byte[tcpClient.ReceiveBufferSize];

                int numBytesRead = netStream.Read(bytes, 0,
                    (int)tcpClient.ReceiveBufferSize);

                byte[] bytesRead = new byte[numBytesRead];
                Array.Copy(bytes, bytesRead, numBytesRead);

                string returndata = Encoding.ASCII.GetString(bytesRead);

                output = String.Format("Read: Length: {0}, Data: \r\n{1}",
                    returndata.Length, returndata);
            }

            Console.WriteLine();
            Console.WriteLine(output);
            Console.WriteLine("-------------------------");

            return output.Trim();
        }
    }
}