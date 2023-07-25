using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class TileScript : MonoBehaviour
{
    public GameScript gameScript;
    public Vector2Int coordinate=new Vector2Int(0,0);
    void OnMouseEnter()
    {
        
    }
    private void OnMouseDown()
    {
        if(MainScript.MobilMode==true)
        {
            if(Input.touchCount==1)
            {
                gameScript.XYSwitch(coordinate);
            }
        }
        else
        {
            gameScript.XYSwitch(coordinate);
        }
        


    }
    private void OnMouseDrag()
    {
        
    }
}