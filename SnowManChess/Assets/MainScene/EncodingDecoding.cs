using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class DataIng //인코딩
{
    public static List<byte> MakeDataSet(List<byte> data)
    {
        //첫번째에는 구분자
        //두번째에는 길이
        //세번째에는 데이터

        List<byte> dataSet = new List<byte>();
        dataSet.AddRange(data);
        dataSet.Add(255);//종결구분자
        return dataSet;
    }
    public static List<byte> Encoding(Command.ToSer.PieceCreate order)
    {
        List<byte> data = new List<byte>();
        data.Add(((byte)Command.Kind.PieceCreate));
        data.Add(((byte)order.pieceKind1));
        data.Add(((byte)order.X2));
        data.Add(((byte)order.y3));
        return data;
    }
}
public static partial class DataIng//디코딩
{
    public static void Decoding(List<byte> dataSet)
    {
        //구분자로 나눠진 하나의 데이터셋을 받는다.
        MonoBehaviour.print(dataSet);
    }
}
