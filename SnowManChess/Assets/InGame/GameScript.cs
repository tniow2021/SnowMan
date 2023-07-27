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
    Map map1;
    void Start()
    {
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
        map1 = new Map(mapSet1);
        map1.gameScript1 = this;
        
    }
    void Update()
    {
        map1.userInput.AllInput();
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

    public  GameObject _Testtile;
    public  GameObject _EmptyTile;
    public  GameObject _SnowTile;
    public  GameObject _LakeTile;

    public  GameObject _KingPiece;
    public  GameObject _QueenPiece;
    public  GameObject _BishopPiece;
    public  GameObject _KnightPiece;
    public  GameObject _RookPiece;
    public  GameObject _PawnPiece;



    private void Awake()
    {
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

    }
}

//public partial class GameScript //기물, 
//{
//    List<GameObject> ListAllPiece = new List<GameObject>();


//    //기물 생성에 성공하면 true반환 아니면 false반환
//    public void PieceCreate(Vector2Int XY, GameObject pieceObject)//나중에 public bool로 만들어야함.
//    {
//        /*
//         * 좌표와 piece를 받는다.
//         */
//        GameObject NewPiece = Instantiate(pieceObject);
//        PieceScript NewPieceScript=NewPiece.GetComponent<PieceScript>();
//        //게임상에서 벌어질 수 있는 상황에 따른 수많은 조건처리를 거친후에

//        if (List2TileScript[XY.x][XY.y].PutPiece(NewPiece))//기물두기에 성공한 경우
//        {
//            ListAllPiece.Add(NewPiece);
//            print("기물 설치완료");
//        }
//        else  //기물두기에 실패한 경우(이미 타일에 기물이 있음)
//        {
//            print("이미 타일에 다른 기물이 존재합니다.");
//        }

//    }

//    public bool PieceMovement()//기물이동
//    {
//        //7월 26일....오늘은 여기까지
//        return true;
//    }

//    public void AllPieceRemove()
//    {
//        foreach(GameObject obj in ListAllPiece)
//        {
//            if(obj is not null)
//            {
//                obj.GetComponent<PieceScript>().ObjectDestory();
//            }
//        }
//        ListAllPiece.Clear();
//    }

//}

