using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public partial class TileScript //�������� ����
{
    public TK kind;
    public Area area;
    public Vector2 additionalLocalPositon;

    public void Turn(int turnNumber)
    {

    }
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
    public bool IsHot = false;//�³�ȭ����
    public bool buildAble = false;//�庮�� �ǹ� �Ǽ����� ����
    public bool EnterAble = true;//�⹰ ���԰��� ����
    public bool IsPieceDie = false;//�⹰�� ���ÿ� �ٷ� �״��� ����(ȣ������ ��)

    public int SnowAmount = 0;//
}
public partial class TileScript : MonoBehaviour//��Ÿ�ϰ���
{


}
public partial class TileScript//ȣ��Ÿ�ϰ���
{
    //public Sprite IceLakeSprite;
    //public Sprite WaterLakeSprite;
    //public void IceLakeToWaterLake()
    //{
    //    GetComponent<SpriteRenderer>().sprite = WaterLakeSprite;
    //    buildAble = false;
    //    EnterAble = false;
    //    IsPieceDie = true;
    //}

}
public partial class TileScript //�ܺο� ȣ����ϴ� �� ����
{
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
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
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