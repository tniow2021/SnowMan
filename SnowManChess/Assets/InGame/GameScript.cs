using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public partial class GameScript : MonoBehaviour
{

    /*
     * ���ӽ��۽ÿ��� �ʿ��� ����
     * :ü���� ũ��, Ÿ�Ϻ� ������(����, �ڿ�)
     * �ǽð� �������(����� �͸� ���, �ֱ������� ��ü�� �˻�)
     * :
     * �⹰ �̵�
     * Ÿ������ ���泻��
     * 
     * 
     * �������� �� �ܰ��� ��ȹ
     * :
     * clinet script�� ���� �ϴ� ���ڿ� ���.
     * �⹰�̵� ����ȭ
     * ���۽���ŵ�����, �ǽð� ������ ����ü ¥��
     * client script�ۼ�
     * ������ �ϱ�ȯ�� ����ŷ ����
     * �����ۼ�
     * 
     * �巡�� �̵���μ���
     * ��(������Ʈ�������� �ּҴ��� ����. �� 50ms��)
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
         
        InputStateKind state=UserInput.instance.StateCherk();


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
        else if (inputMode == InputMode.Disposition)//�ӽ� 
        {
            if (state == InputStateKind.Touch)
            {
                if(map.mapArea.GetAreaTouched(out Area area))
                {
                    map.BeAllTileWhite();
                    if (testScript.���� == ��.�ǹ�)
                    {
                        map.Create(bkk, user1,area);
                        inputMode = InputMode.Pick;
                    }
                    if (testScript.���� == ��.�⹰)
                    {
                        map.Create(Pkk,user2, area);
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
        BeFixedCamera(inputMode);
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


public partial class GameScript //������ �ö󰡴� ��
{

}
public partial class GameScript //Map���� �������� ��
{
 
}
public partial class GameScript //���� ����
{



}




