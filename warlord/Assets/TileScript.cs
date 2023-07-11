using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    
    MainScript mainsc; 
    public int 자기위치i;
    public int 자기위치j;
    void Start()
    {
       mainsc = GameObject.Find("MainObject").GetComponent<MainScript>();
    }
    private void OnMouseEnter()
    {
            mainsc.위치.x = 자기위치i;
            mainsc.위치.y = 자기위치j;
    }
}
