using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class TileScript : MonoBehaviour
{
    
    //MainScript mainsc; 
    //public int 자기위치i;
    //public int 자기위치j;
    //void Start()
    //{
    //   mainsc = GameObject.Find("MainObject").GetComponent<MainScript>();
    //}
    //private void OnMouseEnter()
    //{
    //        mainsc.위치.x = 자기위치i;
    //        mainsc.위치.y = 자기위치j;
    //}
}
public partial class TileScript
{
    public GameScript gameScript;
    public Vector2Int coordinate;
    void OnMouseEnter()
    {
        
    }
    private void OnMouseDown()//이것들 다 마우스 가 아니라 터치로 바꿔야함.
    {
        //print(coordinate);
        
    }
    private void OnMouseDrag()
    {
        
    }
}