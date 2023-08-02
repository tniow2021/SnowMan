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


    Communication communi=Communication.GetInstance();

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        DontDestroyOnLoad(ClinetGameObject);

        
        communi.Connect(ServerIP, ServerPort);
        //테스트
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
public partial class ClientScript//올라가는 놈
{
    
    public void SendToServer()
    {

    }

}











