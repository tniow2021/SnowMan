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
}//�̱���
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
        if (client.Connected is true)//������ �Ǿ�����
        {
            byte[] binary = dataSet.ToArray();
            stream.Write(binary);
        }
        else//������ �ȵ������� 
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
            if (stream.DataAvailable) if ((nbyte = stream.Read(readBuff, 0, readBuff.Length)) != 0)//connection ����� read()�� 0�� ��ȯ.
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

