using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    public BK kind;
    public Map map1;
    public Vector2 additionalLocalPositon;
    public Area area;
    public User user;
    public void Turn(int turnNumber)
    {

    }
    /*
     * ���ڿ��� �ϳ� �Ҹ��ؼ� ����� �����庮
     */
    public void ObjectDestory()
    {
        Destroy(this.gameObject);
    }
}
