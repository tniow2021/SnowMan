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
public static class communication
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
}
public class ClientScript : MonoBehaviour
{
    
    public GameObject ClinetGameObject;
    public string ServerIP;//멘토님에게 보낼 빌드는 화면처음에 ip와 포트를 연결할 수 있게 만들기
    public int ServerPort;

    public string PresentScene;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(ClinetGameObject);
    }

    void Update()
    {

    }
}
public static class MakeBinary
{
    /*
     * 순서 제 2번째
     * MakeDetaSet에서 넘겨받은 DataSet 인스턴스를 
     * 해체해서 하나의 바이트열로 만든다. 만들어진 바이트 열은 
     * communication 클래스로 보낸다.
     * 
     * 이 클래스에서 바이너리 데이터의 구조를 정의한다
     * 
     */
}
public class DataSet
{
    /*
     * 데이터셋을 여기에 정의한다.
     */
}
public static class MakeDetaSet
{
    static DataSet dataSet = new DataSet();
    /*
     * 순서 제 1번째
     * 게임스크립트에서 이 정적클래스에 접근해서 이안에 있는 변수들을 채우고
     * 변수를 다 채웠으면 특정 함수를 실행시켜 만들어진 DataSet 인스턴스을 MakeBinary에 보낸다.
     * 
     */
}



