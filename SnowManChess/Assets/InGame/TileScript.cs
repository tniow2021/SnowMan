using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public partial class TileScript //지형정의 관련
{
    public TK kind;
    public Area area;
    public Vector2 additionalLocalPositon;

    public void Turn(int turnNumber)
    {

    }
    /*
     * 위에 기물이 존재할때 데미지.
     * 가속력(배수곱이 아니라 더하기 연산으로.)
     * 빠져죽느냐 여부
     * 건물(장벽)건설가능 여부
     * (눈바닥의 경우)
     * 눈바닥이 되기까지의 시간(근데 이건 서버에서 처리할 사항같다)
     * 
     * 눈더미-눈바닥-땅바닥은 하나의 지형오브젝트안에서 애니메이션으로 처리한다
     * 눈치우기 함수()
     * 
     * 
     * 
     * 얼어있는 호수-녹아있는 호수 는 하나의 지형오브젝트에서 애니메이션으로 처리한다.
     * 해빙()-> 속성값을 변화 
     * 
     * 
     * 눈계열
     * 눈더미-눈바닥-땅바닥
     * 
     * 호수계열
     * 얼어있는 호수-녹은 호수
     * 
     * 
     */
    public int warmingDamage;//온난화데미지
    public int PlusSpeed;//가속값
    public bool warmingAble = false;//온난화영향
    public bool buildAble = false;//장벽등 건물 건설가능 여부
    public bool EnterAble = true;//기물 진입가능 여부
    public bool IsPieceDie = false;//기물이 들어갈시에 바로 죽는지 여부(호수같은 거)

    public int SnowAmount = 0;//
}
public partial class TileScript : MonoBehaviour//기물관련
{


}
public partial class TileScript //외부에 호출당하는 거 관련
{
    float timer = 0;
    public void TileTouch()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }
    public void TileDrag()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
    }
    public void Tiletest()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
    }
    public void TileCandidate()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
    }
    public void ColorBule()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(0x9E,0xA7, 0xFF,255);
    }
    public void BetileWilte()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

}

public partial class TileScript //애니메이터관련 
{
    public Animator animator;
}
public partial class TileScript
{
    public Map map1;
    public Vector2Int coordinate;
    public bool BeMouseOnTile;
    void OnMouseEnter()
    {
        if (Input.touchCount == 1)
        {
            BeMouseOnTile = true;
            UserInput.instance.EnterTileSwitch(coordinate);
        }
    }

    void OnMouseExit()
    {
        BeMouseOnTile = false;
    }
    
    public void ObjectDestory()
    {
        area.tile = null;
        Destroy(this.gameObject);
    }
}