using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PieceScript : MonoBehaviour //�⹰����
{
    public Area area;
    public PK kind;
    public User user;
    public Vector2 additionalLocalPositon;
    
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
    public void Turn(int turnNumber)
    {

    }
    public int AttackPower;
    public int AttackDistance;
    public int HP;
    public int speed;

    //�̵�Ư������
    public List<Vector2Int> AbleRoute= new List<Vector2Int>();
    /* 
     * ������ ����� ����ǥ�ø� �س��´�
     * 0,0�̶� ġ�� �������� ���ư���.
     * 
     * ����:���Ʒ��¿�� 2ĭ�� ���� �̵����ɰ��
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
public partial class PieceScript//������Ʈ����
{
    public void ObjectDestory()
    {
        Destroy(this.gameObject);
    }
    public void HardMove(Vector2Int XY)
    {
        transform.localPosition = new Vector3(XY.x, XY.y, 0);
    }
}



