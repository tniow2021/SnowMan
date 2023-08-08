using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAreas : MonoBehaviour
{
    //생성
    List<List<Area>> areas = new List<List<Area>>();

    public Vector2Int size
    {
        get { return sizeXy; }
    }
    Vector2Int sizeXy = new Vector2Int();
    public MapAreas(Map map, Vector2Int mapSize)
    {
        areas = new List<List<Area>>();
        sizeXy.x = mapSize.x;
        sizeXy.y = mapSize.y;
        for (int i = 0; i < mapSize.x; i++)
        {
            List<Area> row = new List<Area>();
            for (int j = 0; j < mapSize.y; j++)
            {
                GameObject newareaObject = MonoBehaviour.Instantiate(ObjectDict.Instance.area.gameObject);
                newareaObject.transform.parent = map.transform;
                newareaObject.transform.localPosition = new Vector3(i, j, 0);
                Area newArea = newareaObject.GetComponent<Area>();
                newArea.mapAreas = this;
                newArea.xy = new Vector2Int(i, j);//좌표지정
                row.Add(newArea);
            }
            areas.Add(row);
        }
    }
    
    //Create에서 채워지는 리스트듷
    List<PieceScript> KingList = new List<PieceScript>();
    public List<TileScript> coolSnowTileList = new List<TileScript>();
    public void CoolTileToHotTile(TileScript tile)
    {
        tile.ChangeHot();
        coolSnowTileList.Remove(tile);
    }

    public PieceScript GetKing(User user)//임시
    {
         foreach(var a in areas)
        {
            foreach(var a2 in a)
            {
                if(a2.Get(out PieceScript piece))
                {
                    if(piece.user==user)
                    {
                        if(piece.kind==PK.Aking||piece.kind==PK.Bking)
                        {
                            return piece;
                        }
                    }
                }
            }
        }
        return null;
    }
    //행동
    //게임스크립트로 가는놈
    Area areaEntered;
    public void AreaEnteredEvent(Area _area)
    {
        areaEntered = _area;
    }
    public bool GetAreaTouched(out PieceScript outpieceScript)
    {
        if (areaEntered is not null)
        {
            if (areaEntered.BeMouseOnArea is true)
            {
                if (areaEntered.Get(out PieceScript piece))
                {
                    outpieceScript = piece;
                    return true;
                }
            }
        }
        outpieceScript = null;
        return false;
    }
    public bool GetAreaTouched(out TileScript outTileScript)
    {
        if (areaEntered is not null)
        {
            if (areaEntered.BeMouseOnArea is true)
            {
                if (areaEntered.Get(out TileScript tile))
                {
                    outTileScript = tile;
                    return true;
                }
            }
        }
        outTileScript = null;
        return false;
    }
    public bool GetAreaTouched(out BuildingScript outBuildingScript)
    {
        if (areaEntered is not null)
        {
            if (areaEntered.BeMouseOnArea is true)
            {
                if (areaEntered.Get(out BuildingScript building))
                {
                    outBuildingScript = building;
                    return true;
                }
            }
        }
        outBuildingScript = null;
        return false;
    }
    public bool GetAreaTouched(out ItemScript outItemScript)
    {
        if (areaEntered is not null)
        {
            if (areaEntered.BeMouseOnArea is true)
            {
                if (areaEntered.Get(out ItemScript item))
                {
                    outItemScript = item;
                    return true;
                }
            }
        }
        outItemScript = null;
        return false;
    }
    public bool GetAreaTouched(out Area outArea)
    {
        if (areaEntered is not null)
        {
            if (areaEntered.BeMouseOnArea is true)
            {
                outArea = areaEntered;
                return true;
            }
        }
        outArea = null;
        return false;
    }


    //메소드
    public bool Insert(Vector2Int Index, Area area)
    {
        if (Index.x > sizeXy.x - 1 || Index.x < 0) return false;
        else if (Index.y > sizeXy.y - 1 || Index.y < 0) return false;

        areas[Index.x][Index.y] = area;
        return true;
    }
    public Area Find(int x, int y)
    {
        return areas[x][y];
    }
    public Area Find(Vector2Int index)
    {
        return areas[index.x][index.y];
    }

    public void Turn(int turnNumber)
    {
        for (int i = 0; i < sizeXy.x; i++)
        {
            for (int j = 0; j < sizeXy.y; j++)
            {
                areas[i][j].tile.Turn(turnNumber);
                areas[i][j].piece.Turn(turnNumber);
                areas[i][j].building.Turn(turnNumber);
                areas[i][j].item.Turn(turnNumber);
            }
        }
    }

    //왕이있으면 킹리스트에 왕을 삽입
    public void Create(PK pk, User user, Area area)
    {
        if (ObjectDict.Instance.FindObject(pk, out GameObject obj))
        {

            GameObject newPieceObject = Instantiate(obj);
            PieceScript newPieceScript = newPieceObject.GetComponent<PieceScript>();
            if (pk == PK.Aking || pk == PK.Bking)
            {
                KingList.Add(newPieceScript);
            }
            newPieceScript.user = user;
            if (area.Put(newPieceScript, out PieceScript oldPiece))
            {
                oldPiece.ObjectDestory();
            }
        }
    }
    public void Create(BK bk, User user, Area area)
    {
        print(area.xy);
        if (ObjectDict.Instance.FindObject(bk, out GameObject obj))
        {
            GameObject newBuildingObject = Instantiate(obj);
            BuildingScript newBuildingScript = newBuildingObject.GetComponent<BuildingScript>();
            newBuildingScript.user = user;
            if (area.Put(newBuildingScript, out BuildingScript oldBuilding))
            {
                oldBuilding.ObjectDestory();
            }
        }
    }
    public void Create(TK tk, Area area)
    {
        if (ObjectDict.Instance.FindObject(tk, out GameObject obj))
        {
            GameObject newTileObject = Instantiate(obj);
            TileScript newTileScript = newTileObject.GetComponent<TileScript>();
            if(newTileScript.IsHaveSnow is true)
            {
                coolSnowTileList.Add(newTileScript);
            }
            if (area.Put(newTileScript, out TileScript oldtile))
            {
                oldtile.ObjectDestory();
            }
        }
    }
    public void Create(IK ik, Area area)
    {
        if (ObjectDict.Instance.FindObject(ik, out GameObject obj))
        {
            GameObject newItemObject = Instantiate(obj);
            ItemScript newItemScript = newItemObject.GetComponent<ItemScript>();
            if (area.Put(newItemScript, out ItemScript oldItem))
            {
                oldItem.ObjectDestory();
            }
        }
    }
    public List<Area> ReturnAreaListThatHaveIT(ElementKind ek)
    {

        List<Area> allarea = new List<Area>();
        if (ek == ElementKind.piece)
        {
            foreach(List<Area> area1 in areas)
            {
                foreach (Area area in area1)
                {
                    if(area.Cherk(ElementKind.piece))
                    {
                        allarea.Add(area);
                    }
                }
            }
        }
        else if (ek == ElementKind.tile)
        {
            foreach (List<Area> area1 in areas)
            {
                foreach (Area area in area1)
                {
                    if (area.Cherk(ElementKind.tile))
                    {
                        allarea.Add(area);
                    }
                }
            }
        }
        else if (ek == ElementKind.building)
        {
            foreach (List<Area> area1 in areas)
            {
                foreach (Area area in area1)
                {
                    if (area.Cherk(ElementKind.building))
                    {
                        allarea.Add(area);
                    }
                }
            }
        }
        else if (ek == ElementKind.item)
        {
            foreach (List<Area> area1 in areas)
            {
                foreach (Area area in area1)
                {
                    if (area.Cherk(ElementKind.item))
                    {
                        allarea.Add(area);
                    }
                }
            }
        }
        return allarea;
    }
    public void Delete(PieceScript piece)
    {
        if(piece.area.Pick(out PieceScript outPiece))
        {
            outPiece.ObjectDestory();
        }
    }
    public void Delete(TileScript tile)
    {
        if (tile.area.Pick(out TileScript outTile))
        {
            outTile.ObjectDestory();
        }
    }
    public void Delete(BuildingScript building)
    {
        if (building.area.Pick(out BuildingScript outbuilding))
        {
            outbuilding.ObjectDestory();
        }
    }
    public void Delete(ItemScript item)
    {
        if (item.area.Pick(out ItemScript outitem))
        {
            outitem.ObjectDestory();
        }
    }
}
