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
    static GameLogic instance;
    List<List<MapArea>> mapArea;
    public static GameLogic GetInstance()
    {
        return instance;
    }
    public GameLogic(List<List<MapArea>> _mapArea)
    {
        mapArea = _mapArea;
    }

    MapArea GetMapArea(XY xy)
    {
        return mapArea[xy.x][xy.y];
    }
    public void PieceMove(Command.PieceMove order)//UserID�� ���߿�
    {
        XY Piece = order.piece;
        XY ToTile = order.ToTile;

        //���ǰ˻�1
        bool CanIgo1 = false;
        MonoBehaviour.print(1);
        //�� �� �ִ� ��ġ�ΰ� üũ
        foreach(Vector2Int coordinate in GetMapArea(Piece).Piece.AbleCoordinate)
        {
            if (Vector2Int.Equals(XY.TOv2(ToTile), coordinate+XY.TOv2(Piece)))
            {
                CanIgo1 = true;
                break;
            }
        }
        if (CanIgo1 is false) return;
        MonoBehaviour.print(2);
        //���ǰ˻�2
        /*
         * 1. ���� ������
         * 2. ȣ����
         */
        bool CanIgo2 = true;
        if (GetMapArea(ToTile).Buliding is not null) if(GetMapArea(ToTile).Buliding.kind == BK.SnowWall)//������ �ڸ��� ������������
        {
                CanIgo2 = false;
        }
        if(GetMapArea(ToTile).Tile.kind==TK.Lake)
        {
            PieceDestoroy(Piece);
            return;
        }
        //���� ��Ÿ���� ����.
        //if (GetMapArea(ToTile).Tile is not null)//������ �ڸ��� �⹰�� ������
        //{
        //    CanIgo = false;
        //}
        MonoBehaviour.print(3);
        if (CanIgo2 is true)//�̵�����
        {
            if (GetMapArea(ToTile).Piece is null)//������ �ڸ��� �⹰�� ������
            {
                //������
                GetMapArea(Piece).Piece.transform.localPosition
                = new Vector3(ToTile.x, ToTile.y, 0);
                //��ǥ����
                GetMapArea(Piece).Piece.Coordinate = XY.TOv2(ToTile);
                //��ũ��Ʈ ��ü
                GetMapArea(ToTile).Piece = GetMapArea(Piece).Piece;
                GetMapArea(Piece).Piece = null;
            }
            else if (GetMapArea(ToTile).Piece is not null)//������ �ڸ��� �⹰�� ������
            {
                //������
                GetMapArea(Piece).Piece.transform.localPosition
                = new Vector3(ToTile.x, ToTile.y, 0);
                //��ǥ����
                GetMapArea(Piece).Piece.Coordinate = XY.TOv2(ToTile);
                //�����⹰ �ı�
                PieceDestoroy(ToTile);
                //��ũ��Ʈ��ü
                GetMapArea(ToTile).Piece = GetMapArea(Piece).Piece;
                GetMapArea(Piece).Piece = null;
            }
        }

    }
    void PieceDestoroy(XY xy)
    {
        if(GetMapArea(xy).Piece.Kind==PK.King)//�й�
        {

        }
        GetMapArea(xy).Piece.ObjectDestory();
    }
}
