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
    //게임스크립트에서 Order를 받는 함수
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
        //piece.AbleRoute를 (0,0)을 기준으로 잘라 2차원 리스트에 저장한다.
        List<List<Vector2Int>> canRouteList = new List<List<Vector2Int>>();
        List<Vector2Int> tempRoute = new List<Vector2Int>();
        for (int i=0;i< piece.AbleRoute.Count;i++)
        {
            if (Equals(piece.AbleRoute[i],new Vector2Int(0,0)))
            {
                canRouteList.Add(tempRoute);
                tempRoute = new List<Vector2Int>();
            }
            else
            {
                tempRoute.Add(piece.AbleRoute[i]);
            }
        }
        //그 2차원리스트에서 1차원리스트를 하나씩 꺼낸다음에 
        //체크하고 넣는데, 체크에서 탈락하면 1차원리스트를 바꾼다.
        //즉 일종의 갈 수 있는 모든 길을 표시하는 알고리즘

        List<Vector2Int> returnList = new List<Vector2Int>();
        foreach (List<Vector2Int> route in canRouteList)
        {
            foreach(Vector2Int xy in route)
            {
                //갈 수  절대위치=기물의 현재위치+ 갈 수 있는 상대위치
                Vector2Int CanGoXY = piece.area.xy + xy;
                if (
                    IsXyInsideTheMapArea(CanGoXY)
                    &&Absolute_check(piece, CanGoXY)
                    &&Relative_cherk(piece, CanGoXY)
                    )
                {
                    returnList.Add(CanGoXY);
                }
                else
                {
                    break;
                }
            }
            
        }

        //반환
        return returnList;
    }
    bool Absolute_check(PieceScript piece,Vector2Int toAreaXy)//(절대설정)
    {
        //(구현중)상대방 기물위치로만 갈 수 있지 자기나 자기 기물한텐 가지못하고,
        //자기위치는 안되고.
        if(piece.area==mapAreas.Find(toAreaXy))
        {
            return false;
        }    
        return true;
    }
    //앞으로 복잡해질 함수
    bool Relative_cherk(PieceScript piece, Vector2Int toAreaXy)//(상대설정)
    {
        //가려는 자리에 건물이 있으면 안된다.
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
public partial class GameLogic//내부처리 로직
{
    void PieceAction(Order.PieceMove order)
    {
        if(order.piece.area.Pick(out PieceScript piece))
        {
            if (order.toArea.Put(piece, out PieceScript outPiece))
            {
                //이부분은 따로 처리할 것.
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
