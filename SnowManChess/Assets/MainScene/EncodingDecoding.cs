using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class DataIng //���ڵ�
{
    public static List<byte> MakeDataSet(List<byte> data)
    {
        //ù��°���� ������
        //�ι�°���� ����
        //����°���� ������

        List<byte> dataSet = new List<byte>();
        dataSet.AddRange(data);
        dataSet.Add(255);//���ᱸ����
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
public static partial class DataIng//���ڵ�
{
    public static void Decoding(List<byte> dataSet)
    {
        //�����ڷ� ������ �ϳ��� �����ͼ��� �޴´�.
        MonoBehaviour.print(dataSet);
    }
}
