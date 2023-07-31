using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public partial class TileScript //�������� ����
{
    public TK kind;
    /*
     * ���� �⹰�� �����Ҷ� ������.
     * ���ӷ�(������� �ƴ϶� ���ϱ� ��������.)
     * �����״��� ����
     * �ǹ�(�庮)�Ǽ����� ����
     * (���ٴ��� ���)
     * ���ٴ��� �Ǳ������ �ð�(�ٵ� �̰� �������� ó���� ���װ���)
     * 
     * ������-���ٴ�-���ٴ��� �ϳ��� ����������Ʈ�ȿ��� �ִϸ��̼����� ó���Ѵ�
     * ��ġ��� �Լ�()
     * 
     * 
     * 
     * ����ִ� ȣ��-����ִ� ȣ�� �� �ϳ��� ����������Ʈ���� �ִϸ��̼����� ó���Ѵ�.
     * �غ�()-> �Ӽ����� ��ȭ 
     * 
     * 
     * ���迭
     * ������-���ٴ�-���ٴ�
     * 
     * ȣ���迭
     * ����ִ� ȣ��-���� ȣ��
     * 
     * 
     */
    public int warmingDamage;//�³�ȭ������
    public int PlusSpeed;//���Ӱ�
    public bool warmingAble = false;//�³�ȭ����
    public bool buildAble = false;//�庮�� �ǹ� �Ǽ����� ����
    public bool EnterAble = true;//�⹰ ���԰��� ����
    public bool IsPieceDie = false;//�⹰�� ���ÿ� �ٷ� �״��� ����(ȣ������ ��)

    public int SnowAmount = 0;//
}
public partial class TileScript : MonoBehaviour//�⹰����
{


}
public partial class TileScript //�ܺο� ȣ����ϴ� �� ����
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
    public void TileCandidate()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
    }
    public void BetileWilte()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

}

public partial class TileScript //�ִϸ����Ͱ��� 
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
        Destroy(this.gameObject);
    }
}