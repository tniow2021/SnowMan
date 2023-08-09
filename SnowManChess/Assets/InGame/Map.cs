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
    //public IK[,] ItemSet;
    public (PK pk, Vector2Int xy, User user)[] PieceSet;
    public (BK bk, Vector2Int xy, User user)[] BuildingSet;
}

public partial class Map : MonoBehaviour
{


    //�� ��Ʈ
    MapSet mapSet;
    public MapAreas mapArea;//main MapAreas

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


                mapArea.Create(mapSet.TileSet[x, y], area);
                //mapArea.Create(mapSet.ItemSet[x, y], area);
                //�⹰�� �ǹ����� ����Ŭ�������� �־��ش�.
 

                mapArea.Insert(new Vector2Int(x, y), area);
            }
        }
        if(mapSet.PieceSet is not null)for(int i=0;i<mapSet.PieceSet.Length;i++)
        {
            (PK pk, Vector2Int xy, User user) a = mapSet.PieceSet[i];
            mapArea.Create(a.pk, a.user, mapArea.Find(a.xy));
        }
        if (mapSet.BuildingSet is not null) for (int i = 0; i < mapSet.BuildingSet.Length; i++)
        {
            (BK bk, Vector2Int xy, User user) a = mapSet.BuildingSet[i];
            mapArea.Create(a.bk, a.user, mapArea.Find(a.xy));
        }
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


}



