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

    //게임스크립트에서 호출되는 public void형 함수와
    //게임로직처리를 위한 privite Order 형함수로 나누기?
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
//    public void PieceMove(Command.PieceMove order)//UserID는 나중에
//    {
//        Vector2Int Piece = XY.TOv2(order.piece);
//        Vector2Int ToTile = XY.TOv2(order.ToTile);

//        //조건검사1
//        bool CanIgo1 = false;
//        MonoBehaviour.print(1);
//        //갈 수 있는 위치인가 체크
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
//        //조건검사2
//        /*
//         * 1. 벽이 있으면
//         * 2. 호수면
//         */
//        bool CanIgo2 = true;
//        if (FindArea(ToTile).building is not null) if (FindArea(ToTile).building.kind == BK.SnowWall)//가려는 자리에 눈벽이있으면
//            {
//                CanIgo2 = false;
//            }
//        if (FindArea(ToTile).tile.kind == TK.Lake)
//        {
//            PieceDestoroy(Piece);
//            return;
//        }
//        //아직 자타구분 안함.
//        //if (FindArea(ToTile).Tile is not null)//가려는 자리에 기물이 있으면
//        //{
//        //    CanIgo = false;
//        //}
//        MonoBehaviour.print(3);
//        if (CanIgo2 is true)//이동가능
//        {
//            if (FindArea(ToTile).piece is null)//가려는 자리에 기물이 없으면
//            {
//                //움직임
//                FindArea(Piece).piece.transform.localPosition
//                = new Vector3(ToTile.x, ToTile.y, 0);

//                //스크립트 교체
//                FindArea(ToTile).piece = FindArea(Piece).piece;
//                FindArea(Piece).piece = null;
//            }
//            else if (FindArea(ToTile).piece is not null)//가려는 자리에 기물이 있으면
//            {
//                //움직임
//                FindArea(Piece).piece.transform.localPosition
//                = new Vector3(ToTile.x, ToTile.y, 0);
//                //좌표지정

//                //기존기물 파괴
//                PieceDestoroy(ToTile);
//                //스크립트교체
//                FindArea(ToTile).piece = FindArea(Piece).piece;
//                FindArea(Piece).piece = null;
//            }
//        }

//    }
//    void PieceDestoroy(Vector2Int xy)
//    {
//     //패배 조건 넣기
//        FindArea(xy).piece.ObjectDestory();
//        FindArea(xy).piece = null;
//    }
//}
