using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * �����:
 * Map map1 �ν��Ͻ��� ���� �����⸦ �����.
 * 
 * MapCreate�� MapSet������ �Ķ���ͷ� �ѱ����μ� 
 * �ʿ� Ÿ�ϰ� �⹰�� �������� ���õǰ�
 * List<List<MapArea>>�� ��ȯ�޴´�.
 * 
 * map1 �ν��Ͻ��� ���ؼ� �ʿ� ����� ���� �� �ְ�
 * ��ȯ���� MapArea �ν��Ͻ��� ���ؼ� ���� �����Ȳ�� �� �� �ִ�.
 * 
 * MapŬ���� ����:
 * MapArea�� ���� �Է�(������)�� ������ ������ �� �� �ִ�.(�������� �޼ҵ带 ȣ���ϴ� �� ¥��)
 * ��Ȯ�� �����ڸ�
 * ���ӽ���
 * :
 * ������ �ʼµ����� �ѱ�->mapcreate->�ʱ� �⹰��ġ��->�ٽ� �������� �ʼµ����� �ѱ�.
 * ������ �� ������ �ʼµ����͸� ��� �޾Ƶ���ٸ� ���ӽ��۽�ȣ�� ����
 * ���� 
 * �����Է�->�ٲ�͸� ��������->�˻� �� ���ӷ���ó��->�����Է�(������ Ŭ���̾�Ʈ���� �Է�)
 * ->������ �� �������� ���� MapArea���� ���� ��ȭ���� 
 * MapŬ���� ������ �⹰�̵�(),Ÿ�Ϻ�ȭ(),�⹰����,�����۵��()���� �Լ��� ȣ����� �ʿ� �ݿ��Ѵ�
 * 
 * �� GameScript�� ���������� ���� �ٲ� �� ���� ������ ���ļ� ����.
 * GameScript�� ������Ʈ�� ����(1ƽ��.) MapArea�ݿ�()�Լ��� ��� ȣ���ؼ� ���� ��ȭ��Ų��.
 * 
 * �Ӽ�
 * :
 * mapArea
 * mapSet
 * ������ ���� ������ Ÿ��
 * ������ ���� ������ �⹰üũ
 * 
 * �޼ҵ�
 * �������Է�():
 * ������ ���� ������ Ÿ��üũ�Լ�(gameScript���� Ÿ�Ͻ�ũ��Ʈ�� out���� ����)
 * ������ ���� ������ �⹰üũ�Լ�(gameScript���� �⹰��ũ��Ʈ�� out���� ����)
 * ->gameScript���� ��� update�������� Ȯ��.
 * 
 * 
 * �������� ���():
 * MapArea�ݿ��Լ�
 * -> ������ ����ȭ���� ������Ʈ��
 * �������Է�():
 * �������� MapArea������ �ʵ����͸� ���޹ް� �װ� MapArea�� ����
 * �������� ���():
 * ������ ���� ����� �������� ����.��
 * ��������:
 * 
 */

public class MapSet
{
    public int X;
    public int Y;
    public TK[,] TileSet;
    public PK[,] PieceSet;
    public IK[,] ItemSet;
    public BK[,] BulidngSet;
    public GameObject MapObject;
}

public class MapArea //�������Ȳ
{
    public TileScript Tile;
    public PieceScript Piece;
    public ItemScript Item;
    public BuildingScript Buliding;
}

public partial class Map : MonoBehaviour
{
    //----------------------------------------------------------
    public static Map insatance;

    //�� ��Ʈ
    MapSet mapSet;
    //���� ��
    public List<List<MapArea>> mapArea = new List<List<MapArea>>();
    //---------------------------------------����Ʈ----------------------------------------
    //Ÿ�Ͽ�����Ʈ �Ѹ���Ʈ
    List<List<GameObject>> List2TileObject = new List<List<GameObject>>();
    public GameObject GetTileObj(Vector2Int XY)
    {
        return List2TileObject[XY.x][XY.y];
    }
    //Ÿ�Ͻ�ũ��Ʈ �Ѹ���Ʈ
    public List<List<TileScript>> List2TileScript = new List<List<TileScript>>();
    public TileScript GetTile(Vector2Int XY)
    {
        return List2TileScript[XY.x][XY.y];
    }
    //�⹰������Ʈ �Ѹ���Ʈ
    public List<GameObject> PieceList = new List<GameObject>();
    //�����ۿ�����Ʈ �Ѹ���Ʈ
    public List<GameObject> ItemList = new List<GameObject>();
    //�ǹ�������Ʈ �Ѹ���Ʈ
    public List<GameObject> BulidingList = new List<GameObject>();

    //----------------------------------------����---------------------------------------------
    //Ÿ��
    public static Dictionary<TK, GameObject> TileDictionnary = new Dictionary<TK, GameObject>()
    {
        {TK.none,GameScript.EmptyTile },//Ÿ�ϼ¿����� None�̸� ���ڸ��� ǥ���ϴ� ���ӿ�����Ʈ�� �־��ְ�
        {TK.test, GameScript.Testtile},
        {TK.Snow,GameScript.SnowTile },
        {TK.Lake,GameScript.LakeTile },
    };
    //�⹰
    public static Dictionary<PK, GameObject> PieceDictionary = new Dictionary<PK, GameObject>()
    {
        {PK.none, null}, //�⹰�¿����� None�̸� NULL������ ó���Ѵ�. NUll���̸� �⹰�� ��ġ���ϴ� �ɷ��Ѵ�.
        {PK.King, GameScript.KingPiece},
        {PK.Queen,GameScript.QueenPiece },
        {PK.Bishop,GameScript.BishopPiece },
        {PK.Knight,GameScript.KnightPiece },
        {PK.Rook,GameScript.RookPiece },
        {PK.Pawn,GameScript.PawnPiece }
    };
    //������
    public static Dictionary<IK, GameObject> ItemDeictionary = new Dictionary<IK, GameObject>()
    {
    };
    //�ǹ�
    public static Dictionary<BK, GameObject> BulidingDeictionary = new Dictionary<BK, GameObject>()
    {
    };
    //----------------------------------------�޼ҵ�-------------------------------------

    public void MapCreate(MapSet _mapSet)
    {
        mapSet = _mapSet;
        for (int i=0;i<mapSet.X;i++)
        {
            mapArea.Add(new List<MapArea>());
            for (int j = 0; j< mapSet.X; j++)
            {
                mapArea[i].Add(new MapArea());
            }
        }

        //�޸� �����
        for (int x = 0; x < mapSet.X; x++)
        {
            List<TileScript> newList1TileScript = new List<TileScript>();
            List<GameObject> newList1TileObject = new List<GameObject>();
            for (int y = 0; y < mapSet.Y; y++)// ������Ʈ ���� �Ҵ�
            {
                //Ÿ�� ����ֱ�
                //�ʼ��� Ÿ�ϼ¿��� TK���� �ҷ��� �װɷ� Ÿ�ϵ�ųʸ����� �˻�
                GameObject Originaltile = TileDictionnary[mapSet.TileSet[x, y]];
                if (Originaltile is not null)
                {

                    //TileSet[x,y]���� none�̴��� EmptyTile�̶�� ģ���� �����ȴ�.
                    GameObject newTileObject = Instantiate(Originaltile);//�߿�
                    TileScript newtileScript = newTileObject.GetComponent<TileScript>();
                    //�ʾƷ��ƿ� �ִ´�.
                    
                    mapArea[x][y].Tile= newtileScript;
                    //MapObject�� �ڽ����� �����.
                    newTileObject.transform.parent = mapSet.MapObject.transform;
                    //MapObject�Ʒ� ��ǥ������ ��ġ�Ѵ�
                    newTileObject.transform.localPosition = new Vector3(x, y, 0);

                    newtileScript.coordinate = new Vector2Int(x, y);

                    newList1TileObject.Add(newTileObject);
                    newList1TileScript.Add(newtileScript);
                }

                //�⹰����ֱ�
                if (PieceDictionary.TryGetValue(mapSet.PieceSet[x, y], out GameObject OriginalPiece))
                {
                    if (OriginalPiece is not null)
                    {
                        GameObject newPiece = Instantiate(OriginalPiece);
                        //�ʾƷ��ƿ� �ִ´�.
                        mapArea[x][y].Piece = newPiece.GetComponent<PieceScript>();
                        //MapObject�� �ڽ����� �����.
                        newPiece.transform.parent = mapSet.MapObject.transform;
                        //MapObject�Ʒ� ��ǥ������ ��ġ�Ѵ�(MainScript�� ������ ������)
                        newPiece.transform.localPosition = new Vector3(x, y, 0) + MainScript.LocalPositionOfPieceOntile;
                    }
                }

                //������ ����ֱ�
                //�ǹ�����ֱ�
            }
            List2TileObject.Add(newList1TileObject);
            List2TileScript.Add(newList1TileScript);
        }
        ClassConnect();
    }
    void ClassConnect()
    {
         //Map�� �⹰,Ÿ��, ������, �ǹ�����Ŭ������ ���� �����ϱ�(��ȣ�����ϱ�)
        foreach(List<TileScript> obj in List2TileScript)
        {
            foreach(TileScript objj in obj)
            {
                objj.map1 = this;
            }
        }
        foreach(GameObject obj in PieceList)
        {
            obj.GetComponent<PieceScript>().map1 = this;
        }
        foreach (GameObject obj in ItemList)
        {
            obj.GetComponent<ItemScript>().map1 = this;
        }
        foreach (GameObject obj in BulidingList)
        {
            obj.GetComponent<BuildingScript>().map1 = this;
        }
    }
    public MapArea GetMapArea(Vector2Int XY)
    {
        return mapArea[XY.x][XY.y];
    }
}
public partial class Map//������ �ö󰡴� ��
{

}
public partial class Map//�������� ��
{



}
public partial class Map
{   
    public void PieceMove(MoveOrder moveOrder)//������ �ڸ��� �⹰�� ������ �ı��� �̵�.
    {
        Vector2Int Piece = moveOrder.Piece;
        Vector2Int ToTile = moveOrder.ToTile;
        if (GetMapArea(ToTile).Piece is null)//������ �ڸ��� �⹰�� ������
        {
            //������
            GetMapArea(Piece).Piece.transform.localPosition
            = new Vector3(moveOrder.ToTile.x, moveOrder.ToTile.y, 0);
            //��ǥ����
            GetMapArea(Piece).Piece.Coordinate = moveOrder.ToTile;
            //��ũ��Ʈ ��ü
            GetMapArea(ToTile).Piece = GetMapArea(Piece).Piece;
            GetMapArea(Piece).Piece = null;
        }
        else if(GetMapArea(ToTile).Piece is not null)//������ �ڸ��� �⹰�� ������
        {
            //������
            GetMapArea(Piece).Piece.transform.localPosition
            = new Vector3(moveOrder.ToTile.x, moveOrder.ToTile.y, 0);
            //��ǥ����
            GetMapArea(Piece).Piece.Coordinate = moveOrder.ToTile;
            //�����⹰ �ı�
            GetMapArea(ToTile).Piece.ObjectDestory();
            //��ũ��Ʈ��ü
            GetMapArea(ToTile).Piece = GetMapArea(Piece).Piece;
            GetMapArea(Piece).Piece = null;
        }
        
    }
    //�⹰�̵�(��ǥ,����)

    //�³�ȭ����();

}



