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

    public User user1=new User("Damyeong", 30, 19);//�ӽ÷�
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
        //Logic = new GameLogic(map.mapArea);

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
    Put,//�̵�����Ҹ�  ����
    Route,
    Disposition//�ӽ�. ���丵��. �⹰������.
}

public partial class GameScript
{

    //�Է°���

 
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
            //�巡�׸� �ϴ� ���ȿ��� ī�޶� �����.
            cameraScript.CameraMoveAble = false;
        }
        else if (mode == InputMode.Pick)
        {
            //�巡�׵� ��ġ�������ʴ� �����¶�� ī�޶� ������ �� �ְ� Ǯ���ش�.
            cameraScript.CameraMoveAble = true;

        }

    }
    List<Vector2Int> Candidate;//map.PieceCanGoTileCandidate(EnterXy);���� ��ȯ�޴´�. �⹰�� �̵��� �� �ִ� Ÿ����ġ����Ʈ
    
    
 
    void AreaPick(Area Area)
    {

    }
    void AreaPut(Area Area)
    {

    }
    //void AreaRoute(Area area)
    //{

    //}

    Order.PieceMove pieceMove = new Order.PieceMove();
    private void Update()
    {
         
        InputStateKind state=UserInput.instance.StateCherk();


        if (inputMode == InputMode.Pick)
        {
            if (state == InputStateKind.Touch)
            {
                if(map.mapArea.GetAreaTouched(out TileScript tile))//����
                {
                    tile.TileTouch();
                }
                if (map.mapArea.GetAreaTouched(out PieceScript piece))//��ġ�� Ÿ�Ͽ� �⹰�� �־�� 
                {
                    pieceMove.piece = piece;
                    inputMode = InputMode.Put;
                    //Candidate = map.PieceCanGoTileCandidate(EnterXY);
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
                if(map.mapArea.GetAreaTouched(out Area area))
                {
                    if (pieceMove.piece.area != area)//������ �ڸ��� ���� �ڸ����޶����
                    {
                        pieceMove.toArea = area;
                        //Logic.PieceMove(pieceMove);

                        map.BeAllTileWhite();
                        area.tile.GetComponent<SpriteRenderer>().color = Color.blue;

                        inputMode = InputMode.Pick;
                    }
                }
                else//���� Ÿ���� �ι� ��ġ�ϸ�
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
                        map.Create(bkk, area);
                        inputMode = InputMode.Pick;
                    }
                    if (testScript.���� == ��.�⹰)
                    {
                        map.Create(Pkk, area);
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




