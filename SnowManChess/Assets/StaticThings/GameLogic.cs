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
    public void PieceMove(Command.PieceMove order)//UserID는 나중에
    {
        XY Piece = order.piece;
        XY ToTile = order.ToTile;

        //조건검사1
        bool CanIgo1 = false;
        MonoBehaviour.print(1);
        //갈 수 있는 위치인가 체크
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
        //조건검사2
        /*
         * 1. 벽이 있으면
         * 2. 호수면
         */
        bool CanIgo2 = true;
        if (GetMapArea(ToTile).Buliding is not null) if(GetMapArea(ToTile).Buliding.kind == BK.SnowWall)//가려는 자리에 눈벽이있으면
        {
                CanIgo2 = false;
        }
        if(GetMapArea(ToTile).Tile.kind==TK.Lake)
        {
            PieceDestoroy(Piece);
            return;
        }
        //아직 자타구분 안함.
        //if (GetMapArea(ToTile).Tile is not null)//가려는 자리에 기물이 있으면
        //{
        //    CanIgo = false;
        //}
        MonoBehaviour.print(3);
        if (CanIgo2 is true)//이동가능
        {
            if (GetMapArea(ToTile).Piece is null)//가려는 자리에 기물이 없으면
            {
                //움직임
                GetMapArea(Piece).Piece.transform.localPosition
                = new Vector3(ToTile.x, ToTile.y, 0);
                //좌표지정
                GetMapArea(Piece).Piece.Coordinate = XY.TOv2(ToTile);
                //스크립트 교체
                GetMapArea(ToTile).Piece = GetMapArea(Piece).Piece;
                GetMapArea(Piece).Piece = null;
            }
            else if (GetMapArea(ToTile).Piece is not null)//가려는 자리에 기물이 있으면
            {
                //움직임
                GetMapArea(Piece).Piece.transform.localPosition
                = new Vector3(ToTile.x, ToTile.y, 0);
                //좌표지정
                GetMapArea(Piece).Piece.Coordinate = XY.TOv2(ToTile);
                //기존기물 파괴
                PieceDestoroy(ToTile);
                //스크립트교체
                GetMapArea(ToTile).Piece = GetMapArea(Piece).Piece;
                GetMapArea(Piece).Piece = null;
            }
        }

    }
    void PieceDestoroy(XY xy)
    {
        if(GetMapArea(xy).Piece.Kind==PK.King)//패배
        {

        }
        GetMapArea(xy).Piece.ObjectDestory();
    }
}
