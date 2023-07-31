using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum 하
{
    기물,
    건물,
    타일
}
public class testScript : MonoBehaviour
{
    public 하 이런;
    public BK bkk;
    public PK pkk;
    public TK tkk;

    public static 하 젠장;
    private void OnMouseDown()
    {
        if(이런==하.건물)
        {
            젠장 = 하.건물;
            GameScript.instance.KindReceived(bkk);
        }
        if (이런 == 하.기물)
        {
            젠장 = 하.기물;
            GameScript.instance.KindReceived(pkk);
        }
        if (이런 == 하.타일)
        {
            젠장 = 하.타일;
            GameScript.instance.KindReceived(tkk);
        }

    }
}
