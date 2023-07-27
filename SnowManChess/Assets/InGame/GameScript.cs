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
    Map map1;
    void Start()
    {
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
        map1 = new Map(mapSet1);
        map1.gameScript1 = this;
        
    }
    void Update()
    {
        map1.userInput.AllInput();
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

    public  GameObject _Testtile;
    public  GameObject _EmptyTile;
    public  GameObject _SnowTile;
    public  GameObject _LakeTile;

    public  GameObject _KingPiece;
    public  GameObject _QueenPiece;
    public  GameObject _BishopPiece;
    public  GameObject _KnightPiece;
    public  GameObject _RookPiece;
    public  GameObject _PawnPiece;



    private void Awake()
    {
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

    }
}

//public partial class GameScript //�⹰, 
//{
//    List<GameObject> ListAllPiece = new List<GameObject>();


//    //�⹰ ������ �����ϸ� true��ȯ �ƴϸ� false��ȯ
//    public void PieceCreate(Vector2Int XY, GameObject pieceObject)//���߿� public bool�� ��������.
//    {
//        /*
//         * ��ǥ�� piece�� �޴´�.
//         */
//        GameObject NewPiece = Instantiate(pieceObject);
//        PieceScript NewPieceScript=NewPiece.GetComponent<PieceScript>();
//        //���ӻ󿡼� ������ �� �ִ� ��Ȳ�� ���� ������ ����ó���� ��ģ�Ŀ�

//        if (List2TileScript[XY.x][XY.y].PutPiece(NewPiece))//�⹰�α⿡ ������ ���
//        {
//            ListAllPiece.Add(NewPiece);
//            print("�⹰ ��ġ�Ϸ�");
//        }
//        else  //�⹰�α⿡ ������ ���(�̹� Ÿ�Ͽ� �⹰�� ����)
//        {
//            print("�̹� Ÿ�Ͽ� �ٸ� �⹰�� �����մϴ�.");
//        }

//    }

//    public bool PieceMovement()//�⹰�̵�
//    {
//        //7�� 26��....������ �������
//        return true;
//    }

//    public void AllPieceRemove()
//    {
//        foreach(GameObject obj in ListAllPiece)
//        {
//            if(obj is not null)
//            {
//                obj.GetComponent<PieceScript>().ObjectDestory();
//            }
//        }
//        ListAllPiece.Clear();
//    }

//}

