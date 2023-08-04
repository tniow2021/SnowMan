using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnConnect : MonoBehaviour
{
    public User user;
    public TurnButten turnButten;
    public List<ChoiseButten> choiseButtens;
    public TurnManager turnManager;

    public bool IsMyTurn = false;
    
    public void NowMyTurn()
    {
        IsMyTurn = true;
        turnButten.NowMyturn();
        foreach(var a in choiseButtens)
        {
            a.NowMyTurn();
        }
    }
    public void NowNotMyTurn()
    {
        IsMyTurn = false;
        turnButten.NowNotMyturn();
        foreach (var a in choiseButtens)
        {
            a.NowNotMyTurn();
        }
    }
    
}
