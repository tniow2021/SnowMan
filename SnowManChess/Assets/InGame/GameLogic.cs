using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class GameLogic
{
    MapAreas mapAreas;
    public GameLogic(MapAreas _mapAreas)
    {
        mapAreas = _mapAreas;
    }
    //���ӽ�ũ��Ʈ���� ȣ��Ǵ� �Լ�
    public bool PieceMove(Order.PieceMove order)
    {
        if(order.piece.user != order.user)
        {
            return false;
        }
        List<Vector2Int> canGoXyList = AreaListOfItCanDo(order.piece);
        if (canGoXyList.Exists(x => Equals(x, order.toArea.xy)))
        {
            PieceAction(order);
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool PieceCreate(Order.PieceCreate order)
    {
        //������ 8ĭ�ۿ� ��ȯ�ȵ�.

        if(PieceCreateRuleCherk(order.area,order.user))
        {
            mapAreas.Create(order.pk, order.user, order.area);
            //���� ��ġ�� Ÿ���� ��Ÿ���� �̰߰Ը����.
            mapAreas.KingList.Find(s=>s.user==order.user).area.tile.ChangeHot();
            return true;
        }
        return false;
    }
    public bool BuildingCreate(Order.BuildingCreate order)
    {
        if(BuildingCreateCherk(order.area,order.user))
        {
            mapAreas.Create(order.bk, order.user, order.area);
            return true;
        }
        else
        {
            return false;
        }
    }
    List<Vector2Int> aroundBuildingCreate = new List<Vector2Int>()
    {
        new Vector2Int(1,0),
        new Vector2Int(-1,0),
        new Vector2Int(0,1),
        new Vector2Int(0,-1),
    };
    public List<Vector2Int> XyListBuildingCreateXY(User user)
    {
        List<Area> allAreaThatHavePiece = mapAreas.ReturnAreaListThatHaveIT(ElementKind.piece);
        List<Area> UsersArea = allAreaThatHavePiece.FindAll(s => s.piece.user == user);

        List<Vector2Int> CanCreateXY=new List<Vector2Int>();
        foreach(Area area in UsersArea)
        {
            CanCreateXY.AddRange(ReturnXyListInsideMapArea(area.xy, aroundBuildingCreate));
        }
        return CanCreateXY;

    }
    bool BuildingCreateCherk(Area area,User user)
    {
        if(area.Cherk(ElementKind.piece))
        {
            return false;
        }
        if (area.Cherk(ElementKind.building))
        {
            return false;
        }
        List<Vector2Int> CanCreateXY = XyListBuildingCreateXY( user);
        if(CanCreateXY .Exists(s=>Equals(s,area.xy))is false)
        {
            return false;
        }
        return true;
    }
    public void DisplayBuildingCreateXY(User user)
    {
        foreach(Vector2Int xy in XyListBuildingCreateXY(user))
        {
            mapAreas.Find(xy).tile.GetComponent<SpriteRenderer>().color = new Color32(168, 236, 255, 255);
        }
    }
    //���� �⹰�� ���� �� �ִ� ����.
    List<Vector2Int> KingPieceCreateAround = new List<Vector2Int>()
    {
        new Vector2Int(-1,1),
        new Vector2Int(0,1),
        new Vector2Int(1,1),
        new Vector2Int(-1,-1),
        new Vector2Int(0,-1),
        new Vector2Int(1,-1),
        new Vector2Int(-1,0),
        new Vector2Int(1,0),
    };
    bool PieceCreateRuleCherk(Area area,User user)
    {
        if(mapAreas.KingList.Find(s => s.user == user).area.tile.ISChangeHot()is false)
        {
            return false;
        }
        foreach(Vector2Int xy in KingPieceCreateAround)
        {
            Vector2Int aroundxy = xy + area.xy;//aroundxy=���� 8ĭ ��ǥ.
            if (IsXyInsideTheMapAreaCherk(aroundxy))
            {
                if(mapAreas.Find(aroundxy).Get(out PieceScript piece2))
                {
                    //���� 8ĭ�� �ִ� �⹰�� ��� �˻��ѵ� ŷ����Ʈ�� �ִ� �Ʊ� �հ� ���Ͽ�
                    //�Ʊ��� ���� 8ĭ���� �⹰�� ���� �� ����. 
                    if(piece2 == mapAreas.KingList.Find(s=>s.user==user))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public void DisplayCanCreatePieceArea(User user)
    {
        PieceScript King= mapAreas.KingList.Find(s => s.user == user);
        foreach (Vector2Int xy in KingPieceCreateAround)
        {
            Vector2Int aroundxy = xy + King.area.xy;//aroundxy=���� 8ĭ ��ǥ.
            if (IsXyInsideTheMapAreaCherk(aroundxy))
            {
                mapAreas.Find(aroundxy).tile.ColorBule();
            }
        }
    }
    public void DisplayAreaThatItCanDo(PieceScript piece,User user)
    {
        if (piece.user != user)
        {
            return;
        }
        List<Vector2Int> canGoXyList;
        canGoXyList = AreaListOfItCanDo(piece);
        foreach (Vector2Int xy in canGoXyList)
        {
            mapAreas.Find(xy).tile.TileCandidate();
        }
    }
}
public partial class GameLogic
{ 
    List<Vector2Int> AreaListOfItCanDo(PieceScript piece)
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

        List<Vector2Int> returnList = new List<Vector2Int>();
        foreach (List<Vector2Int> route in canRouteList)
        {
            foreach (Vector2Int xy in route)
            {
                //�� ��  ������ġ=�⹰�� ������ġ+ �� �� �ִ� �����ġ
                Vector2Int CanGoXY = piece.area.xy + xy;
                if (PieceMoveAllCherk(piece, CanGoXY))
                {
                    returnList.Add(CanGoXY);
                }
                else
                {
                    break;
                }
            }
        }

        //��ȯ
        return returnList;
    }
    bool PieceMoveAllCherk(PieceScript piece, Vector2Int toAreaXy)
    {
        if (IsXyInsideTheMapAreaCherk(toAreaXy))
        {
            Area toArea = mapAreas.Find(toAreaXy);
            if (Absolute_check(piece, toArea) && Relative_cherk(piece, toArea))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    bool IsXyInsideTheMapAreaCherk(Vector2Int xy)
    {
        if (xy.x < 0 || xy.x >= mapAreas.size.x
            || xy.y < 0 || xy.y >= mapAreas.size.y)
        {
            return false;
        }
        else
        {
            return true;
        }
    }//�ʾƷ��ƾȿ� �ش���ǥ�� ���� �� �ִ°�?
    List<Vector2Int>ReturnXyListInsideMapArea(Vector2Int center,List<Vector2Int>around)
    {
        List<Vector2Int> returnList = new List<Vector2Int>();
        foreach(var xy in around)
        {
            Vector2Int xy2 = center + xy;
            if(IsXyInsideTheMapAreaCherk(xy2))
            {
                returnList.Add(xy2);
            }
        }
        return returnList;
    }
    bool Absolute_check(PieceScript piece, Area toArea)//(���뼳��)
    {
        //(������)���� �⹰��ġ�θ� �� �� ���� �ڱ⳪ �ڱ� �⹰���� �������ϰ�,
        //�ڱ���ġ�� �ȵǰ�.
        if (piece.area == toArea)
        {
            return false;
        }
        return true;
    }
    bool Relative_cherk(PieceScript piece, Area toArea)//(��뼳����)
    {
        //�ϳ��� Ʋ���� false
        if (TileCherk(piece, toArea) is false) return false;
        if (PieceCherk(piece, toArea) is false) return false;
        if (BuildingCherk(piece, toArea) is false) return false;

        return true;
    }
}
public partial class GameLogic    //������ �������� �Լ��׷�
{ 
    bool TileCherk(PieceScript piece, Area toArea)
    {
        if(toArea.Get(out TileScript tile))
        {
            if(tile.kind==TK.Lake)
            {
                return false;
            }
        }
        else
        {
            return false;// Ÿ���� ���� ���¸� �����°� �´�.
        }

        return true;
    }
    bool PieceCherk(PieceScript piece, Area toArea)//(������)
    {
        if(toArea.Get(out PieceScript _piece))
        {
            if(piece.user == _piece.user)
            {
                return false;
            }
        }
        
        return true;
    }
    bool BuildingCherk(PieceScript piece, Area toArea)
    {
        if(toArea.Get(out BuildingScript building))
        {
            if(building.kind ==BK.SnowWall)
            {
                return false;
            }
        }
        return true;
    }
    
    
}
public partial class GameLogic//����ó�� ����
{
    void PieceAction(Order.PieceMove order)
    {
        if(order.piece.area.Pick(out PieceScript piece))
        {
            if (order.toArea.Put(piece, out PieceScript outPiece))
            {
                //�̺κ��� ���� ó���� ��.
                outPiece.ObjectDestory();
            }
            else//�̵��� ������ �´���� �ǹ�,Ÿ��,������
            {
                if(order.toArea.Get(out BuildingScript building))
                {
                    if(building.kind==BK.Boom)//��������Ʈ ó����
                    {
                        piece.ObjectDestory();
                        building.ObjectDestory();
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

                }
            }
        }
    }
}