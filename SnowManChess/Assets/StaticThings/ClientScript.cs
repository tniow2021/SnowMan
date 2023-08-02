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


    Communication communi=Communication.GetInstance();

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        DontDestroyOnLoad(ClinetGameObject);

        
        communi.Connect(ServerIP, ServerPort);
        //�׽�Ʈ
        //Command.ToSer.PieceCreate order = new Command.ToSer.PieceCreate();
        //order.pieceKind1 = PK.King;
        //order.X2 = 5;
        //order.y3 = 7;
        //communi.Send(DataIng.MakeDataSet(DataIng.Encoding(order)));
    }

    void Update()
    {
        //communi.Receive();
    }
}
public partial class ClientScript//�ö󰡴� ��
{
    
    public void SendToServer()
    {

    }

}











