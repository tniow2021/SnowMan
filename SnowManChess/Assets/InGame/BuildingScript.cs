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
    public bool 기물통과가능한가 = true;
    public bool 공격대상인가 = false;
    public int HP = 2;
    public void Turn(int turnNumber)
    {

    }
    /// <summary>
    /// 한번 호출할때마다 HP를 1씩 깎고 남은 HP를 반환한다.
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
