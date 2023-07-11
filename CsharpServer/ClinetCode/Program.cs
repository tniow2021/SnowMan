using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace TcpCli
{
    class communication
    {
        public class Server
        {
            int Port;
            IPAddress Address;
            TcpListener listener;
            TcpClient Client;
            NetworkStream stream;
            public void ServerStart(int _port, IPAddress _Address)
            {
                Port = _port;
                listener = new TcpListener(Address, Port);
                listener.Start();
            }
            public void ClientCherk()
            {
                Client = listener.AcceptTcpClient();
                stream = Client.GetStream();
            }

            byte[] sendBuff = new byte[1024];
            public void Send(string _text)//비연속적. 버퍼크기 제한
            {
                sendBuff = Encoding.Default.GetBytes(_text);
                stream.Write(sendBuff, 0, sendBuff.Length);
            }

            byte[] receiveBuff = new byte[1024];
            int nbyte;
            string incomingMessage = "";
            public string Receive()//비연속적. 버퍼크기제한
            {

                //stream.DataAvailable:버퍼에 읽을 데이터가 남아있는지 여부.
                if (stream.DataAvailable)
                {
                    if ((nbyte = stream.Read(receiveBuff, 0, receiveBuff.Length)) != 0)//connection 종료시 read()는 0을 반환.
                    {
                        incomingMessage = Encoding.Default.GetString(receiveBuff, 0, nbyte);//버퍼가 가득찰 경우를 상관안함.비연속적임.
                        Console.WriteLine(incomingMessage);
                    }
                }
                return incomingMessage;
            }
            public void Close()
            {
                stream.Close();
                Client.Close();
                listener.Stop();
            }
        }
        public class Client
        {
            int Port;
            string IP;
            TcpClient client;
            NetworkStream stream;
            public void ClientStart(int _port, string _IP = "127.0.0.1")
            {
                Port = _port;
                IP = _IP;
                client = new TcpClient(IP, Port);
                stream = client.GetStream();
            }

            byte[] sendBuff = new byte[1024];
            public void Send(string _text)//비연속적. 버퍼크기 제한
            {
                sendBuff = Encoding.Default.GetBytes(_text);
                stream.Write(sendBuff, 0, sendBuff.Length);
            }

            byte[] receiveBuff = new byte[1024];
            int nbyte;
            string incomingMessage = "";
            public string Receive()//비연속적. 버퍼크기제한
            {
                //stream.DataAvailable:버퍼에 읽을 데이터가 남아있는지 여부.
                if (stream.DataAvailable)
                {
                    if ((nbyte = stream.Read(receiveBuff, 0, receiveBuff.Length)) != 0)//connection 종료시 read()는 0을 반환.
                    {
                        incomingMessage = Encoding.Default.GetString(receiveBuff, 0, nbyte);//버퍼가 가득찰 경우를 상관안함.비연속적임.
                        Console.WriteLine(incomingMessage);
                    }
                }
                return incomingMessage;
            }
            public void Close()
            {
                stream.Close();
                client.Close();
            }
        }
    }
    class Program
    {
        static void Main()
        {
            communication.Client client = new communication.Client();
            Console.WriteLine("연결하시겠습니까? y/n");
            string t = Console.ReadLine();
            if (t is "y")
            {
                client.ClientStart(7777);
                while (true)
                {
                    Console.Write("보낼 문자열:");
                    client.Send(Console.ReadLine());
                    Console.Write("받은 문자열");
                    Console.WriteLine(client.Receive());
                }
            }
                


        }
        static void main(string[] args)
        {
            //Decoder.Convert();
            //연결할때까지 기다렸다가 리턴한다.

            Console.WriteLine("연결하시겠습니까? y/n");

            string t = Console.ReadLine();


            if (t is "y")
            {
                TcpClient client = new TcpClient("127.0.0.1", 6974);
                Console.WriteLine("연결함..!");
                NetworkStream stream = client.GetStream();
                Console.WriteLine("스트림얻음");

                byte[] writebuff = new byte[1024];
                byte[] readbuff = new byte[1024];
                int nbyte;
                char[] charbuff = new char[100];


                Decoder decoder;//얘는 연속적 변환할때 쓰는 애.
                while (true)
                {
                    Console.WriteLine("보낼 메세지:");
                    string msg = Console.ReadLine();
                    writebuff = Encoding.Default.GetBytes(msg);
                    stream.Write(writebuff, 0, writebuff.Length);
                    Console.WriteLine("전송완료.이하 수신된 메세지.");

                    //read()함수가 버퍼값을 받느라 계속 무한루프를 돌고 있어서 
                    //중간에 멈추는 오류가 있다. 버퍼가 비었으면 넘어가는 코드를 추가하도록 하자.

                    Console.WriteLine(stream.DataAvailable);
                    if (stream.DataAvailable) if ((nbyte = stream.Read(readbuff, 0, readbuff.Length)) != 0)//connection 종료시 read()는 0을 반환.
                    {
                        //int nchar=Encoding.Default.GetChars(readbuff, 0, nbyte, charbuff,0);
                        string incomingMessage = Encoding.Default.GetString(readbuff, 0, nbyte);//버퍼가 가득찰 경우를 상관안함.비연속적임.
                        Console.WriteLine(incomingMessage);

                    }
                    Console.WriteLine("종료하시겠습니까? y/n");
                    string exitchar = Console.ReadLine();
                    if (exitchar is "y") break;
                }

                stream.Close();
                client.Close();
            }

        }
    }
}