using System;

using System.Collections;
using System.Collections.Generic;

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

        Hot//온난화
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
