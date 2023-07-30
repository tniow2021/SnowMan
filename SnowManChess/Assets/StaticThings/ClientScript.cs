using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;

/*
 * Ŭ���̾�Ʈ-���� ���� ��������(7�� 26��)
 * 
 * ������ �帧
 * 
 * -----------------Ŭ���̾�Ʈ--------------------------------------
 * ������:
 * GameScript->MakeDataSet Ŭ������ �� �ֱ�->MakeDataSet�� �Լ�ȣ��
 * ->MakeBinary(���ڵ�)->communication---->[����]
 * 
 * �ޱ�: (�������� �����̴�)
 * 
 * -----------------���� (Ŭ���̾�Ʈ�� �����̴�)--------------
 * �ޱ�:
 * communication->MakeBinary(���ڵ�)->MakeDataSet�� �����������Լ� ȣ��
 * ->MakeDataSet���� DataSet �ν��Ͻ��� �����ϰ� ���ӷ���ó��Ŭ������ ��ȯ
 * �̶� DataSet�� ���ӷ���ó��Ŭ������ ���� �߾˾Ƹ԰� �ణ�� �߰� ���������� ��ģ��.
 * 
 * ������: ( �ޱ��� �����̴�)
 * ---------------------------------------------------------------
 * 
 * 
 */
public partial class ClientScript : MonoBehaviour
{
    public static ClientScript instance;
    public static ClientScript GetClientScript()
    {
        return instance;
    }
    public GameObject ClinetGameObject;
    public string ServerIP;//����Կ��� ���� ����� ȭ��ó���� ip�� ��Ʈ�� ������ �� �ְ� �����
    public int ServerPort;

    public string PresentScene;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        DontDestroyOnLoad(ClinetGameObject);
        Connect(ServerIP, ServerPort);



        //�׽�Ʈ
        PieceCreate order = new PieceCreate();
        order.pieceKind = PK.King;
        order.XY = new Vector2Int(5, 7);
        SendToServer(order);
    }

    void Update()
    {

    }
}
public partial class ClientScript
{
    /*
     * ���� �� 3��°
     * MakeBinary���� ���� ����Ʈ���� �˾Ƽ� �� �� ����ϰ� �����ְ� �����ϰ� ��Ű����� �����ϰ�
     * �����Ѵ�.
     * 
     * communication Ŭ������ �ְ�ޱ⸦ ����Ѵ�.
     * update���� �̿��Ѵ�.
     * 
     * 
     * ������ �ȵǸ� ������ �ٽ� �õ��غ��� �׷��� �ȵǸ� ȭ�鿡 ���ۺ����̸� ����
     * �����ð� �������� ������ �õ��غ��� �ȵǸ� ��Ƽ�÷��� ������ ������ �޼����� ����. 
     * 
     */

    TcpClient client;
    NetworkStream stream;
    void Connect(string IP,int port)
    {
        client = new TcpClient(IP, port);
        stream = client.GetStream();
    }
    public void Send(List<byte> dataSet)
    {
        if(client.Connected is true)//������ �Ǿ�����
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
    public void Receive()
    {
        if (client.Connected is true)
        {
            if (stream.DataAvailable) if ((nbyte = stream.Read(readBuff, 0, readBuff.Length)) != 0)//connection ����� read()�� 0�� ��ȯ.
            {
                
            }
        }
    }
}
public partial class ClientScript
{
    void Decoding(List<byte> dataSet)
    {
        //�����ڷ� ������ �ϳ��� �����ͼ��� �޴´�.
    }
    void MakeDataSet(List<byte> data)
    {
        //ù��°���� ������
        //�ι�°���� ����
        //����°���� ������

        List<byte> dataSet = new List<byte>();
        dataSet.Add(255);//������
        dataSet.Add(((byte)data.Count));
        dataSet.AddRange(data);
        Send(dataSet);
    }
}
public partial class ClientScript//�ö󰡴� ��
{
    public void SendToServer(PieceCreate order)
    {
        List<byte> data = new List<byte>();
        data.Add(((byte)Command.Kind.PieceCreate));
        data.Add(((byte)order.pieceKind));
        data.Add(((byte)order.XY.x));
        data.Add(((byte)order.XY.y));
        MakeDataSet(data);
    }
    public void SendToServer()
    {

    }
  
}
public partial class ClientScript//�������� ��
{
    public void FromServer(MoveOrder moveOrder)
    {

    }
}








