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
    public bool �⹰��������Ѱ� = true;
    public bool ���ݴ���ΰ� = false;
    public int HP = 2;
    public void Turn(int turnNumber)
    {

    }
    /// <summary>
    /// �ѹ� ȣ���Ҷ����� HP�� 1�� ��� ���� HP�� ��ȯ�Ѵ�.
    /// </summary>
    /// <returns></returns>
    public int DecreaseHP()
    {
        HP--;
        return HP;
    }
    public void ObjectDestory()
    {
        Destroy(this.gameObject);
    }
}
