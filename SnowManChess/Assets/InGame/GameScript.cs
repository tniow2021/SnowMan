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
    Map map1;
    void Start() 
    {
        instance = this.gameObject.GetComponent<GameScript>();
        clientScript = ClientScript.GetClientScript();
        GameObject MapObject1 = new GameObject();
        MapSet mapSet1 = new MapSet()
        {
            MapObject = MapObject1,
            X = 9,
            Y = 9,
            TileSet = new TK[,]
            {
                {TK.Lake,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow},
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
                {PK.King,PK.none,PK.Knight,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none },
                {PK.none,PK.none,PK.none,PK.none,PK.Knight,PK.none,PK.none,PK.none,PK.none },
                {PK.King,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none },
                {PK.King,PK.none,PK.none,PK.none,PK.none,PK.Rook,PK.none,PK.none,PK.none },
                {PK.King,PK.none,PK.none,PK.none,PK.Rook,PK.none,PK.none,PK.none,PK.none },
                {PK.King,PK.none,PK.Queen,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none },
                {PK.King,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none },
                {PK.King,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none },
                {PK.King,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none,PK.none }
            }
        };
        Map.insatance = new Map();
        Map.insatance.MapCreate(mapSet1);
        
    }
}
public partial class GameScript //������ �ö󰡴� ��
{
    public ClientScript clientScript; 
    public void FromMap(Command.ThingOntile.Post post)
    {
        //
        if(clientScript is not null)clientScript.FromGame(post);
    }
}
public partial class GameScript //Map���� �������� ��
{
    public void FromClient(Command command)
    {

    }
    public void Action()
    {

    }
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

        cameraScript = _CameraScript;
        TouchDecisionTime = _TouchDecisionTime;
    }
}




