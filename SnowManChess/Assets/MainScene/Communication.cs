using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;

public partial class Communication
{
    static Communication instance = new Communication();
    public static Communication GetInstance()
    {
        return instance;
    }
}//싱글톤
public partial class Communication
{ 
    TcpClient client;
    NetworkStream stream;
    public void Connect(string IP, int port)
    {
        client = new TcpClient(IP, port);
        stream = client.GetStream();
    }
    public void Send(List<byte> dataSet)
    {
        if (client.Connected is true)//연결이 되었을때
        {
            byte[] binary = dataSet.ToArray();
            stream.Write(binary);
        }
        else//연결이 안되있을때 
        {

        }
    }

    int nbyte;
    byte[] readBuff = new byte[1024];
    List<byte> ByteList = new List<byte>();
    public void Receive()
    {
        if (client.Connected is true)
        {
            if (stream.DataAvailable) if ((nbyte = stream.Read(readBuff, 0, readBuff.Length)) != 0)//connection 종료시 read()는 0을 반환.
            {
                MonoBehaviour.print(readBuff);
                for (int i = 0; i < nbyte; i++)
                {

                    
                    if (readBuff[i] == 255)
                    {
                        DataIng.Decoding(ByteList);
                        ByteList.Clear();
                        continue;
                    }
                    ByteList.Add(readBuff[i]);
                }
            }
        }
    }
    
}

