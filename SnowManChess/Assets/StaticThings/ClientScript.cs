using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;

/*
 * 클라이언트-서버 연결 구조설게(7월 26일)
 * 
 * 데이터 흐름
 * 
 * -----------------클라이언트--------------------------------------
 * 보내기:
 * GameScript->MakeDataSet 클래스에 값 넣기->MakeDataSet의 함수호출
 * ->MakeBinary(인코딩)->communication---->[서버]
 * 
 * 받기: (보내기의 역순이다)
 * 
 * -----------------서버 (클라이언트의 역순이다)--------------
 * 받기:
 * communication->MakeBinary(디코딩)->MakeDataSet의 데이터조합함수 호출
 * ->MakeDataSet에서 DataSet 인스턴스를 조합하고 게임로직처리클래스에 반환
 * 이때 DataSet은 게임로직처리클래스가 좀더 잘알아먹게 약간의 추가 가공과정을 거친다.
 * 
 * 보내기: ( 받기의 역순이다)
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
    public string ServerIP;//멘토님에게 보낼 빌드는 화면처음에 ip와 포트를 연결할 수 있게 만들기
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



        //테스트
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
     * 순서 제 3번째
     * MakeBinary에서 받은 바이트열은 알아서 잘 딱 깔끔하고 센스있게 전송하고 통신과정을 제어하고
     * 관리한다.
     * 
     * communication 클래스는 주고받기를 모두한다.
     * update문을 이용한다.
     * 
     * 
     * 연결이 안되면 빠르게 다시 시도해보고 그래도 안되면 화면에 빙글빙글이를 띄우고
     * 일정시간 간격으로 연결을 시도해보다 안되면 멀티플레이 씬에서 나가고 메세지를 띄운다. 
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
        if(client.Connected is true)//연결이 되었을때
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
    public void Receive()
    {
        if (client.Connected is true)
        {
            if (stream.DataAvailable) if ((nbyte = stream.Read(readBuff, 0, readBuff.Length)) != 0)//connection 종료시 read()는 0을 반환.
            {
                
            }
        }
    }
}
public partial class ClientScript
{
    void Decoding(List<byte> dataSet)
    {
        //구분자로 나눠진 하나의 데이터셋을 받는다.
    }
    void MakeDataSet(List<byte> data)
    {
        //첫번째에는 구분자
        //두번째에는 길이
        //세번째에는 데이터

        List<byte> dataSet = new List<byte>();
        dataSet.Add(255);//구분자
        dataSet.Add(((byte)data.Count));
        dataSet.AddRange(data);
        Send(dataSet);
    }
}
public partial class ClientScript//올라가는 놈
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
public partial class ClientScript//내려가는 놈
{
    public void FromServer(MoveOrder moveOrder)
    {

    }
}








