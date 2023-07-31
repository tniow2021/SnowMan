using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    public BK kind;
    public Map map1;
    /*
     * 눈자원을 하나 소모해서 만드는 얼음장벽
     */
    public void ObjectDestory()
    {
        Destroy(this.gameObject);
    }
}
