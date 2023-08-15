using System.Collections;
using System.Collections.Generic;

namespace test
{


    public class XY
    {
        public XY(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
        public XY()
        {
            x = 0;
            y = 0;
        }
        public int x;
        public int y;
        public static bool Equal(XY xy1, XY xy2)
        {
            if (xy1.x == xy2.x && xy1.y == xy2.y)
            {
                return true;
            }
            else return false;
        }
        public static XY Plus(XY xy1, XY xy2)
        {
            XY returnXY = xy1;
            returnXY.x += xy2.x;
            returnXY.y += xy2.y;
            return returnXY;
        }
        public static byte[] ListToByteArray(List<XY> xys)
        {
            List<byte> byteList = new List<byte>();
            foreach (XY xy in xys)
            {
                byteList.Add((byte)xy.x);
                byteList.Add((byte)xy.y);
            }
            byte[] ggg = byteList.ToArray();
            return byteList.ToArray();
        }
        public static XY[] ByteArrayToXYArray(byte[] binary)
        {
            List<XY> XYList = new List<XY>();
            for (int i = 0; i < binary.Length; i += 2)//Length는 Count랑 같은 놈
            {
                XY xy = new XY();
                xy.x = binary[i];
                xy.y = binary[i + 1];
                XYList.Add(xy);
            }

            return XYList.ToArray();
        }
        public static XY ToXY(Vector2Int v2)
        {
            XY xy = new XY();
            xy.x = v2.x;
            xy.y = v2.y;
            return xy;
        }
        public static Vector2Int TOv2(XY xy)
        {
            return new Vector2Int(xy.x, xy.y);
        }
    }
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

        public class MapCreate
        {

        }
        public class MapDelete
        {

        }
        public class PieceMove
        {
            public XY piece, ToTile;
            public int UserID;//얘같은 int형도 닽이 바이너리로 만들 수 있는 방법을 떠올려보자.
            public byte[] GetBinary()
            {
                List<XY> XYList = new List<XY>()
            {piece,
            ToTile};
                return XY.ListToByteArray(XYList);
            }
            public PieceMove GetCommand(byte[] binary)
            {
                XY[] xy = XY.ByteArrayToXYArray(binary);
                piece = xy[0];
                ToTile = xy[1];
                return this;
            }
        }
        public class PieceCreate
        {
            public PK pieceKind;
            public int X;
            public int y;
            public byte[] GetBinary()
            {
                byte[] binary = new byte[] {
                ((byte)pieceKind),
                ((byte)X),
                ((byte)y) };
                return binary;
            }
            public PieceCreate GetCommand(byte[] binary)
            {
                pieceKind = (PK)binary[0];
                X = (int)binary[1];
                y = (int)binary[2];
                return this;
            }
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

    public partial class Command
    {

    }
}