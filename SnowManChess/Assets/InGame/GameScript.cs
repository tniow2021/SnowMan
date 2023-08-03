using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class UserManager
{
    public User admin = new User(UserKind.admin, 0);
    List<User> users = new List<User>();

    List<User> orderList = new List<User>();//Order �� ������ ��
    int CurrentOrdinal = 0;
    public UserManager()
    {
        users.Add(admin);
    }
    public void Add(User user)
    {
        //count�� ���п� ���� ������� ���̵� �Ű�����.
        users.Add(user);
    }
    public void OrdinalAdd(User user)
    {
        orderList.Add(user);
    }
    public bool GetThisTurn(out User user)//�ο����� �����ʰ� �����ؼ� ����
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
        //    CurrentOrdinal = 0;//�ٽ�ó������.
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

        //�ϴ� ������ user1,user2��.
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
    Put,//�̵�����Ҹ�  ����
    Route,
    Disposition//�⹰������.
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
            //�巡�׸� �ϴ� ���ȿ��� ī�޶� �����.
            cameraScript.CameraMoveAble = false;
        }
        else if (mode == InputMode.Pick)
        {
            //�巡�׵� ��ġ�������ʴ� �����¶�� ī�޶� ������ �� �ְ� Ǯ���ش�.
            cameraScript.CameraMoveAble = true;

        }

    }
    Order.PieceMove pieceMove = new Order.PieceMove();
    private void Update()
    {
        //���� Ƚ������ϴ°� �������Ѵ�.
        if(userManager.GetThisTurn(out User user))
        {
            print(user.ID);
            InputStateKind state=UserInput.instance.StateCherk();
            GameHandling(state,user);
            BeFixedCamera(inputMode);
        }
        else
        {
            print("����!");
        }
    }
    void GameHandling(InputStateKind state,User user)
    {
        if (inputMode == InputMode.Pick)
        {
            if (state == InputStateKind.Touch)
            {
                if (map.mapArea.GetAreaTouched(out PieceScript piece))//��ġ�� Ÿ�Ͽ� �⹰�� �־�� 
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
                    //�⹰�����̱�. true�� ����, false�� ����
                    if (Logic.PieceMove(pieceMove))
                    {
                        print("�⹰�����̱� ����!");
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
        else if (inputMode == InputMode.Disposition)//�ӽ� 
        {
            if (state == InputStateKind.Touch)
            {
                if (map.mapArea.GetAreaTouched(out Area area))
                {
                    map.BeAllTileWhite();
                    if (testScript.���� == ��.�ǹ�)
                    {
                        map.Create(bkk, user, area);
                        inputMode = InputMode.Pick;
                    }
                    if (testScript.���� == ��.�⹰)
                    {
                        map.Create(Pkk, user, area);
                        inputMode = InputMode.Pick;
                    }
                    if (testScript.���� == ��.Ÿ��)
                    {
                        map.Create(Tkk, area);
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


