using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    public class PieceMove
    {
        public PieceScript piece;
        public Area toArea;
        public void Clear()
        {
            piece = null;
            toArea = null;
        }
    }
    

}
