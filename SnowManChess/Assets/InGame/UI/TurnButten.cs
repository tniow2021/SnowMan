using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TurnButten : MonoBehaviour
{
    public TurnConnect turnConnect;
    public void Turnning()
    {
        GameScript.instance.turnManager.TurnButtenEvent(turnConnect.user);
     
    }
    public bool IsMyTurn = false;
    public void NowMyturn()
    {
        IsMyTurn = true;
        buttenImage.color = Color.red;
    }
    public void NowNotMyturn()
    {
        IsMyTurn = false;
        buttenImage.color = Color.white;
    }
    public Image buttenImage;
}
