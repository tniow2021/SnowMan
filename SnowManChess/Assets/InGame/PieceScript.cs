using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PieceScript : MonoBehaviour //�⹰����
{
    public Area area;
    public PK kind;
    public User user;

    public Vector2 additionalLocalPositon;
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
        
        if (kind == PK.Aking||kind==PK.Bking)
        {
            GameScript.instance.DieKingEvent(user);
            area.mapAreas.KingList.Remove(this);
        }
        area.piece = null;
        Destroy(this.gameObject);
    }
    public void HardMove(Vector2Int XY)
    {
        transform.localPosition = new Vector3(XY.x, XY.y, 0);
    }
}



