using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Runtime.InteropServices;

class communication
{
    public class Server
    {
        int Port;
        IPAddress Address;
        TcpListener listener;
        TcpClient Client;
        NetworkStream stream;
        public void ServerStart(int _port, string IP = "0.0.0.0")
        {
            Port = _port;
            Address = IPAddress.Parse(IP);
            listener = new TcpListener(Address, Port);
            listener.Start();
        }
        public void ServerStart(int _port, IPAddress _address)
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
        public void Send(string _text)//�񿬼���. ����ũ�� ����
        {
            sendBuff = Encoding.Default.GetBytes(_text);
            stream.Write(sendBuff, 0, sendBuff.Length);
        }

        byte[] receiveBuff = new byte[1024];
        int nbyte;
        string incomingMessage = "";
        public string Receive()//�񿬼���. ����ũ������
        {

            //stream.DataAvailable:���ۿ� ���� �����Ͱ� �����ִ��� ����.
            if (stream.DataAvailable)
            {
                if ((nbyte = stream.Read(receiveBuff, 0, receiveBuff.Length)) != 0)//connection ����� read()�� 0�� ��ȯ.
                {
                    incomingMessage = Encoding.Default.GetString(receiveBuff, 0, nbyte);//���۰� ������ ��츦 �������.�񿬼�����.
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
        public bool Connectable()
        {
            return client.Connected;
        }
        byte[] sendBuff = new byte[1024];
        public void Send(string _text)//�񿬼���. ����ũ�� ����
        {
        
            sendBuff = Encoding.Default.GetBytes(_text);
            stream.Write(sendBuff, 0, sendBuff.Length);
        
        }

        byte[] receiveBuff = new byte[1024];
        int nbyte;
        string incomingMessage = "";
        public string Receive()//�񿬼���. ����ũ������
        {
       
            //stream.DataAvailable:���ۿ� ���� �����Ͱ� �����ִ��� ����.
            if (stream.DataAvailable)
            {
                if ((nbyte = stream.Read(receiveBuff, 0, receiveBuff.Length)) != 0)//connection ����� read()�� 0�� ��ȯ.
                {
                    incomingMessage = Encoding.Default.GetString(receiveBuff, 0, nbyte);//���۰� ������ ��츦 �������.�񿬼�����.

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



public class IDPS : MonoBehaviour
{
    public TMP_InputField ID;
    public TMP_InputField PS;
    private string idtest = "user";
    private string pstest = "password";
    public TMP_Text �α��ι���;

    public string IP = "";

    private void Start()
    {
        
    }

    communication.Client client;
    public void ��������()
    {
        client = new communication.Client();
        client.ClientStart(7777, IP);

    }
    public void �Է¹�ư()
    {
        //if (string.Equals(ID.text, idtest) && string.Equals(PS.text, pstest))
        if(true)
        {
            if(client.Connectable())
            {
                client.Send($"INSERT INTO Players(UserName,UserPs) VALUES('{ID.text}','{PS.text}');");
                Debug.Log("�α��μ���!");
                �α��ι���.text = "�α��μ���!";
                
            }
            else
            {
                Debug.Log("�������ῡ ����!");
                �α��ι���.text = "�������ῡ ����!";
            }
           
  
        
        }
        else
        {
            Debug.Log("�α��ν���...");
            �α��ι���.text = "�α��ν���...";
        }
        ID.text = "";
        PS.text = "";
    }

}
