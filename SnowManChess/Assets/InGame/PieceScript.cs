using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PieceScript : MonoBehaviour //기물정의
{
    public Area area;
    public PK kind;
    public User user;
    public Vector3 addLocalPosition;
    private void Start()
    {
        transform.localScale = ObjectDict.Instance.pieceScale;
    }

    ItemScript item;
    public bool GetItem(out ItemScript _item)
    {
        if(item is not null)
        {
            _item = item;
            return true;
        }
        else
        {
            _item = null;
            return false;
        }
    }
    public void GiveItem(ItemScript _item)
    {
        item = _item;
    }
    int HotCount = 0;
    public int hotDurability = 5;
    public void Turn()
    {
        if(area.tile.IsHot is true)
        {
            HotCount++;
        }
        else
        {
            HotCount = 0;
        }
        if(HotCount>= hotDurability)
        {
            area.mapAreas.Delete(this);
        }
    }
    public int AttackPower;
    public int AttackDistance;
    public int HP;
    public int speed;

    //이동특성정의
    public List<Vector2Int> AbleRoute= new List<Vector2Int>();
    /* 
     * 일종의 경로의 방향표시를 해놓는다
     * 0,0이라 치면 원점으로 돌아간다.
     * 
     * 예시:위아래좌우로 2칸씩 뻗은 이동가능경로
     * 1,0
     * 2,0
     * 0,0
     * -1,0
     * -2,0
     * 0,0
     * 0,1
     * 0,2
     * 0,0
     * 0,-1
     * 0,-2
     * 
     */
    public Animator animator;


}
public partial class PieceScript//오브젝트관련
{
    public void ObjectDestory()
    {
        Destroy(this.gameObject);
    }

}



