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
    Map map;
    GameLogic Logic;

    public User user1=new User("Damyeong", 30, 19);//�ӽ÷�
    User user2 = new User("Dongmin", 30, 18);
    void Start() 
    {
        instance = this.gameObject.GetComponent<GameScript>();
        GameObject MapObject1 = new GameObject();
        MapSet mapSet1 = new MapSet()
        {
            MapObject = MapObject1,
            X = 9,
            Y = 9,
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
            BulidngSet =new BK[,]
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
            }
            
        };
        Map.insatance = new Map();
        map = Map.insatance;
        Map.insatance.MapCreate(mapSet1);
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
    Put,//�̵�����Ҹ�  ����
    Route,
    Disposition//�ӽ�. ���丵��. �⹰������.
}
public partial class GameScript
{
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
    Command.PieceMove pieceMove = new Command.PieceMove();
    List<Vector2Int> Candidate;//map.PieceCanGoTileCandidate(EnterXy);���� ��ȯ�޴´�. �⹰�� �̵��� �� �ִ� Ÿ����ġ����Ʈ
    private void Update()
    {
        Vector2Int EnterXy; 
        int state=UserInput.instance.StateCherk(out EnterXy);



        if(inputMode ==InputMode.Pick)
        {
            if(state==InputStateKind.Touch)
            {
                if(Map.insatance.GetMapArea(EnterXy).Tile.BeMouseOnTile is true)
                {

                    map.GetMapArea(EnterXy).Tile.Tiletest();
                    if (map.GetMapArea(EnterXy).Piece is not null)//��ġ�� Ÿ�Ͽ� �⹰�� �־�� 
                    {
                        map.GetMapArea(EnterXy).Tile.TileTouch();
                        pieceMove.piece = XY.ToXY(EnterXy);
                        inputMode = InputMode.Put;
                        Candidate=map.PieceCanGoTileCandidate(EnterXy);
                    }
                }
            }
            else if(state==InputStateKind.LongTouch)
            {
                if (map.GetMapArea(EnterXy).Piece is not null)
                {
                    inputMode = InputMode.Route;
                }
            }
        }
        else if (inputMode == InputMode.Put)
        {
            if (state == InputStateKind.Touch)
            {
                if (map.GetMapArea(EnterXy).Tile.BeMouseOnTile is true)
                {
                    if (! XY.Equals(pieceMove.piece,XY.ToXY( EnterXy)))//�⹰�� �̵���ų Ÿ��������
                    {
                        pieceMove.ToTile = XY.ToXY(EnterXy);
                        Logic.PieceMove(pieceMove);

                        map.BeAllTileWhite();
                        map.GetMapArea(EnterXy).Tile.GetComponent<SpriteRenderer>().color = Color.blue;

                        inputMode = InputMode.Pick;
                    }
                    else//���� Ÿ���� �ι� ��ġ�ϸ�
                    {
                        inputMode = InputMode.Pick;//�ٽ� ó������
                    }
                    
                }
            }
            else//�ʿܰ��� ��ġ
            {

            }
               
        }
        else if (inputMode == InputMode.Route)
        {
            if(state==InputStateKind.Ended)
            {
                inputMode = InputMode.Pick;
            }
            map.GetMapArea(EnterXy).Tile.TileDrag();

        }
        else if(inputMode==InputMode.Disposition)//�ӽ� 
        {

            if (state == InputStateKind.Touch)
            {
                map.BeAllTileWhite();
                if (testScript.����==��.�ǹ�)
                {
                    map.BuildingCreate(bkk, EnterXy);
                    inputMode = InputMode.Pick;
                }
                if (testScript.���� == ��.�⹰)
                {
                    map.PieceCreate(Pkk, EnterXy);
                    inputMode = InputMode.Pick;
                }
                if (testScript.���� == ��.Ÿ��)
                {
                    map.TileCreate(Tkk, EnterXy);
                    inputMode = InputMode.Pick;
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
    //Ÿ������
    public static GameObject Testtile;
    public static GameObject EmptyTile;
    public static GameObject SnowTile;
    public static GameObject LakeTile;

    //�⹰����
    public static GameObject KingPiece;
    public static GameObject QueenPiece;
    public static GameObject BishopPiece;
    public static GameObject KnightPiece;
    public static GameObject RookPiece;
    public static GameObject PawnPiece;

    public GameObject _Testtile;
    public GameObject _EmptyTile;
    public GameObject _SnowTile;
    public GameObject _LakeTile;

    public GameObject _KingPiece;
    public GameObject _QueenPiece;
    public GameObject _BishopPiece;
    public GameObject _KnightPiece;
    public GameObject _RookPiece;
    public GameObject _PawnPiece;
    //ssssssssssssssssssss
    public GameObject _Aking;
    public GameObject _Aknight;
    public GameObject _Abishop;
    public GameObject _Apown;
    public GameObject _Arook;
    public GameObject _Bking;
    public GameObject _Bknight;
    public GameObject _Bbishop;
    public GameObject _Bpown;
    public GameObject _Brook;

    public  static GameObject Aking;
    public static GameObject Aknight;
    public static GameObject Abishop;
    public static GameObject Apown;
    public static GameObject Arook;
    public static GameObject Bking;
    public static GameObject Bknight;
    public static GameObject Bbishop;
    public static GameObject Bpown;
    public static GameObject Brook;



    //�ǹ�����
    public static GameObject SnowWallBuilding;
    public GameObject _SnowWallBuilding;

    //������ ����
    public static GameObject Ice;
    public GameObject _Ice;

    //�Է°���

    public float _TouchDecisionTime = 0.7f;
    public static float TouchDecisionTime;
    public CameraScript _CameraScript;
    public static CameraScript cameraScript;


    private void Awake()
    {
        UserInput.instance = GetComponent<UserInput>();

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
        //
        Aking = _Aking;
        Aknight = _Aknight;
        Abishop = _Abishop;
        Apown = _Apown;
        Arook = _Arook;

        Bking = _Bking;
        Bknight = _Bknight;
        Bbishop = _Bbishop;
        Bpown = _Bpown;
        Brook = _Brook;













        //

        SnowWallBuilding = _SnowWallBuilding;
        Ice = _Ice;

        cameraScript = _CameraScript;
        TouchDecisionTime = _TouchDecisionTime;


    }

}




