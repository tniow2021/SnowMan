using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceScript : MonoBehaviour //�⹰����
{
    public Map map1;

    public Vector2Int Coordinate=new Vector2Int();

    public int AttackPower;
    public int AttackDistance;
    public int HP;
    public int speed;

    //�̵�Ư������
    public List<Vector2Int> AbleCoordinate = new List<Vector2Int>();
    /*
     * (0,0)�� ���� �⹰����ġ�� ���̶� �� �� 
     * �̵������� ��ǥ
     * ex:
     * new Vector2Int(0,1), //��
     * new Vector2Int(1,1), //�տ��� �������� ��ĭ
     * new Vector2Int(-1,1) //�տ��� ���������� ��ĭ
     */
    public Animator animator;

    public void ObjectDestory()
    {
        Destroy(this);
    }
}



