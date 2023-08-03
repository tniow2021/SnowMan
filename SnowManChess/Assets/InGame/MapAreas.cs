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
}
