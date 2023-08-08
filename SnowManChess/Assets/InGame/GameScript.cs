using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
public class UserManager
{
    public User admin = new User(UserKind.admin, 0);
    List<User> users = new List<User>();

    public UserManager()
    {
        users.Add(admin);
    }
    public void Add(User user)
    {
        //count값 덕분에 들어온 순서대로 아이디가 매겨진다.
        users.Add(user);
    }

    public int Count()
    {
        return users.Count;
    }
}
public class TurnManager
{
    static TurnManager instance;
    GameLogic logic;
    public TurnManager(GameLogic _logic)
    {
        logic = _logic;
        instance = this;
    }
    public static TurnManager GetInstance()
    {
        return instance;
    }
    List<TurnConnect> orderList = new List<TurnConnect>();//Order 는 서수란 뜻
    int index = 0;
    int TurnCounting = 0;
    public int GetTurnCount()
    {
        return TurnCounting;
    }
    void TurnChange()
    {
        logic.TurnIsEnd();
        TurnCounting++;
        orderList[index].NowNotMyTurn();
        index++;
        if(index >= orderList.Count)
        {
            index = 0;
        }
        orderList[index].NowMyTurn();
    }
    public User GetTurn()//널오류가 뜨지않게 조심해서 쓰기
    {
        MonoBehaviour.print(index);
        return orderList[index].user;
    }
    public void TurnButtenEvent(User user)
    {   //턴은 원래 턴매니저에 의해 자동으로
        //바뀌는데 수를 두지않고 턴을 바꿀려면
        //지금 두고 있는 사람이 턴버튼을 눌러야 턴이 돌아간다.
        if (GetTurn()==user)
        {
            TurnChange();
        }
    }
    int maximumMovement = 1;
    int MovementNumber=0;
    //복잡해질 함수
    public void MovementReport(MovementKind mk)
    {
        MonoBehaviour. print(mk);
        if (mk == MovementKind.PieceCreate) MovementNumber++;
        if (mk == MovementKind.MovePiece) MovementNumber++;
        if (mk == MovementKind.BuildingCreate) MovementNumber++;

        if(MovementNumber>=maximumMovement)
        {
            TurnChange();
            MovementNumber = 0;
        }
    }
    public void OrdinalAdd(TurnConnect TurnConn,User user)
    {
        TurnConn.user = user;
        TurnConn.turnManager = this;
        if (orderList.Count == 0) TurnConn.NowMyTurn();
        orderList.Add(TurnConn);
    }
    
}
public enum MovementKind
{
    MovePiece,
    PieceCreate,
    BuildingCreate
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
    public TurnManager turnManager; 

    public TurnConnect Turn1;
    public TurnConnect Turn2;
    void Start()
    {
        instance = this.gameObject.GetComponent<GameScript>();


        User user1 = new User(UserKind.general, 1,"damyeong");
        User user2 = new User(UserKind.general, 2,"dongmin");
        userManager.Add(user1);
        userManager.Add(user2);



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

        };
        map.MapCreate(mapSet1);
        Logic = new GameLogic(map.mapArea);
        turnManager = new TurnManager(Logic);
               //일단 순서는 user1,user2로.
        turnManager.OrdinalAdd(Turn1,user1);
        turnManager.OrdinalAdd(Turn2,user2); 
        UIsetting(topUser, bottomUser);
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
    public GameObject WinLossWindow;
    public TMP_Text TopUserNameText;
    public TMP_Text bottomUserNameText;
    void UIsetting(User topUser,User bottomUser)
    {
        TopUserNameText.text = topUser.name;
        bottomUserNameText.text = bottomUser.name;
    }
    public void DieKingEvent(User user)
    {
        WinLossWindow.SetActive(true);
        WinLossWindow.transform.GetChild(0).GetComponent<TMP_Text>().text
            = user.name + " is Loss";
    }

    public CameraScript cameraScript;
    public InputMode inputMode = InputMode.Pick;
    //void BeFixedCamera(InputMode mode)
    //{
    //    if (Input.touchCount == 2)
    //    {
    //        cameraScript.CameraMoveAble = true;
    //    }
    //    else if (Input.touchCount > 2)
    //    {
    //        cameraScript.CameraMoveAble = true;
    //    }


    //    if (mode == InputMode.Route)
    //    {
    //        //드래그를 하는 동안에는 카메라를 멈춘다.
    //        cameraScript.CameraMoveAble = false;
    //    }
    //    else if (mode == InputMode.Pick)
    //    {
    //        //드래그도 터치도하지않는 대기상태라면 카메라가 움직일 수 있게 풀어준다.
    //        cameraScript.CameraMoveAble = true;

    //    }

    //}


    private void Update()
    {

        User user = turnManager.GetTurn();
        InputStateKind state=UserInput.instance.StateCherk();
        GameHandling(state,user);
        //BeFixedCamera(inputMode);
        
    }

    Order.PieceMove pieceMove = new Order.PieceMove();
    Order.PieceCreate pieceCreate = new Order.PieceCreate();
    Order.BuildingCreate buildingCreate = new Order.BuildingCreate();
    void GameHandling(InputStateKind state,User user)
    {
        if (inputMode == InputMode.Pick)
        {
            if (state == InputStateKind.Touch)
            {
                if (map.mapArea.GetAreaTouched(out PieceScript piece))//터치한 타일에 기물이 있어야 
                {
                    pieceMove.piece = piece;
                    Logic.DisplaypieceMove(piece);
                    inputMode = InputMode.Put;
                }
            }
        }
        else if (inputMode == InputMode.Put)
        {
            if (state == InputStateKind.Touch)
            {

                if (map.mapArea.GetAreaTouched(out Area area))
                {
                    pieceMove.toArea = area;
                    pieceMove.user = user;
                    area.tile.GetComponent<SpriteRenderer>().color = Color.blue;
                    //기물움직이기. true면 성공, false면 실패
                    if (Logic.PieceMove(pieceMove))
                    {
                        print("기물움직이기 성공!");

                        turnManager.MovementReport(MovementKind.MovePiece);
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
        else if (inputMode == InputMode.Disposition)//임시 
        {
            if(state==InputStateKind.StandBy)
            {
                if (복잡해 == 하.기물)
                {
                    Logic.DisplayPieceCreate(user);
                }
                if(복잡해==하.건물)
                {
                    Logic.DisplayBuildingCreate(bkk,user);
                }
            }                
            if (state == InputStateKind.Touch)
            {
                if (map.mapArea.GetAreaTouched(out Area area))
                {
                    map.BeAllTileWhite();
                    if (복잡해 == 하.건물)
                    {
                        buildingCreate.bk = bkk;
                        buildingCreate.user = user;
                        buildingCreate.area = area;
                        print(123);
                        if (Logic.BuildingCreate(buildingCreate))
                        {
                            print(456);
                            turnManager.MovementReport(MovementKind.BuildingCreate);
                        }
                        inputMode = InputMode.Pick;
                    }
                    if (복잡해 == 하.기물)
                    {
                        pieceCreate.pk = Pkk;
                        pieceCreate.user = user;
                        pieceCreate.area = area;
                        if (Logic.PieceCreate(pieceCreate))
                        {
                            turnManager.MovementReport(MovementKind.PieceCreate);
                        }
                        inputMode = InputMode.Pick;
                        
                    }
                    if (복잡해 == 하.타일)
                    {
                        //map.Create(Tkk, area);
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
    하 복잡해;
    public void KindReceived(PK pk,하 이런)
    {
        복잡해 = 이런;
        inputMode = InputMode.Disposition;
        Pkk = pk;
    }
    public void KindReceived(TK tk, 하 이런)
    {
        복잡해 = 이런;
        Tkk = tk;
        inputMode = InputMode.Disposition;
    }
    public void KindReceived(BK bk, 하 이런)
    {
        복잡해 = 이런;
        bkk = bk;
        inputMode = InputMode.Disposition;
    }
}


