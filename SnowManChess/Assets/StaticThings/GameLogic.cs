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
public class GameLogic
{
    public void PieceMove()
    {

    }

    //���ӽ�ũ��Ʈ���� ȣ��Ǵ� public void�� �Լ���
    //���ӷ���ó���� ���� privite Order ���Լ��� ������?
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
