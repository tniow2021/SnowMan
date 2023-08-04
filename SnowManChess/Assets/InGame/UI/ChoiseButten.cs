using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public enum 하
{
    기물,
    건물,
    아이템,
    타일
}
public class ChoiseButten : MonoBehaviour
{
    public TurnConnect turnConnect;
    public BK bkk;
    public PK pkk;
    public TK tkk;

    public 하 이런;


    public bool IsMyTurn = false;
    public void Down()
    {
        print($"{IsMyTurn},,{이런},,{pkk}");
        if (IsMyTurn is true)
        {
            if (이런 == 하.건물)
            {
                GameScript.instance.KindReceived(bkk,이런);
            }
            if (이런 == 하.기물)
            {
                GameScript.instance.KindReceived(pkk, 이런);
            }
            if (이런 == 하.타일)
            {
                GameScript.instance.KindReceived(tkk, 이런);
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
