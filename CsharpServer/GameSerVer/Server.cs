using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;

namespace GameServer
{ 

    
    class Server
    {    
        static  void print(string a)
        {
            Console.WriteLine(a);
        }
        public static void Main()
        {
            print("시발");
            Room room=new Room();
            TcpClient client = new TcpClient();
        }
    }
}
