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
    //게임스크립트에서 호출되는 함수
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
        //왕주위 8칸밖에 소환안됨.

        if(PieceCreateRuleCherk(order.area,order.user))
        {
            mapAreas.Create(order.pk, order.user, order.area);
            //왕이 위치한 타일의 눈타일을 뜨겁게만든다.
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
    //왕이 기물을 만들 수 있는 범위.
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
            Vector2Int aroundxy = xy + area.xy;//aroundxy=주위 8칸 좌표.
            if (IsXyInsideTheMapAreaCherk(aroundxy))
            {
                if(mapAreas.Find(aroundxy).Get(out PieceScript piece2))
                {
                    //주위 8칸에 있는 기물을 모두 검색한뒤 킹리스트에 있는 아군 왕과 비교하여
                    //아군왕 주위 8칸에만 기물을 놓을 수 있음. 
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
            Vector2Int aroundxy = xy + King.area.xy;//aroundxy=주위 8칸 좌표.
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

        List<Vector2Int> returnList = new List<Vector2Int>();
        foreach (List<Vector2Int> route in canRouteList)
        {
            foreach (Vector2Int xy in route)
            {
                //갈 수  절대위치=기물의 현재위치+ 갈 수 있는 상대위치
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

        //반환
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
    }//맵아레아안에 해당좌표가 있을 수 있는가?
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
    bool Absolute_check(PieceScript piece, Area toArea)//(절대설정)
    {
        //(구현중)상대방 기물위치로만 갈 수 있지 자기나 자기 기물한텐 가지못하고,
        //자기위치는 안되고.
        if (piece.area == toArea)
        {
            return false;
        }
        return true;
    }
    bool Relative_cherk(PieceScript piece, Area toArea)//(상대설정들)
    {
        //하나라도 틀리면 false
        if (TileCherk(piece, toArea) is false) return false;
        if (PieceCherk(piece, toArea) is false) return false;
        if (BuildingCherk(piece, toArea) is false) return false;

        return true;
    }
}
public partial class GameLogic    //앞으로 복잡해질 함수그룹
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
            return false;// 타일이 없는 상태면 못가는게 맞다.
        }

        return true;
    }
    bool PieceCherk(PieceScript piece, Area toArea)//(수정중)
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
public partial class GameLogic//내부처리 로직
{
    void PieceAction(Order.PieceMove order)
    {
        if(order.piece.area.Pick(out PieceScript piece))
        {
            if (order.toArea.Put(piece, out PieceScript outPiece))
            {
                //이부분은 따로 처리할 것.
                outPiece.ObjectDestory();
            }
            else//이동한 곳에서 맞닿들인 건물,타일,아이템
            {
                if(order.toArea.Get(out BuildingScript building))
                {
                    if(building.kind==BK.Boom)//폭발이펙트 처리ㄴ
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