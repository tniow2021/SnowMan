using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    
    MainScript mainsc; 
    public int �ڱ���ġi;
    public int �ڱ���ġj;
    void Start()
    {
       mainsc = GameObject.Find("MainObject").GetComponent<MainScript>();
    }
    private void OnMouseEnter()
    {
            mainsc.��ġ.x = �ڱ���ġi;
            mainsc.��ġ.y = �ڱ���ġj;
    }
}
