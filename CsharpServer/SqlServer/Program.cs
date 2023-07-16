
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Net.Sockets;
using System.Net;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Utilities;
using static SqlServer.communication;


namespace SqlServer
{
    class GameSQL
    {
        string SqlServerAdress;
        string UserName;
        string UserPassword;
        string DataBaseName;
        public GameSQL
        (
        string _SqlServerAdress,
        string _DataBaseName,
        string _UserName,
        string _UserPassword
        )
        {
            // ArgumentCherk
            {
                bool ArgumentCherk = true;
                if (string.IsNullOrEmpty(_SqlServerAdress)) ArgumentCherk = false;
                else if (string.IsNullOrEmpty(_UserName)) ArgumentCherk = false;
                else if (string.IsNullOrEmpty(_UserPassword)) ArgumentCherk = false;
                else if (string.IsNullOrEmpty(_DataBaseName)) ArgumentCherk = false;

                if (ArgumentCherk is false)
                {
                    Console.WriteLine("new GameSQL(): Parameters are not perfect.");
                    return;
                }
            }
            SqlServerAdress = _SqlServerAdress;
            UserName = _UserName;
            UserPassword = _UserPassword;
            DataBaseName = _DataBaseName;
        }

        MySqlConnection conn;
        MySqlCommand command;
        public void Connection()
        {
            string strConn =
                $"Server ={SqlServerAdress}; Database = {DataBaseName}; Uid = {UserName}; Pwd = {UserPassword};";
            Console.WriteLine(strConn);
            conn = new MySqlConnection();
            conn.ConnectionString = strConn;

            Console.WriteLine("Connecting to MySQL...");
            conn.Open();
            command = new MySqlCommand();
            command.Connection = conn;

        }
        public void Close()
        {
            //System.Data.ConnectionState는 MySqlConnection.State의 값으로 아마도 System에서 자체적으로 상수값을 지원해주는듯.
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                Console.WriteLine("error. Game.Close: already closed");
                return;
            }
            conn.Close();
        }
        public void Insert(string Table, string Values)
        {
            Console.WriteLine($"INSERT INTO {Table} VALUES({Values});");
            command.CommandText = $"INSERT INTO {Table} VALUES({Values});";
            if (command.ExecuteNonQuery() != 1) //ExecuteNonQuery()의 반환값은 변동되는 행의 갯수임으로 INSERT문을 보낼땐 1이된다. 1이 아니거나 -1이면 오류.
            {
                Console.WriteLine("Failed to insert data.");
            }
        }
        public void Query(string _Query)
        {
            command.CommandText = _Query;
            if (command.ExecuteNonQuery() != 1) //ExecuteNonQuery()의 반환값은 변동되는 행의 갯수임으로 INSERT문을 보낼땐 1이된다. 1이 아니거나 -1이면 오류.
            {
                Console.WriteLine("Failed to insert data.");
            }
        }
        public void Select(string Table, string Fields, string Where = "")
        {
            Console.WriteLine($"Select {Fields} from {Table} Where {Where};");

            string TempCommandText;
            if (String.IsNullOrEmpty(Where))
            {
                TempCommandText = $"Select {Fields} from {Table};";
            }
            else
            {
                TempCommandText = $"Select {Fields} from {Table} Where {Where};";
            }

            command.CommandText = TempCommandText;
            MySqlDataReader table = command.ExecuteReader();

            while (table.Read())
            {
                for (int i = 0; i < table.FieldCount; i++)
                {
                    Console.Write($"{table[i]}, ");
                }
                Console.Write('\n');

            }
            table.Close();
        }
    }
    class communication
    {
        public class Server
        {
            int Port;
            IPAddress Address;
            TcpListener listener;
            TcpClient Client;
            NetworkStream stream;
            public void ServerStart(int _port, string IP="0.0.0.0")
            {
                Port = _port;
                Address=IPAddress.Parse(IP);
                listener = new TcpListener(Address, Port);
                listener.Start();
            }
            public void ServerStart(int _port,IPAddress _address)
            {
                Port = _port;
                Address = _address;
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
                incomingMessage = "";
                //stream.DataAvailable:버퍼에 읽을 데이터가 남아있는지 여부.
                if (stream.DataAvailable)
                {
                    if ((nbyte = stream.Read(receiveBuff, 0, receiveBuff.Length)) != 0)//connection 종료시 read()는 0을 반환.
                    {
                        incomingMessage = Encoding.Default.GetString(receiveBuff, 0, nbyte);//버퍼가 가득찰 경우를 상관안함.비연속적임.
                        //Console.WriteLine(incomingMessage);
                    }
                }
                return incomingMessage;
            }
          
            public void AllClose()
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
            public bool Connectable()
            {
                return client.Connected;
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


    //https://learn.microsoft.com/en-us/dotnet/api/system.text.encoder.convert?view=net-7.0
    class Program
    {
        static void main()
        {
            GameSQL gamesql = new GameSQL("localhost", "gamedb", "Unity", "password");
            gamesql.Connection();
            communication.Server server= new communication.Server();
            Console.WriteLine("시작하시겠습니까? y/n");
            
            string t = Console.ReadLine();
            if (t is "y")
            {
                server.ServerStart(7777);
                server.ClientCherk();
                Console.WriteLine("클라이언트와 연결되었습니다");
                while (true)
                {
                    
                    
                    //Console.Write("보낼 문자열:");
                    //server.Send(Console.ReadLine());
                    string query = server.Receive();
                    if(query.Length > 5)//일반 데이터와 sql쿼리를 구분하기위한 임시방편
                    {
                        gamesql.Query(query);
                        Console.Write("받은 문자열");
                        Console.WriteLine(query);
                    }
                    Thread.Sleep(20);
                    Console.WriteLine("대기중");
                    
                    
                }
                server.AllClose();
            }

        }
        static void Main(string[] args)
        {
            int temp = 1;
            if (temp == 1)
            {
                GameSQL a = new GameSQL("localhost", "gamedb", "Unity", "password");
                a.Connection();
                a.Insert("friend", "'user123','qwert12345'");
                //a.Select("abc", "*");
                a.Close();
                //
            }
            else if (temp == 2)
            {
                Console.WriteLine("시작하시겠습니까? y/n");
                string t = Console.ReadLine();
                if (t is "y")
                {
                    TcpListener listener = new TcpListener(IPAddress.Any, 6974);
                    listener.Start();
                    Console.WriteLine("서버실행됨.");
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("클라이언트와 연결됨.");
                    NetworkStream stream = client.GetStream();
                    Console.WriteLine("스트림얻음.");

                    byte[] writebuff = new byte[1024];
                    byte[] readbuff = new byte[1024];
                    int nbyte;
                    char[] charbuff = new char[100];


                    //Decoder decoder;//얘는 연속적 변환할때 쓰는 애.
                    while (true)
                    {
                        Console.WriteLine("보낼 메세지:");
                        string msg = Console.ReadLine();
                        writebuff = Encoding.Default.GetBytes(msg);
                        stream.Write(writebuff, 0, writebuff.Length);
                        Console.WriteLine("전송완료. 이하 수신된 메세지.");

                        Console.WriteLine(stream.DataAvailable);

                        //stream.DataAvailable:버퍼에 읽을 데이터가 남아있는지 여부.
                        if (stream.DataAvailable)if ((nbyte = stream.Read(readbuff, 0, readbuff.Length)) != 0)//connection 종료시 read()는 0을 반환.
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
                    listener.Stop();

                }
            }

        }
    }
}
