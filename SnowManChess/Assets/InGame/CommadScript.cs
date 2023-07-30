using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Command
{
    public enum Kind
    {
        MapCreate,
        MapDelete,

        PieceMove,
        PieceCreate,
        PieceDelete,

        CreateItem,
        GetItem,

        CreateBuliding,
        DeleteBulifing,

        Hot//¿Â³­È­
    }
    public Kind kind;
    public class ToSer
    {
        public class MapCreate
        {

        }
        public class MapDelete
        {

        }
        public class PieceMove
        {

        }
        public class PieceCreate
        {
            public PK pieceKind1;
            public int X2;
            public int y3;
        }
        public class PieceDelete
        {

        }
        public class CreateItem
        {

        }
        public class GetItem
        {

        }
        public class CreateBuliding
        {

        }
        public class DeleteBulifing
        {

        }
        public class Hot
        {

        }
    }

    public class ToUni
    {
        public class MapCreate
        {

        }
        public class MapDelete
        {

        }
        public class PieceMove
        {

        }
        public class PieceCreate
        {
            public byte pieceKind;
            public byte x;
            public byte y;
            public byte ID;
        }
        public class PieceDelete
        {

        }
        public class CreateItem
        {

        }
        public class GetItem
        {

        }
        public class CreateBuliding
        {

        }
        public class DeleteBulifing
        {

        }
        public class Hot
        {

        }
    }
}
    
public partial class Command
{

}
