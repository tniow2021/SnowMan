using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * 사용방법:
 * Map map1 인스턴스를 맴의 껍데기를 만든다.
 * 
 * MapCreate에 MapSet변수를 파라미터로 넘김으로서 
 * 맵에 타일과 기물이 종류별로 세팅되고
 * List<List<MapArea>>를 반환받는다.
 * 
 * map1 인스턴스를 통해서 맵에 명령을 내릴 수 있고
 * 반환받은 MapArea 인스턴스를 통해서 맵의 현재상황을 알 수 있다.
 * 
 * Map클래스 형태:
 * MapArea에 대한 입력(값수정)은 오로지 서버만 할 수 있다.(원격으로 메소드를 호출하는 걸 짜자)
 * 정확히 말하자면
 * 게임시작
 * :
 * 서버가 맵셋데이터 넘김->mapcreate->초기 기물배치함->다시 서버한테 맵셋데이터 넘김.
 * 서버가 두 유저의 맵셋데이터를 모두 받아들었다면 게임시작신호를 보냄
 * 이후 
 * 유저입력->바뀐것만 서버전달->검사 및 게임로직처리->서버입력(서버가 클라이언트에게 입력)
 * ->서버가 준 수정값과 현재 MapArea값을 비교해 변화값만 
 * Map클래스 선에서 기물이동(),타일변화(),기물죽음,아이템드랍()등의 함수를 호출시켜 맵에 반영한다
 * 
 * 즉 GameScript는 직접적으로 맵을 바꿀 수 없고 서버를 거쳐서 간다.
 * GameScript는 업데이트를 통해(1틱당.) MapArea반영()함수를 계속 호출해서 맵을 변화시킨다.
 * 
 * 속성
 * :
 * mapArea
 * mapSet
 * 유저가 현재 선택한 타일
 * 유저가 현재 선택한 기물체크
 * 
 * 메소드
 * 유저가입력():
 * 유저가 현재 선택한 타일체크함수(gameScript에서 타일스크립트를 out으로 받음)
 * 유저가 현재 선택한 기물체크함수(gameScript에서 기물스크립트를 out으로 받음)
 * ->gameScript에서 계속 update돌려가며 확인.
 * 
 * 
 * 유저에게 출력():
 * MapArea반영함수
 * -> 유저의 게임화면을 업데이트함
 * 서버가입력():
 * 서버에게 MapArea형식의 맵데이터를 전달받고 그걸 MapArea에 넣음
 * 서버에게 출력():
 * 유저가 내린 명령을 서버에게 전달.ㄴ
 * 서버로직:
 * 
 */
public enum TK//Tile kind
{
    none,
    test,
    Snow,
    Lake
}
public enum PK
{
    none,
    King,
    Queen,
    Bishop,
    Knight,
    Rook,
    Pawn
}
public enum IK
{
}
public enum BK
{ 
}
public class MapSet
{
    public int X;
    public int Y;
    public TK[,] TileSet;
    public PK[,] PieceSet;
    public IK[,] ItemSet;
    public BK[,] BulidngSet;
    public GameObject MapObject;
}

public class MapArea //맵진행상황
{
    public GameObject Tile;
    public GameObject Piece;
    public GameObject Item;
    public GameObject Buliding;
}

public partial class Map : MonoBehaviour
{
    public GameScript gameScript1;

    //맵 세트
    MapSet mapSet;
    //입력
    public UserInput userInput;
    //가상 맵
    public List<List<MapArea>> mapArea = new List<List<MapArea>>();
    //---------------------------------------리스트----------------------------------------
    //타일오브젝트 총리스트
    public List<List<GameObject>> List2TileObject = new List<List<GameObject>>();
    //타일스크립트 총리스트
    public List<List<TileScript>> List2TileScript = new List<List<TileScript>>();
    //기물오브젝트 총리스트
    public List<GameObject> PieceList = new List<GameObject>();
    //아이템오브젝트 총리스트
    public List<GameObject> ItemList = new List<GameObject>();
    //건뭉오브젝트 총리스트
    public List<GameObject> BulidingList = new List<GameObject>();

    //----------------------------------------사전---------------------------------------------
    //타일
    public static Dictionary<TK, GameObject> TileDictionnary = new Dictionary<TK, GameObject>()
    {
        {TK.none,GameScript.EmptyTile },//타일셋에서는 None이면 빈자리를 표현하는 게임오브젝트를 넣어주고
        {TK.test, GameScript.Testtile},
        {TK.Snow,GameScript.SnowTile },
        {TK.Lake,GameScript.LakeTile },
    };
    //기물
    public static Dictionary<PK, GameObject> PieceDictionary = new Dictionary<PK, GameObject>()
    {
        {PK.none, null}, //기물셋에서는 None이면 NULL값으로 처리한다. NUll값이면 기물을 배치안하는 걸로한다.
        {PK.King, GameScript.KingPiece},
        {PK.Queen,GameScript.QueenPiece },
        {PK.Bishop,GameScript.BishopPiece },
        {PK.Knight,GameScript.KnightPiece },
        {PK.Rook,GameScript.RookPiece },
        {PK.Pawn,GameScript.PawnPiece }
    };
    //아이템
    public static Dictionary<IK, GameObject> ItemDeictionary = new Dictionary<IK, GameObject>()
    {
    };
    //건물
    public static Dictionary<BK, GameObject> BulidingDeictionary = new Dictionary<BK, GameObject>()
    {
    };
    //----------------------------------------메소드-------------------------------------
    public Map(MapSet _mapSet)
    {
        _mapSet.MapObject.AddComponent<UserInput>();
        userInput=_mapSet.MapObject.GetComponent<UserInput>();
        userInput.map1 = this;
        MapCreate(_mapSet);
        ClassConnect();
    }

    void MapCreate(MapSet _mapSet)
    {
        mapSet = _mapSet;
        for (int i=0;i<mapSet.X;i++)
        {
            mapArea.Add(new List<MapArea>());
            for (int j = 0; j< mapSet.X; j++)
            {
                mapArea[i].Add(new MapArea());
            }
        }

        //메모리 만들기
        for (int x = 0; x < mapSet.X; x++)
        {
            List<TileScript> newList1TileScript = new List<TileScript>();
            List<GameObject> newList1TileObject = new List<GameObject>();
            for (int y = 0; y < mapSet.Y; y++)// 오브젝트 단위 할당
            {
                //타일 집어넣기
                //맵셋의 타일셋에서 TK값을 불러와 그걸로 타일딕셔너리에서 검색
                GameObject Originaltile = TileDictionnary[mapSet.TileSet[x, y]];
                if (Originaltile is not null)
                {

                    //TileSet[x,y]값이 none이더라도 EmptyTile이라는 친구가 생성된다.
                    GameObject newTileObject = Instantiate(Originaltile);//중요
                    TileScript newtileScript = newTileObject.GetComponent<TileScript>();
                    //맵아레아에 넣는다.
                    
                    mapArea[x][y].Tile=newTileObject;
                    //MapObject의 자식으로 만든다.
                    newTileObject.transform.parent = mapSet.MapObject.transform;
                    //MapObject아래 좌표값으로 배치한다
                    newTileObject.transform.localPosition = new Vector3(x, y, 0);

                    newtileScript.coordinate = new Vector2Int(x, y);

                    newList1TileObject.Add(newTileObject);
                    newList1TileScript.Add(newtileScript);
                }

                //기물집어넣기
                if (PieceDictionary.TryGetValue(mapSet.PieceSet[x, y], out GameObject OriginalPiece))
                {
                    if (OriginalPiece is not null)
                    {
                        GameObject newPiece = Instantiate(OriginalPiece);
                        //맵아레아에 넣는다.
                        mapArea[x][y].Piece = newPiece;
                        //MapObject의 자식으로 만든다.
                        newPiece.transform.parent = mapSet.MapObject.transform;
                        //MapObject아래 좌표값으로 배치한다(MainScript의 설정을 따른다)
                        newPiece.transform.localPosition = new Vector3(x, y, 0) + MainScript.LocalPositionOfPieceOntile;
                    }
                }

                //아이템 집어넣기
                //건물집어넣기
            }
            List2TileObject.Add(newList1TileObject);
            List2TileScript.Add(newList1TileScript);
        }
    }
    void ClassConnect()
    {
         //Map과 기물,타일, 아이템, 건물등의클래스를 서로 연결하기(상호참조하기)
        foreach(List<TileScript> obj in List2TileScript)
        {
            foreach(TileScript objj in obj)
            {
                objj.map1 = this;
            }
        }
        foreach(GameObject obj in PieceList)
        {
            obj.GetComponent<PieceScript>().map1 = this;
        }
        foreach (GameObject obj in ItemList)
        {
            obj.GetComponent<ItemScript>().map1 = this;
        }
        foreach (GameObject obj in BulidingList)
        {
            obj.GetComponent<BuildingScript>().map1 = this;
        }
    }
    public List<List<MapArea>> GetMapArea()
    {
        return mapArea;
    }

    //기물이동(좌표,좌펴)

    //온난화짐행();
}
public partial class Map
{
    
}



