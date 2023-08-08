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
        //count�� ���п� ���� ������� ���̵� �Ű�����.
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
    List<TurnConnect> orderList = new List<TurnConnect>();//Order �� ������ ��
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
    public User GetTurn()//�ο����� �����ʰ� �����ؼ� ����
    {
        MonoBehaviour.print(index);
        return orderList[index].user;
    }
    public void TurnButtenEvent(User user)
    {   //���� ���� �ϸŴ����� ���� �ڵ�����
        //�ٲ�µ� ���� �����ʰ� ���� �ٲܷ���
        //���� �ΰ� �ִ� ����� �Ϲ�ư�� ������ ���� ���ư���.
        if (GetTurn()==user)
        {
            TurnChange();
        }
    }
    int maximumMovement = 1;
    int MovementNumber=0;
    //�������� �Լ�
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
               //�ϴ� ������ user1,user2��.
        turnManager.OrdinalAdd(Turn1,user1);
        turnManager.OrdinalAdd(Turn2,user2); 
        UIsetting(topUser, bottomUser);
    }
}
public enum InputMode
{
    Pick,
    Put,//�̵�����Ҹ�  ����
    Route,
    Disposition//�⹰������.
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
    //        //�巡�׸� �ϴ� ���ȿ��� ī�޶� �����.
    //        cameraScript.CameraMoveAble = false;
    //    }
    //    else if (mode == InputMode.Pick)
    //    {
    //        //�巡�׵� ��ġ�������ʴ� �����¶�� ī�޶� ������ �� �ְ� Ǯ���ش�.
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
                if (map.mapArea.GetAreaTouched(out PieceScript piece))//��ġ�� Ÿ�Ͽ� �⹰�� �־�� 
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
                    //�⹰�����̱�. true�� ����, false�� ����
                    if (Logic.PieceMove(pieceMove))
                    {
                        print("�⹰�����̱� ����!");

                        turnManager.MovementReport(MovementKind.MovePiece);
                    }
                    else
                    {
                        print("�⹰�����̱� ����!");
                    }
                    map.BeAllTileWhite();
                    inputMode = InputMode.Pick;
                }
                else//area�� ������(�Ƹ� �̷����� ��������.)
                {
                    inputMode = InputMode.Pick;//�ٽ� ó������
                }
            }
            else//�ʿܰ��� ��ġ
            {

            }

        }
        else if (inputMode == InputMode.Disposition)//�ӽ� 
        {
            if(state==InputStateKind.StandBy)
            {
                if (������ == ��.�⹰)
                {
                    Logic.DisplayPieceCreate(user);
                }
                if(������==��.�ǹ�)
                {
                    Logic.DisplayBuildingCreate(bkk,user);
                }
            }                
            if (state == InputStateKind.Touch)
            {
                if (map.mapArea.GetAreaTouched(out Area area))
                {
                    map.BeAllTileWhite();
                    if (������ == ��.�ǹ�)
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
                    if (������ == ��.�⹰)
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
                    if (������ == ��.Ÿ��)
                    {
                        //map.Create(Tkk, area);
                        inputMode = InputMode.Pick;
                    }
                }
            }
        }
    }
}

public partial class GameScript//Ŀ���� �⹰-�ǹ�-Ÿ�� ��ġ
{
    PK Pkk;
    TK Tkk;
    BK bkk;
    �� ������;
    public void KindReceived(PK pk,�� �̷�)
    {
        ������ = �̷�;
        inputMode = InputMode.Disposition;
        Pkk = pk;
    }
    public void KindReceived(TK tk, �� �̷�)
    {
        ������ = �̷�;
        Tkk = tk;
        inputMode = InputMode.Disposition;
    }
    public void KindReceived(BK bk, �� �̷�)
    {
        ������ = �̷�;
        bkk = bk;
        inputMode = InputMode.Disposition;
    }
}


