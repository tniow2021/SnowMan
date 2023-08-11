using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public partial class GameLogic
{
    MapAreas mapAreas;
    AreaListCherk areaListCherk;
    public GameLogic(MapAreas _mapAreas)
    {
        mapAreas = _mapAreas;
        areaListCherk = new AreaListCherk(_mapAreas);
    }
    //���ӽ�ũ��Ʈ���� ȣ��Ǵ� �Լ�
    public bool PieceMove(Order.PieceMove order)
    {
        if(order.piece.user!=order.user)
        {
            return false;
        }
        List<Area> canGoXyList = areaListCherk.PieceMove(order.piece);
        if (canGoXyList.Count == 0) MonoBehaviour.print("�ù�");
        if (canGoXyList.Exists(x => Equals(x, order.toArea)))
        {
            PieceAction(order);
            return true;
        }
        else
        {
            return false;
        }
    }
    public void DisplaypieceMove(PieceScript piece,User user)
    {
        if(piece.user!=user)
        {
            return;
        }
        List<Area> canGoXyList = areaListCherk.PieceMove(piece);
        foreach(Area area in canGoXyList)
        {
            area.tile.TileCandidate();
        }
    }

    public bool PieceCreate(Order.PieceCreate order)
    {
        //�ڿ��˻�
        if (mapAreas.GetKing(order.user).area.tile.kind==TK.Hot)
        {
            return false;
        }

        List<Area> canCreateXyList = areaListCherk.PieceCreate(order.user);
        if (canCreateXyList.Exists(x => Equals(x, order.area)))
        {
            mapAreas.Create(order.pk,order.user,order.area);
            Hot(mapAreas.GetKing(order.user).area);
            return true;
        }
        else
        {
            return false;
        }
    }
    public void DisplayPieceCreate(User user)
    {
         foreach(Area area in areaListCherk.PieceCreate(user))
        {
            area.tile.ColorBule();
        }
    }

    public bool BuildingCreate(Order.BuildingCreate order)
    {
        List<Area> canCreateXyList = areaListCherk.BuildingCreate(order.bk,order.user);
        if (canCreateXyList.Exists(x => Equals(x, order.area)))
        {
            mapAreas.Create(order.bk, order.user, order.area);
            return true;
        }
        else
        {
            return false;
        }
    }
    public void DisplayBuildingCreate(BK bk, User user)
    {
        foreach(Area area in areaListCherk.BuildingCreate(bk,user))
        {
            area.tile.GetComponent<SpriteRenderer>().color = new Color32(168, 236, 255, 255);
        }
    }
}
public partial class GameLogic//�ϰ���
{
    System.Random random = new System.Random();
    public void TurnIsEnd()
    {
        TurnHandling();
    }
    void Hot(Area area)
    {
        if(area.Get(out TileScript coolTile))
        {
            if(coolTile.kind==TK.Snow1|| (coolTile.kind == TK.Snow2))
            {        
                if(ObjectDict.Instance.FindObject(TK.Hot,out GameObject HotTile))
                {
                    GameObject newHotTile = MonoBehaviour.Instantiate(HotTile);
                    TileScript newHotTileScript=newHotTile.GetComponent<TileScript>();
                    if(area.Put(newHotTileScript, out TileScript outTile))
                    {
                        outTile.ObjectDestory();
                    }
                }
            }
        }

        
    }
    void TurnHandling()
    {
        foreach(PieceScript piece in mapAreas.allPieceList)
        {
            piece.Turn();
        }
    }
}
public partial class GameLogic//����ó�� ����
{
    void PieceAction(Order.PieceMove order)
    {
        //�̵��ϱ���
        if(order.toArea.Get(out BuildingScript building))
        {
            if (building.���ݴ���ΰ� is true)
            {
                int HP = building.DecreaseHP();
                if (HP <= 0) mapAreas.Delete(building);
                return;
            }
        }


        if (order.piece.area.Pick(out PieceScript piece))
        {
            if (order.toArea.Put(piece, out PieceScript outPiece))
            {
                MonoBehaviour.print(outPiece.gameObject.name);
                mapAreas.Delete(outPiece);
            }
            else//�̵��� ������ �´���� �ǹ�,Ÿ��,������
            {
                if(order.toArea.Get(out BuildingScript building2))
                {
                    if(building2.kind==BK.Boom)//��������Ʈ ó����
                    {
                        mapAreas.Delete(piece);
                        mapAreas.Delete(building2);
                    }
                }
                if (order.toArea.Get(out ItemScript item))
                {
                    if(item.kind==IK.Jump)
                    {

                    }
                }
                if (order.toArea.Get(out TileScript tile))
                {
                    if(tile.IsPieceDie is true)
                    {
                        mapAreas.Delete(piece);
                    }
                }
            }
        }
    }
}
public class AreaListCherk
{
    MapAreas mapAreas;
    public AreaListCherk(MapAreas _mapAreas)
    {
        mapAreas = _mapAreas;
    }

    public List<Area> PieceMove(PieceScript piece)
    {
        //piece.AbleRoute�� (0,0)�� �������� �߶� 2���� ����Ʈ�� �����Ѵ�.
        List<List<Vector2Int>> canRouteList = new List<List<Vector2Int>>();
        List<Vector2Int> tempRoute = new List<Vector2Int>();
        for (int i = 0; i < piece.AbleRoute.Count; i++)
        {
            if (Equals(piece.AbleRoute[i], new Vector2Int(0, 0)))
            {
                canRouteList.Add(tempRoute);
                tempRoute = new List<Vector2Int>();
            }
            else
            {
                tempRoute.Add(piece.AbleRoute[i]);
            }
        }
        //�� 2��������Ʈ���� 1��������Ʈ�� �ϳ��� ���������� 
        //üũ�ϰ� �ִµ�, üũ���� Ż���ϸ� 1��������Ʈ�� �ٲ۴�.
        //�� ��θ� Ž���ϴ� �߰��� ������ ��츦 ó���ϴ� ���̴�.
        List<Area> returnList = new List<Area>();
        foreach (List<Vector2Int> route in canRouteList)
        {
            foreach (Vector2Int xy in route)
            {
                //�� ��  ������ġ=�⹰�� ������ġ+ �� �� �ִ� �����ġ
                Vector2Int CanGoXY = piece.area.xy + xy;
                if (InsideMapCherk(CanGoXY) is false)
                {
                    continue;
                }
                Area area = mapAreas.Find(CanGoXY);
                if (AreaCherk.PieceMove.Allcherk(piece, area))
                {
                    returnList.Add(area);
                }
                else
                {
                    if(area.Get(out BuildingScript building))
                    {
                        if(building.���ݴ���ΰ� is true)
                        {
                            returnList.Add(area);
                            break;
                        }
                    }
                    break;
                }
            }
        }

        //��ȯ
        return returnList;

    }
    
    public List<Vector2Int> kingAround = new List<Vector2Int>()
    {
        new Vector2Int(-1,1),
        new Vector2Int(0,1),
        new Vector2Int(1,1),
        new Vector2Int(-1,-1),
        new Vector2Int(0,-1),
        new Vector2Int(1,-1),
        new Vector2Int(-1,0),
        new Vector2Int(1,0)
    };
    public List<Area> PieceCreate(User user)
    {
        List<Area> CanCreateAreaList = new List<Area>();
        Vector2Int KingXy = mapAreas.GetKing(user).area.xy;
        foreach (Vector2Int aroundXy in kingAround)
        {//ŷ ����带 �ҷ����� ������
            Vector2Int xy = aroundXy + KingXy;
            if (InsideMapCherk(xy))
            {//�����ϴ� ��� ������ �⹰��ǥ�� ������� ���� �ʳ��� �ִٸ� 
                if (AreaCherk.PieceCreate.Allcherk(mapAreas.Find(xy)))
                {//���ڸ��� �⹰�� �����Ϸ����Ҷ� ������ ������ �߰��Ѵ�.
                    CanCreateAreaList.Add(mapAreas.Find(xy));
                }
            }
        }

        return CanCreateAreaList;
    }
    static List<Vector2Int> buildingCreateAround = new List<Vector2Int>()
    {
        new Vector2Int(1,0),
        new Vector2Int(-1,0),
        new Vector2Int(0,1),
        new Vector2Int(0,-1),
    };
    public List<Area> BuildingCreate(BK bk, User user)
    {
        List<Area> CanCreateAreaList = new List<Area>();
        foreach (Area _area in mapAreas.ReturnAreaListThatHaveIT(ElementKind.piece))
        {//�⹰�� �ִ� ��� ��ġ�� �ҷ��´�
            if (_area.piece.user != user)
            {//�Ʊ��� �⹰�� �ִ� �Ʒ��Ƹ� �����Ѵ�.
                continue;
            }
            foreach (Vector2Int aroundXy in buildingCreateAround)
            {//�ǹ��Ǽ� ����带 �ҷ����� ������
                Vector2Int xy = _area.xy + aroundXy;
                if (InsideMapCherk(xy))
                {//�����ϴ� ��� ������ �⹰��ǥ�� ������� ���� �ʳ��� �ִٸ� 
                    if (AreaCherk.BuildingCreate.Allcherk(bk, mapAreas.Find(xy)))
                    {//���ڸ��� �ǹ��� �Ǽ��Ϸ����Ҷ� ������ ������ �߰��Ѵ�.
                        CanCreateAreaList.Add(mapAreas.Find(xy));
                    }
                }
            }
        }
        return CanCreateAreaList;
    }
    bool InsideMapCherk(Vector2Int xy)
    {
        if (xy.x < 0 || xy.x >= mapAreas.size.x
            || xy.y < 0 || xy.y >= mapAreas.size.y)
        {
            return false;
        }
        return true;
    }

  
}
public static class AreaCherk
{
    public static class PieceMove
    {
        public static bool Allcherk(PieceScript piece, Area toArea)
        {
            if (toArea.Get(out PieceScript toPiece))
            {
                if (PieceToPiece(piece, toPiece) is false)
                {
                    return false;
                }
            }
            if (toArea.Get(out BuildingScript building))
            {
                if (PieceTobuilding(piece, building) is false)
                {
                    return false;
                }
            }
            if (toArea.Get(out TileScript tile))
            {
                if (PieceToTile(piece, tile) is false)
                {
                    return false;
                }
            }
            return true;
        }
        static bool PieceToPiece(PieceScript piece, PieceScript toPiece)
        {
            if (piece.user == toPiece.user)
            {
                return false;
            }
            return true;
        }
        static bool PieceToTile(PieceScript piece, TileScript toTile)
        {
            if (toTile.EnterAble is false)
            {
                return false;
            }
            return true;
        }
        static bool PieceTobuilding(PieceScript piece, BuildingScript toBuilding)
        {
            if (toBuilding.�⹰��������Ѱ� is false)//snowWall
            {
                return false;
            }
            return true;
        }
    }
    public static class BuildingCreate
    {
        public static bool Allcherk(BK bk, Area area)
        {
            if (area.Cherk(ElementKind.piece))
            {
                return false;
            }
            if (area.Cherk(ElementKind.building))
            {
                return false;
            }

            return true;
        }
    }
    public static class PieceCreate
    {
        public static bool Allcherk(Area toArea)
        {
            if (toArea.Cherk(ElementKind.piece))
            {
                return false;
            }
            if (toArea.Cherk(ElementKind.building))
            {
                return false;
            }
            if (toArea.Get(out TileScript tile))
            {
                if (tile.EnterAble is false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

