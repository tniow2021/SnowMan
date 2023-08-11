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
    //게임스크립트에서 호출되는 함수
    public bool PieceMove(Order.PieceMove order)
    {
        if(order.piece.user!=order.user)
        {
            return false;
        }
        List<Area> canGoXyList = areaListCherk.PieceMove(order.piece);
        if (canGoXyList.Count == 0) MonoBehaviour.print("시발");
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
        //자원검사
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
public partial class GameLogic//턴관련
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
public partial class GameLogic//내부처리 로직
{
    void PieceAction(Order.PieceMove order)
    {
        //이동하기전
        if(order.toArea.Get(out BuildingScript building))
        {
            if (building.공격대상인가 is true)
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
            else//이동한 곳에서 맞닿들인 건물,타일,아이템
            {
                if(order.toArea.Get(out BuildingScript building2))
                {
                    if(building2.kind==BK.Boom)//폭발이펙트 처리ㄴ
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
        //piece.AbleRoute를 (0,0)을 기준으로 잘라 2차원 리스트에 저장한다.
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
        //그 2차원리스트에서 1차원리스트를 하나씩 꺼낸다음에 
        //체크하고 넣는데, 체크에서 탈락하면 1차원리스트를 바꾼다.
        //즉 경로를 탐색하다 중간에 막히는 경우를 처리하는 것이다.
        List<Area> returnList = new List<Area>();
        foreach (List<Vector2Int> route in canRouteList)
        {
            foreach (Vector2Int xy in route)
            {
                //갈 수  절대위치=기물의 현재위치+ 갈 수 있는 상대위치
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
                        if(building.공격대상인가 is true)
                        {
                            returnList.Add(area);
                            break;
                        }
                    }
                    break;
                }
            }
        }

        //반환
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
        {//킹 어라운드를 불러오고 더힌디
            Vector2Int xy = aroundXy + KingXy;
            if (InsideMapCherk(xy))
            {//존재하는 모든 각각의 기물좌표와 어라운드의 합이 맵내에 있다면 
                if (AreaCherk.PieceCreate.Allcherk(mapAreas.Find(xy)))
                {//그자리에 기물을 생성하려고할때 문제가 없으면 추가한다.
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
        {//기물이 있는 모든 위치를 불러온다
            if (_area.piece.user != user)
            {//아군의 기물이 있는 아레아만 선택한다.
                continue;
            }
            foreach (Vector2Int aroundXy in buildingCreateAround)
            {//건물건설 어라운드를 불러오고 더힌디
                Vector2Int xy = _area.xy + aroundXy;
                if (InsideMapCherk(xy))
                {//존재하는 모든 각각의 기물좌표와 어라운드의 합이 맵내에 있다면 
                    if (AreaCherk.BuildingCreate.Allcherk(bk, mapAreas.Find(xy)))
                    {//그자리에 건물을 건설하려고할때 문제가 없으면 추가한다.
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
            if (toBuilding.기물통과가능한가 is false)//snowWall
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

