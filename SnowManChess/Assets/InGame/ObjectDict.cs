using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDict : MonoBehaviour
{
    public static ObjectDict Instance;
    ObjectDict()
    {
        Instance = this;
    }
    public Area area;
    public List<TileScript> tiles = new List<TileScript>();

    public List<PieceScript> Pieces = new List<PieceScript>();

    public List<BuildingScript> buildings = new List<BuildingScript>();

    public List<ItemScript> items = new List<ItemScript>();

    public bool Findscript(TK tk, out TileScript outParameter)
    {
        TileScript t = tiles.Find(s => s.kind == tk);
        if (t is not null)
        {
            outParameter = t;
            return true;
        }
        else
        {
            outParameter = null;
            return false;
        }
    }
    public bool Findscript(PK pk, out PieceScript outParameter)
    {
        PieceScript t = Pieces.Find(s => s.kind == pk);
        if (t is not null)
        {
            outParameter = t;
            return true;
        }
        else
        {
            outParameter = null;
            return false;
        }
    }
    public bool Findscript(BK bk, out BuildingScript outParameter)
    {
        BuildingScript t = buildings.Find(s => s.kind == bk);
        if (t is not null)
        {
            outParameter = t;
            return true;
        }
        else
        {
            outParameter = null;
            return false;
        }
    }
    public bool Findscript(IK ik, out ItemScript outParameter)
    {
        ItemScript t = items.Find(s => s.kind == ik);
        if (t is not null)
        {
            outParameter = t;
            return true;
        }
        else
        {
            outParameter = null;
            return false;
        }
    }


    public bool FindObject(TK tk, out GameObject outParameter)
    {
        if (Findscript(tk, out TileScript temp) is true)
        {
            outParameter = temp.gameObject;
            return true;
        }
        else
        {
            outParameter = null;
            return false;
        }
    }
    public bool FindObject(PK pk, out GameObject outParameter)
    {
        if (Findscript(pk, out PieceScript temp) is true)
        {
            outParameter = temp.gameObject;
            return true;
        }
        else
        {
            outParameter = null;
            return false;
        }
    }
    public bool FindObject(BK bk, out GameObject outParameter)
    {
        if (Findscript(bk, out BuildingScript temp) is true)
        {
            outParameter = temp.gameObject;
            return true;
        }
        else
        {
            outParameter = null;
            return false;
        }
    }
    public bool FindObject(IK ik, out GameObject outParameter)
    {
        if (Findscript(ik, out ItemScript temp) is true)
        {
            outParameter = temp.gameObject;
            return true;
        }
        else
        {
            outParameter = null;
            return false;
        }
    }
}
