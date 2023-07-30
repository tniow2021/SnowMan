using System;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    class Program
    {
        static void Main()
        {
            SerVer server = new SerVer();
            server.main();
        }
    }
    public class SerVer
    {
        TcpListener Listner;
        TcpClient client;


        int nbyte;
        byte[] readBuff = new byte[1024];
        public void main()
        {
            Listner = new TcpListener(IPAddress.Any,7777);
            Listner.Start();
            //비동기는 나중에 하고
            Console.WriteLine("클라이언트 받는중..");
            client = Listner.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            while(true)
            {
                if (stream.DataAvailable) if ((nbyte = stream.Read(readBuff, 0, readBuff.Length)) != 0)//connection 종료시 read()는 0을 반환.
                {
                        Console.WriteLine(readBuff[0]);
                        Console.WriteLine(readBuff[2]);
                        Console.WriteLine(readBuff[3]);
                        Console.WriteLine(readBuff[4]);
                        Console.WriteLine(readBuff[5]);
                        Console.WriteLine(readBuff[6]);
                }
                Console.WriteLine("종료하시겠습니까? y/n");
                string exitchar = Console.ReadLine();
                if (exitchar is "y") break;
            }
            Console.WriteLine("연결은 됨..!");
        }
    }

}
