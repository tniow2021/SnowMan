using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public IK kind;
    public Map map1;
    public Vector2 additionalLocalPositon;
    public Area area;

    public bool 건물통과가능하게하는가 = false;
    public bool 타일통과가능하게하는가 = false;
    public void Turn(int turnNumber)
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ObjectDestory()
    {
        Destroy(this.gameObject);
    }
}
