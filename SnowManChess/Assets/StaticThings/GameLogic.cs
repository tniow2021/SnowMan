using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserList
{
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
    public User(string _name,int _point,int _ID)
    {
        name = _name;
        point = _point;
        ID = _ID;
    }
    public int ID;
    public string name;
    public int point;

}
public partial class GameLogic
{
    MapAreas mapAreas;
    public GameLogic(MapAreas _mapAreas)
    {
        mapAreas = _mapAreas;
    }
    //���ӽ�ũ��Ʈ���� Order�� �޴� �Լ�
    public bool PieceMove(Order.PieceMove order)
    {
        List<Vector2Int> canGoXyList = AreaListOfItCanDo(order.piece);
        MonoBehaviour.print(canGoXyList);
        MonoBehaviour.print(order.piece.area.xy);
        if (canGoXyList.Exists(x=>Equals(x,order.toArea.xy)))
        {
            PieceAction(order);
            return true;
        }
        else
        {
            return false;
        }
    }

    //
    public void DisplayAreaThatItCanDo(PieceScript piece)
    {
        List<Vector2Int> canGoXyList;
        canGoXyList = AreaListOfItCanDo(piece);
        foreach(Vector2Int xy in canGoXyList)
        {
            mapAreas.Find(xy).tile.TileCandidate();
        }
    }
    List<Vector2Int> AreaListOfItCanDo(PieceScript piece)
    {
        List<Vector2Int> canGoXyList = new List<Vector2Int>();
        foreach (Vector2Int xy in piece.AbleCoordinate)
        {
            //�� ��  ������ġ=�⹰�� ������ġ+ �� �� �ִ� �����ġ
            Vector2Int AbleXY = piece.area.xy + xy;
            if(IsXyInsideTheMapArea(AbleXY))//��ǥüũ
            {
                if (Absolute_check(piece, AbleXY))
                {
                    if (Relative_cherk(piece, AbleXY))
                    {
                        canGoXyList.Add(AbleXY);
                    }
                }
            }
        }
        //��ȯ
        return canGoXyList;
    }
    bool Absolute_check(PieceScript piece,Vector2Int toAreaXy)//(���뼳��)
    {
        //(������)���� �⹰��ġ�θ� �� �� ���� �ڱ⳪ �ڱ� �⹰���� �������ϰ�,
        //�ڱ���ġ�� �ȵǰ�.
        if(piece.area==mapAreas.Find(toAreaXy))
        {
            return false;
        }    
        return true;
    }
    //������ �������� �Լ�
    bool Relative_cherk(PieceScript piece, Vector2Int toAreaXy)//(��뼳��)
    {
        //������ �ڸ��� �ǹ��� ������ �ȵȴ�.
        if (mapAreas.Find(toAreaXy).building is not null)
        {
            return false;
        }

        return true;
    }
    bool IsXyInsideTheMapArea(Vector2Int xy)
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

//public class GameLogic
//{
//    static GameLogic instance;
//    MapAreas mapArea;
//    public static GameLogic GetInstance()
//    {
//        return instance;
//    }
//    public GameLogic(MapAreas _mapArea)
//    {
//        mapArea = _mapArea;
//    }

//    Area FindArea(Vector2Int xy)
//    {
//        return mapArea.Find(xy);
//    }
//    public void PieceMove(Command.PieceMove order)//UserID�� ���߿�
//    {
//        Vector2Int Piece = XY.TOv2(order.piece);
//        Vector2Int ToTile = XY.TOv2(order.ToTile);

//        //���ǰ˻�1
//        bool CanIgo1 = false;
//        MonoBehaviour.print(1);
//        //�� �� �ִ� ��ġ�ΰ� üũ
//        foreach (Vector2Int coordinate in FindArea(Piece).piece.AbleCoordinate)
//        {
//            if (Vector2Int.Equals(ToTile, coordinate + Piece))
//            {
//                CanIgo1 = true;
//                break;
//            }
//        }
//        if (CanIgo1 is false) return;
//        MonoBehaviour.print(2);
//        //���ǰ˻�2
//        /*
//         * 1. ���� ������
//         * 2. ȣ����
//         */
//        bool CanIgo2 = true;
//        if (FindArea(ToTile).building is not null) if (FindArea(ToTile).building.kind == BK.SnowWall)//������ �ڸ��� ������������
//            {
//                CanIgo2 = false;
//            }
//        if (FindArea(ToTile).tile.kind == TK.Lake)
//        {
//            PieceDestoroy(Piece);
//            return;
//        }
//        //���� ��Ÿ���� ����.
//        //if (FindArea(ToTile).Tile is not null)//������ �ڸ��� �⹰�� ������
//        //{
//        //    CanIgo = false;
//        //}
//        MonoBehaviour.print(3);
//        if (CanIgo2 is true)//�̵�����
//        {
//            if (FindArea(ToTile).piece is null)//������ �ڸ��� �⹰�� ������
//            {
//                //������
//                FindArea(Piece).piece.transform.localPosition
//                = new Vector3(ToTile.x, ToTile.y, 0);

//                //��ũ��Ʈ ��ü
//                FindArea(ToTile).piece = FindArea(Piece).piece;
//                FindArea(Piece).piece = null;
//            }
//            else if (FindArea(ToTile).piece is not null)//������ �ڸ��� �⹰�� ������
//            {
//                //������
//                FindArea(Piece).piece.transform.localPosition
//                = new Vector3(ToTile.x, ToTile.y, 0);
//                //��ǥ����

//                //�����⹰ �ı�
//                PieceDestoroy(ToTile);
//                //��ũ��Ʈ��ü
//                FindArea(ToTile).piece = FindArea(Piece).piece;
//                FindArea(Piece).piece = null;
//            }
//        }

//    }
//    void PieceDestoroy(Vector2Int xy)
//    {
//     //�й� ���� �ֱ�
//        FindArea(xy).piece.ObjectDestory();
//        FindArea(xy).piece = null;
//    }
//}
