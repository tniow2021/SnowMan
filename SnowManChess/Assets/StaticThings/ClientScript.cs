using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
public class communication
{
    /*
     -데이터구조체1,2,3
    유저 데이터
    실시간 게임 데이터
    -데이터 구조체 검사
    -연결확인
    연결안되있으면 재연결시도
    바로 재연결안되면 재연결될때까지 모든 작업을 마지막 연결타임때 상태로 되돌린뒤 
    화면에 골뱅이 띄우기 이때 나가기 버튼도 있어야함

     보내기()
    {
    }
     */
}
public class ClientScript : MonoBehaviour
{
    
    public GameObject ClinetGameObject;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(ClinetGameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
