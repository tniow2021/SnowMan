using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    public BK kind;
    public Map map1;
    /*
     * ���ڿ��� �ϳ� �Ҹ��ؼ� ����� �����庮
     */
    public void ObjectDestory()
    {
        Destroy(this.gameObject);
    }
}
