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
    public Vector2Int size;
    public TK[,] TileSet;
    public PK[,] PieceSet;
    public IK[,] ItemSet;
    public BK[,] BulidngSet;
}
public class MapAreas
{
    //����
    List<List<Area>> areas = new List<List<Area>>();

    public Vector2Int size
    {
        get { return sizeXy; }
    }
    Vector2Int sizeXy = new Vector2Int();
    public MapAreas(Map map,Vector2Int mapSize)
    {
        areas = new List<List<Area>>();
        sizeXy.x = mapSize.x;
        sizeXy.y = mapSize.y;
        for(int i=0; i< mapSize.x;i++)
        {
            List<Area> row = new List<Area>();
            for (int j=0;j< mapSize.y;j++)
            {
                GameObject newareaObject = MonoBehaviour.Instantiate(ObjectDict.Instance.area.gameObject);
                newareaObject.transform.parent = map.transform;
                newareaObject.transform.localPosition = new Vector3(i, j, 0);
                Area newArea = newareaObject.GetComponent<Area>();
                newArea.mapAreas = this;
                newArea.xy = new Vector2Int(i, j);//��ǥ����
                row.Add(newArea);
            }
            areas.Add(row);
        }
    }

    //�ൿ
    //���ӽ�ũ��Ʈ�� ���³�
    Area areaEntered;
    public void AreaEnteredEvent(Area _area)
    {
        areaEntered = _area;
    }
    public bool GetAreaTouched(out PieceScript outpieceScript)
    {
        if(areaEntered is not null)
        {
            if(areaEntered.BeMouseOnArea is true)
            {
                if(areaEntered.Get(out PieceScript piece))
                {
                    outpieceScript = piece;
                    return true;
                }
            }
        }
        outpieceScript = null;
        return false;
    }
    public bool GetAreaTouched(out TileScript outTileScript)
    {
        if (areaEntered is not null)
        {
            if (areaEntered.BeMouseOnArea is true)
            {
                if (areaEntered.Get(out TileScript tile))
                {
                    outTileScript = tile;
                    return true;
                }
            }
        }
        outTileScript = null;
        return false;
    }
    public bool GetAreaTouched(out BuildingScript outBuildingScript)
    {
        if (areaEntered is not null)
        {
            if (areaEntered.BeMouseOnArea is true)
            {
                if (areaEntered.Get(out BuildingScript building))
                {
                    outBuildingScript = building;
                    return true;
                }
            }
        }
        outBuildingScript = null;
        return false;
    }
    public bool GetAreaTouched(out ItemScript outItemScript)
    {
        if (areaEntered is not null)
        {
            if (areaEntered.BeMouseOnArea is true)
            {
                if (areaEntered.Get(out ItemScript item))
                {
                    outItemScript = item;
                    return true;
                }
            }
        }
        outItemScript = null;
        return false;
    }
    public bool GetAreaTouched(out Area outArea)
    {
        if (areaEntered is not null)
        {
            if (areaEntered.BeMouseOnArea is true)
            {
                outArea = areaEntered;
                return true;
            }
        }
        outArea = null;
        return false;
    }


    //�޼ҵ�
    public bool Insert(Vector2Int Index,Area area)
    {
        if (Index.x > sizeXy.x - 1 || Index.x < 0) return false;
        else if (Index.y > sizeXy.y - 1 || Index.y < 0) return false;

        areas[Index.x][Index.y] = area;
        return true;
    }
    public Area Find(int x, int y)
    {
        return areas[x][y];
    }
    public Area Find(Vector2Int index)
    {
        return areas[index.x][index.y];
    }

    public void Turn(int turnNumber)
    {
        for(int i=0;i< sizeXy.x; i++)
        {
            for(int j=0;j< sizeXy.y;j++)
            {
                areas[i][j].tile.Turn(turnNumber);
                areas[i][j].piece.Turn(turnNumber);
                areas[i][j].building.Turn(turnNumber);
                areas[i][j].item.Turn(turnNumber);
            }
        }
    }

}

public partial class Map : MonoBehaviour
{


    //�� ��Ʈ
    MapSet mapSet;
    public MapAreas mapArea;

    //----------------------------------------�޼ҵ�-------------------------------------

    public void MapCreate(MapSet _mapSet)
    {
        
        mapSet = _mapSet;
        mapArea = new MapAreas(this, mapSet.size);

        //�޸� �����
        for (int x = 0; x < mapSet.size.x; x++)
        {
            for (int y = 0; y < mapSet.size.y; y++)// ������Ʈ ���� �Ҵ�
            {
                //Ÿ�� ����ֱ�
                //�ʼ��� Ÿ�ϼ¿��� TK���� �ҷ��� �װɷ� Ÿ�ϵ�ųʸ����� �˻�


                Area area = FindArea(x, y);


                Create(mapSet.TileSet[x, y], area);
                Create(mapSet.PieceSet[x, y], area);
                Create(mapSet.BulidngSet[x, y], area);
                Create(mapSet.ItemSet[x, y], area);

                mapArea.Insert(new Vector2Int(x, y), area);
            }
        }
    }
    public void Turn(int turnNumber)
    {
        mapArea.Turn(turnNumber);
    }
    public Area FindArea(Vector2Int index)
    {
        return mapArea.Find(index);
    }
    public Area FindArea(int x,int y)
    {
        return mapArea.Find(new Vector2Int(x,y));
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

    public void BeAllTileWhite()
    {
        for(int i=0;i<mapSet.size.x;i++)
        {
            for (int j = 0;j < mapSet.size.y; j++)
            {
                mapArea.Find(i, j).tile.BetileWilte();
            }
        }
    }

    public void Create(PK pk,Area area)
    {
        if(ObjectDict.Instance.FindObject(pk,out GameObject obj))
        {
            GameObject newPieceObject = Instantiate(obj);
            PieceScript newPieceScript = newPieceObject.GetComponent<PieceScript>();
            if(area.Put(newPieceScript,out PieceScript oldPiece))
            {
                oldPiece.ObjectDestory();
            }
        }
    }
    public void Create(TK tk, Area area)
    {
        if(ObjectDict.Instance.FindObject(tk,out GameObject obj))
        {
            GameObject newTileObject = Instantiate(obj);
            TileScript newTileScript = newTileObject.GetComponent<TileScript>();
            if(area.Put(newTileScript, out TileScript oldtile))
            {
                oldtile.ObjectDestory();
            }
        }
    }
    public void Create(BK bk, Area area)
    {
        if(ObjectDict.Instance.FindObject(bk, out GameObject obj))
        {
            GameObject newBuildingObject = Instantiate(obj);
            BuildingScript newBuildingScript = newBuildingObject.GetComponent<BuildingScript>();
            if(area.Put(newBuildingScript, out BuildingScript oldBuilding))
            {
                oldBuilding.ObjectDestory();
            }
        }
    }    
    public void Create(IK ik, Area area)
    {
        if(ObjectDict.Instance.FindObject(ik,out GameObject obj))
        {
            GameObject newItemObject = Instantiate(obj);
            ItemScript newItemScript = newItemObject.GetComponent<ItemScript>();
            if(area.Put(newItemScript, out ItemScript oldItem))
            {
                oldItem.ObjectDestory();
            }
        }
    }
}



