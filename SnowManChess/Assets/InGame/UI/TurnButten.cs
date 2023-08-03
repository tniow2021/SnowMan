using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnButten : MonoBehaviour
{
    public User user;//게임스크립트한테 받음
    public void Turnning()
    {
        GameScript.instance.userManager.TurnChange(user);
    }
}
