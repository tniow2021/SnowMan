using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public partial class GameScript : MonoBehaviour
{

    /*
     * 게임시작시에만 필요한 정보
     * :체스판 크기, 타일별 정보들(지형, 자원)
     * 실시간 통신정보(변경된 것만 통신, 주기적으로 전체를 검사)
     * :
     * 기물 이동
     * 타일정보 변경내역
     * 
     * 
     * 만들어야할 것 단계적 계획
     * :
     * clinet script를 허브로 하는 문자열 통신.
     * 기물이동 동기화
     * 시작시통신데이터, 실시간 데이터 구조체 짜기
     * client script작성
     * 잠재적 턴교환과 마스킹 구현
     * 서버작성
     * 
     * 드래그 이동경로설정
     * 핑(오브젝트움직임의 최소단위 구현. 한 50ms로)
     */
    public static GameScript instance;
    Map map1;
    void Start() 
    {
        instance = this.gameObject.GetComponent<GameScript>();
        clientScript = ClientScript.GetClientScript();
        GameObject MapObject1 = new GameObject();
        MapSet mapSet1 = new MapSet()
        {
            MapObject = MapObject1,
            X = 9,
            Y = 9,
            TileSet = new TK[,]
            {
                {TK.Lake,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow},
                {TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow},
                {TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow},
                {TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow},
                {TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow},
                {TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow},
                {TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow},
                {TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow},
                {TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow}
            },
            PieceSet = new PK[,]
            {
                {PK.King,PK.none,PK.Knight,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none },
                {PK.none,PK.none,PK.none,PK.none,PK.Knight,PK.none,PK.none,PK.none,PK.none },
                {PK.King,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none },
                {PK.King,PK.none,PK.none,PK.none,PK.none,PK.Rook,PK.none,PK.none,PK.none },
                {PK.King,PK.none,PK.none,PK.none,PK.Rook,PK.none,PK.none,PK.none,PK.none },
                {PK.King,PK.none,PK.Queen,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none },
                {PK.King,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none },
                {PK.King,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none },
                {PK.King,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none }
            }
        };
        Map.insatance = new Map();
        Map.insatance.MapCreate(mapSet1);
        
    }
}
public partial class GameScript //서버로 올라가는 길
{
    public ClientScript clientScript; 
    public void FromMap(Command.ThingOntile.Post post)
    {
        //
        if(clientScript is not null)clientScript.FromGame(post);
    }
}
public partial class GameScript //Map으로 내려가는 길
{
    public void FromClient(Command command)
    {

    }
    public void Action()
    {

    }
}
public partial class GameScript //사전 설정
{
    //타일종류
    public static GameObject Testtile;
    public static GameObject EmptyTile;
    public static GameObject SnowTile;
    public static GameObject LakeTile;

    //기물종류
    public static GameObject KingPiece;
    public static GameObject QueenPiece;
    public static GameObject BishopPiece;
    public static GameObject KnightPiece;
    public static GameObject RookPiece;
    public static GameObject PawnPiece;

    public GameObject _Testtile;
    public GameObject _EmptyTile;
    public GameObject _SnowTile;
    public GameObject _LakeTile;

    public GameObject _KingPiece;
    public GameObject _QueenPiece;
    public GameObject _BishopPiece;
    public GameObject _KnightPiece;
    public GameObject _RookPiece;
    public GameObject _PawnPiece;

    //입력관련

    public float _TouchDecisionTime = 0.7f;
    public static float TouchDecisionTime;
    public CameraScript _CameraScript;
    public static CameraScript cameraScript;


    private void Awake()
    {
        UserInput.instance = GetComponent<UserInput>();

        Testtile = _Testtile;
        EmptyTile = _EmptyTile;
        SnowTile = _SnowTile;
        LakeTile = _LakeTile;

        KingPiece = _KingPiece;
        QueenPiece = _QueenPiece;
        BishopPiece = _BishopPiece;
        KnightPiece = _KnightPiece;
        RookPiece = _RookPiece;
        PawnPiece = _PawnPiece;

        cameraScript = _CameraScript;
        TouchDecisionTime = _TouchDecisionTime;
    }
}




