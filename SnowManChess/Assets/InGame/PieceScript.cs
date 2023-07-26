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

    //이동특성정의
    public List<Vector2Int> AbleCoordinate = new List<Vector2Int>();
    /*
     * (0,0)을 현재 기물이위치한 곳이라 할 때 
     * 이동가능한 좌표
     * ex:
     * new Vector2Int(0,1), //앞
     * new Vector2Int(1,1), //앞에서 왼쪽으로 한칸
     * new Vector2Int(-1,1) //앞에서 오른쪽으로 한칸
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


