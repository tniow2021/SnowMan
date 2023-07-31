using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PieceScript : MonoBehaviour //기물정의
{
    public Map map1;

    public PK Kind;

    public Vector2Int Coordinate=new Vector2Int();

    public int AttackPower;
    public int AttackDistance;
    public int HP;
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
    public Animator animator;


}
public partial class PieceScript//오브젝트관련
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



