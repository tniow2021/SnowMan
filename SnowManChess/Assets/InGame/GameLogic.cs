using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager
{
    public User admin = new User(UserKind.admin, 0);
    List<User> Users = new List<User>();
    public void Add(User user)
    {
        Users.Add(user);
    }
    public int Count()
    {
        return Users.Count;
    }
}
public class User
{
    public User(UserKind _kind,int _ID,string _name="")
    {
        name = _name;
        ID = _ID;
        kind = _kind;
    }
    public UserKind kind { get; }
    public int ID { get; }
    public string name { get; }

}
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
        List<Vector2Int> canGoXyList = AreaListOfItCanDo(order.piece);
        MonoBehaviour.print(canGoXyList);
        MonoBehaviour.print(order.piece.area.xy);
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
    public void DisplayAreaThatItCanDo(PieceScript piece)
    {
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
                MonoBehaviour.print(CanGoXY);
                if (AllCherk(piece, CanGoXY))
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
    bool AllCherk(PieceScript piece, Vector2Int toAreaXy)
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
                MonoBehaviour.Destroy(outPiece.gameObject);
            }
        }
    }
}