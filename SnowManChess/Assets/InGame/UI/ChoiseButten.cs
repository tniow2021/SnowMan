using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public enum ��
{
    �⹰,
    �ǹ�,
    ������,
    Ÿ��
}
public class ChoiseButten : MonoBehaviour
{
    public TurnConnect turnConnect;
    public BK bkk;
    public PK pkk;
    public TK tkk;

    public �� �̷�;


    public bool IsMyTurn = false;
    public void Down()
    {
        print($"{IsMyTurn},,{�̷�},,{pkk}");
        if (IsMyTurn is true)
        {
            if (�̷� == ��.�ǹ�)
            {
                GameScript.instance.KindReceived(bkk,�̷�);
            }
            if (�̷� == ��.�⹰)
            {
                GameScript.instance.KindReceived(pkk, �̷�);
            }
            if (�̷� == ��.Ÿ��)
            {
                GameScript.instance.KindReceived(tkk, �̷�);
            }
        }

    }
    public void Up()
    {
    }
    public void NowMyTurn()
    {
        IsMyTurn = true;
    }
    public void NowNotMyTurn()
    {
        IsMyTurn = false;
    }
    public Image image;


}
