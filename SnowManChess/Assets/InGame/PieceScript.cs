using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PieceScript : MonoBehaviour
{
    public Vector2Int Coordinate=new Vector2Int();

    public float AttackPower;
    public float AttackDistance;
    public float HP;
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
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ObjectDestory()
    {
        Destroy(this);
    }
}
public partial class PieceScript
{

    void Update()
    {

    }
}


