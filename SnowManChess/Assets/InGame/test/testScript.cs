using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ��
{
    �⹰,
    �ǹ�,
    Ÿ��
}
public class testScript : MonoBehaviour
{
    public �� �̷�;
    public BK bkk;
    public PK pkk;
    public TK tkk;

    public static �� ����;
    private void OnMouseDown()
    {
        if(�̷�==��.�ǹ�)
        {
            ���� = ��.�ǹ�;
            GameScript.instance.KindReceived(bkk);
        }
        if (�̷� == ��.�⹰)
        {
            ���� = ��.�⹰;
            GameScript.instance.KindReceived(pkk);
        }
        if (�̷� == ��.Ÿ��)
        {
            ���� = ��.Ÿ��;
            GameScript.instance.KindReceived(tkk);
        }

    }
}
