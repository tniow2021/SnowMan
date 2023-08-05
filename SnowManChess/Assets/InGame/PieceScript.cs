using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PieceScript : MonoBehaviour //기물정의
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



