using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class UserManager
{
    public User admin = new User(UserKind.admin, 0);
    List<User> users = new List<User>();

    List<User> orderList = new List<User>();//Order 는 서수란 뜻
    int CurrentOrdinal = 0;
    public UserManager()
    {
        users.Add(admin);
    }
    public void Add(User user)
    {
        //count값 덕분에 들어온 순서대로 아이디가 매겨진다.
        users.Add(user);
    }
    public void OrdinalAdd(User user)
    {
        orderList.Add(user);
    }
    public bool GetThisTurn(out User user)//널오류가 뜨지않게 조심해서 쓰기
    {
        if(orderList.Count>0)
        {
            user = orderList[CurrentOrdinal];
            return true;
        }
        else
        {
            user = null;
            return false;
        }
        
    }
    public void TurnChange(User user)
    {
        CurrentOrdinal = orderList.IndexOf(user);
        //CurrentOrdinal++;
        //if(CurrentOrdinal>=orderList.Count)
        //{
        //    CurrentOrdinal = 0;//다시처음으로.
        //}
    }
    public int Count()
    {
        return users.Count;
    }
}
public partial class GameScript : MonoBehaviour
{
    public static GameScript instance;
    private void Awake()
    {
        UserInput.instance = GetComponent<UserInput>();
    }

    GameLogic Logic;
    public Map map;
    public UserManager userManager = new UserManager();
    public TurnButten turnButten1;
    public TurnButten turnButten2;
    void Start()
    {
        instance = this.gameObject.GetComponent<GameScript>();

        User user1 = new User(UserKind.general, 1);
        User user2 = new User(UserKind.general, 2);
        userManager.Add(user1);
        userManager.Add(user2);

        //일단 순서는 user1,user2로.
        userManager.OrdinalAdd(user1);
        userManager.OrdinalAdd(user2);

        turnButten1.user = user1;
        turnButten2.user = user2;

        User topUser = user2;
        User bottomUser = user1;
        MapSet mapSet1 = new MapSet()
        {
            size = new Vector2Int(9, 9),
            PieceSet = new (PK, Vector2Int, User)[]
            {
                (PK.Bking, new Vector2Int(4, 8), topUser),
                (PK.Aking, new Vector2Int(4, 0), bottomUser)
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
        //오더 횟수계산하는거 만들어야한다.
        if(userManager.GetThisTurn(out User user))
        {
            print(user.ID);
            InputStateKind state=UserInput.instance.StateCherk();
            GameHandling(state,user);
            BeFixedCamera(inputMode);
        }
        else
        {
            print("젠장!");
        }
    }
    void GameHandling(InputStateKind state,User user)
    {
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
                if (map.mapArea.GetAreaTouched(out Area area))
                {
                    area.tile.TileDrag();
                }
            }
            else if (state == InputStateKind.Ended)
            {
                inputMode = InputMode.Pick;
            }
        }
        else if (inputMode == InputMode.Disposition)//임시 
        {
            if (state == InputStateKind.Touch)
            {
                if (map.mapArea.GetAreaTouched(out Area area))
                {
                    map.BeAllTileWhite();
                    if (testScript.젠장 == 하.건물)
                    {
                        map.Create(bkk, user, area);
                        inputMode = InputMode.Pick;
                    }
                    if (testScript.젠장 == 하.기물)
                    {
                        map.Create(Pkk, user, area);
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


