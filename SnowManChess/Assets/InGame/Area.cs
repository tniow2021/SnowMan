using System.Collections;
using System.Collections.Generic;

using System;
using UnityEngine;

public class Area : MonoBehaviour
{
    public MapAreas mapAreas;//역참조
    public Vector2Int xy;
    public TileScript tile;
    public PieceScript piece;
    public ItemScript item;
    public BuildingScript building;


    public bool BeMouseOnArea=false;
    void OnMouseEnter()
    {
        if (Input.touchCount == 1)
        {
            BeMouseOnArea = true;
            mapAreas.AreaEnteredEvent(this);
        }
    }
    void OnMouseExit()
    {
        BeMouseOnArea = false;
    }
    public bool Get(out PieceScript OutpieceScript)
    {
        if(piece is not null)
        {
            OutpieceScript = piece;
            return true;
        }
        else
        {
            OutpieceScript = null;
            return false;
        }
    }
    public bool Get(out TileScript OutTileScript)
    {
        if (tile is not null)
        {
            OutTileScript = tile;
            return true;
        }
        else
        {
            OutTileScript = null;
            return false;
        }
    }
    public bool Get(out BuildingScript OutBuildingScript)
    {
        if (building is not null)
        {
            OutBuildingScript = building;
            return true;
        }
        else
        {
            OutBuildingScript = null;
            return false;
        }
    }
    public bool Get(out ItemScript OutItemScript)
    {
        if (item is not null)
        {
            OutItemScript = item;
            return true;
        }
        else
        {
            OutItemScript = null;
            return false;
        }
    }

    /// <summary>
    /// 기존 요소가 이미 존재하면 true를 반환하고 해당요소를 out합니다.
    /// </summary>
    /// <param name="_tile"></param>
    /// <param name="old"></param>
    /// <returns></returns>
    public bool Put(TileScript _tile, out TileScript old)
    {
        _tile.transform.parent = transform;
        _tile.transform.localPosition = _tile.additionalLocalPositon;
        _tile.area = this;
        if (tile is null)
        {
            old = null;
            tile = _tile;
            return false;
        }
        else//tile에 이미 타일스크립트가 있으면
        {
            old = tile;
            tile = _tile;
            return true;
        }
    }
    /// <summary>
    /// 기존 요소가 이미 존재하면 true를 반환하고 해당요소를 out합니다.
    /// </summary>
    /// <param name="_tile"></param>
    /// <param name="old"></param>
    /// <returns></returns>
    public bool Put(PieceScript _piece, out PieceScript old)
    {
        _piece.transform.parent = transform;
        _piece.transform.localPosition = _piece.additionalLocalPositon;
        _piece.area = this;
        if (piece is null)
        {
            old = null;
            piece = _piece;
            return false;
        }
        else//tile에 이미 타일스크립트가 있으면
        {
           
            old = piece;
            piece = _piece;
            return true;
        }
    }
    /// <summary>
    /// 기존 요소가 이미 존재하면 true를 반환하고 해당요소를 out합니다.
    /// </summary>
    /// <param name="_tile"></param>
    /// <param name="old"></param>
    /// <returns></returns>
    public bool Put(ItemScript _item, out ItemScript old)
    {
        _item.transform.parent = transform;
        _item.transform.localPosition = _item.additionalLocalPositon;
        _item.area = this;
        if (item is null)
        {
            old = null;
            item = _item;
            return false;
        }
        else//tile에 이미 타일스크립트가 있으면
        {
            old = item;
            item = _item;
            return true;
        }
    }
    /// <summary>
    /// 기존 요소가 이미 존재하면 true를 반환하고 해당요소를 out합니다.
    /// </summary>
    /// <param name="_tile"></param>
    /// <param name="old"></param>
    /// <returns></returns>
    public bool Put(BuildingScript _building, out BuildingScript old)
    {
        _building.transform.parent = transform;
        _building.transform.localPosition = _building.additionalLocalPositon;
        _building.area = this;
        if (building is null)
        {
            old = null;
            building = _building;
            return false;
        }
        else//tile에 이미 타일스크립트가 있으면
        {
            old = building;
            building = _building;
            return true;
        }
    }
}
