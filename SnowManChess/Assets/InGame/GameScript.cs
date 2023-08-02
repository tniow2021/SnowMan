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
    private void Awake()
    {
        UserInput.instance = GetComponent<UserInput>();

    }

    GameLogic Logic;

    public User user1=new User("Damyeong", 30, 19);//임시로
    User user2 = new User("Dongmin", 30, 18);

    public Map map;
    void Start()
    {
        
        instance = this.gameObject.GetComponent<GameScript>();
        MapSet mapSet1 = new MapSet()
        {
            size = new Vector2Int(9, 9),
            TileSet = new TK[,]
            {
                {TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow},
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
                {PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none },
                {PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none },
                {PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none },
                {PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none },
                {PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none },
                {PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none },
                {PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none },
                {PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none },
                {PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none }
            },
            BulidngSet = new BK[,]
            {
                {BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none },
                {BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none },
                {BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none },
                {BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none },
                {BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none },
                {BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none },
                {BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none },
                {BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none },
                {BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none,BK.none },
            },
            ItemSet=new IK[,]
            {
                {IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none },
                {IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none },
                {IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none },
                {IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none },
                {IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none },
                {IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none },
                {IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none },
                {IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none },
                {IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none,IK.none }
            }
            

        };
        map.MapCreate(mapSet1);
        Logic = new GameLogic(map.mapArea);

    }
}
public class MoveOrder
{
    public Vector2Int Piece;
    public Vector2Int ToTile;
}

public enum InputMode
{
    Pick,
    Put,//이동할장소를  지정
    Route,
    Disposition//임시. 멘토링용. 기물선택자.
}

public partial class GameScript
{

    //입력관련

 
    public CameraScript cameraScript;
    public List<Vector2Int> RouteList = new List<Vector2Int>();

    public InputMode inputMode = InputMode.Pick;
    void BeFixedCamera(InputMode mode)
    {
        if (Input.touchCount == 2)
        {
            cameraScript.CameraMoveAble = true;
        }
        else if (Input.touchCount > 2)
        {
            cameraScript.CameraMoveAble = true;
        }


        if (mode == InputMode.Route)
        {
            //드래그를 하는 동안에는 카메라를 멈춘다.
            cameraScript.CameraMoveAble = false;
        }
        else if (mode == InputMode.Pick)
        {
            //드래그도 터치도하지않는 대기상태라면 카메라가 움직일 수 있게 풀어준다.
            cameraScript.CameraMoveAble = true;

        }

    }
    Command.PieceMove pieceMove = new Command.PieceMove();
    List<Vector2Int> Candidate;//map.PieceCanGoTileCandidate(EnterXy);에게 반환받는다. 기물이 이동할 수 있는 타일위치리스트
    
    
 

    private void Update()
    {
         
        int state=UserInput.instance.StateCherk();
        (Vector2Int EnterXY, bool BeMouseOnArea, Area area) a = map.mapArea.TouchCherk();
        Vector2Int EnterXY = a.EnterXY;
        bool BeMouseOnArea = a.BeMouseOnArea;
        print(EnterXY);
        print(BeMouseOnArea);
        Area area = a.area;

        if (inputMode ==InputMode.Pick)
        {
            if(state==InputStateKind.Touch)
            {
                if(BeMouseOnArea is true)
                {

                    map.FindArea(EnterXY).tile.Tiletest();
                    if (map.FindArea(EnterXY).piece is not null)//터치한 타일에 기물이 있어야 
                    {
                        map.FindArea(EnterXY).tile.TileTouch();
                        pieceMove.piece = XY.ToXY(EnterXY);
                        inputMode = InputMode.Put;
                        Candidate=map.PieceCanGoTileCandidate(EnterXY);
                    }
                }
            }
            else if(state==InputStateKind.LongTouch)
            {
                if (map.FindArea(EnterXY).piece is not null)
                {
                    inputMode = InputMode.Route;
                }
            }
        }
        else if (inputMode == InputMode.Put)
        {
            if (state == InputStateKind.Touch)
            {
                if (BeMouseOnArea)
                {
                    if (! XY.Equals(pieceMove.piece,XY.ToXY(EnterXY)))//기물을 이동시킬 타일지정함
                    {
                        pieceMove.ToTile = XY.ToXY(EnterXY);
                        Logic.PieceMove(pieceMove);

                        map.BeAllTileWhite();
                        map.FindArea(EnterXY).tile.GetComponent<SpriteRenderer>().color = Color.blue;

                        inputMode = InputMode.Pick;
                    }
                    else//같은 타일을 두번 터치하면
                    {
                        inputMode = InputMode.Pick;//다시 처음으로
                    }
                    
                }
            }
            else//맵외곽을 터치
            {

            }
               
        }
        else if (inputMode == InputMode.Route)
        {
            if(state==InputStateKind.Ended)
            {
                inputMode = InputMode.Pick;
            }
            map.FindArea(EnterXY).tile.TileDrag();

        }
        else if(inputMode==InputMode.Disposition)//임시 
        {

            if (state == InputStateKind.Touch)
            {
                map.BeAllTileWhite();
                if (testScript.젠장==하.건물)
                {
                    map.Create(bkk, area);
                    inputMode = InputMode.Pick;
                }
                if (testScript.젠장 == 하.기물)
                {
                    map.Create(Pkk, area);
                    inputMode = InputMode.Pick;
                }
                if (testScript.젠장 == 하.타일)
                {
                    map.Create(Tkk, area);
                    inputMode = InputMode.Pick;
                }
            }
        }
        BeFixedCamera(inputMode);


    }
}

public partial class GameScript//커스텀 기물-건물-타일 배치
{
    PK Pkk;
    TK Tkk;
    BK bkk;
    public void KindReceived(PK pk)
    {
        inputMode = InputMode.Disposition;
        Pkk = pk;
    }
    public void KindReceived(TK tk)
    {
        Tkk = tk;
        inputMode = InputMode.Disposition;
    }
    public void KindReceived(BK bk)
    {
        bkk = bk;
        inputMode = InputMode.Disposition;
    }
}


public partial class GameScript //서버로 올라가는 길
{

}
public partial class GameScript //Map으로 내려가는 길
{
 
}
public partial class GameScript //사전 설정
{



}




