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



    public Map map;

    UserManager userManager = new UserManager();
    User user1 = new User(UserKind.general, 1, "damyeong");
    User user2 = new User(UserKind.general, 2, "dongmin");

    void Start()
    {
        instance = this.gameObject.GetComponent<GameScript>();

        userManager.Add(user1);
        userManager.Add(user2);
        MapSet mapSet1 = new MapSet()
        {
            size = new Vector2Int(9, 9),
            PieceSet = new (PK, Vector2Int, User)[]
            {
                (PK.Aking, new Vector2Int(4, 0), user1),
                (PK.Bking, new Vector2Int(4, 8), user2)
            },
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
public enum InputMode
{
    Pick,
    Put,//이동할장소를  지정
    Route,
    Disposition//기물선택자.
}
public partial class GameScript
{

    public CameraScript cameraScript;
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
    Order.PieceMove pieceMove = new Order.PieceMove();
    private void Update()
    {
         
        InputStateKind state=UserInput.instance.StateCherk();


        if (inputMode == InputMode.Pick)
        {
            if (state == InputStateKind.Touch)
            {
                if (map.mapArea.GetAreaTouched(out PieceScript piece))//터치한 타일에 기물이 있어야 
                {
                    pieceMove.piece = piece;
                    Logic.DisplayAreaThatItCanDo(piece);
                    inputMode = InputMode.Put;
                } 
            }
            else if (state == InputStateKind.LongTouch)
            {
                inputMode = InputMode.Route;
            }
        }
        else if (inputMode == InputMode.Put)
        {
            if (state == InputStateKind.Touch)
            {
                
                if (map.mapArea.GetAreaTouched(out Area area))
                {
                    pieceMove.toArea = area;
                    area.tile.GetComponent<SpriteRenderer>().color = Color.blue;
                    //기물움직이기. true면 성공, false면 실패
                    if (Logic.PieceMove(pieceMove))
                    {
                        print("기물움직이기 성공!");
                    }
                    else
                    {
                        print("기물움직이기 실패!");
                    }
                    map.BeAllTileWhite();
                    inputMode = InputMode.Pick;
                }
                else//area가 없으면(아마 이런일은 없겠지만.)
                {
                    inputMode = InputMode.Pick;//다시 처음으로
                }
            }
            else//맵외곽을 터치
            {

            }

        }
        else if (inputMode == InputMode.Route)
        {
            if (state == InputStateKind.Touch)
            {
                if(map.mapArea.GetAreaTouched(out Area area))
                {
                    area.tile.TileDrag();
                }
            }
            else if(state == InputStateKind.Ended)
            {
                inputMode = InputMode.Pick;
            }
        }
        else if (inputMode == InputMode.Disposition)//임시 
        {
            if (state == InputStateKind.Touch)
            {
                if(map.mapArea.GetAreaTouched(out Area area))
                {
                    map.BeAllTileWhite();
                    if (testScript.젠장 == 하.건물)
                    {
                        map.Create(bkk, user1,area);
                        inputMode = InputMode.Pick;
                    }
                    if (testScript.젠장 == 하.기물)
                    {
                        map.Create(Pkk,user2, area);
                        inputMode = InputMode.Pick;
                    }
                    if (testScript.젠장 == 하.타일)
                    {
                        map.Create(Tkk, area);
                        inputMode = InputMode.Pick;
                    }
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




