using System.Collections;
using System.Collections.Generic;

using System;
using UnityEngine;

public class Area : MonoBehaviour
{
    public MapAreas mapAreas;//������
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
            mapAreas.EnterXySwitch(xy);
        }
    }
    void OnMouseExit()
    {
        BeMouseOnArea = false;
    }
    
    /// <summary>
    /// ���� ��Ұ� �̹� �����ϸ� true�� ��ȯ�ϰ� �ش��Ҹ� out�մϴ�.
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
        else//tile�� �̹� Ÿ�Ͻ�ũ��Ʈ�� ������
        {
            old = tile;
            tile = _tile;
            return true;
        }
    }
    /// <summary>
    /// ���� ��Ұ� �̹� �����ϸ� true�� ��ȯ�ϰ� �ش��Ҹ� out�մϴ�.
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
        else//tile�� �̹� Ÿ�Ͻ�ũ��Ʈ�� ������
        {
           
            old = piece;
            piece = _piece;
            return true;
        }
    }
    /// <summary>
    /// ���� ��Ұ� �̹� �����ϸ� true�� ��ȯ�ϰ� �ش��Ҹ� out�մϴ�.
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
        else//tile�� �̹� Ÿ�Ͻ�ũ��Ʈ�� ������
        {
            old = item;
            item = _item;
            return true;
        }
    }
    /// <summary>
    /// ���� ��Ұ� �̹� �����ϸ� true�� ��ȯ�ϰ� �ش��Ҹ� out�մϴ�.
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
        else//tile�� �̹� Ÿ�Ͻ�ũ��Ʈ�� ������
        {
            old = building;
            building = _building;
            return true;
        }
    }
}
