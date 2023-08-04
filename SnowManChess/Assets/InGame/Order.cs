using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    public class PieceMove
    {
        public PieceScript piece;
        public Area toArea;
        public User user;
        public void Clear()
        {
            piece = null;
            toArea = null;
        }
    }
    public class PieceCreate
    {
        public PK pk;
        public User user;
        public Area area;
    }
    public class BuildingCreate
    {
        public BK bk;
        public Area area;
        public User user;
    }
    

}
