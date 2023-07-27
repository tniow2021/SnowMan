using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public partial class TileScript //�������� ����
{
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
    //public GameObject Piece=null;
    ////���⼭ LocalPositionOfPieceOntile�� Ÿ�� �Ʒ� ������ġ�� ���߿� ������ ��������
    //public static Vector3 LocalPositionOfPieceOntile=new Vector3(0,0,0);
    ////+ �����۽�ũ��Ʈ�� ���Ŀ� �߰�
    //public bool PutPiece(GameObject PieceValue)
    //{
    //    //�⹰�� Ÿ������ �ø���.
    //    if(Piece is null)
    //    {
    //        Piece = PieceValue;
    //        Piece.transform.parent = transform;
    //        //���⼭ LocalPositionOfPieceOntile�� Ÿ�� �Ʒ� ������ġ�� ���߿� ������ ��������.
    //        Piece.transform.localPosition = LocalPositionOfPieceOntile;
    //        //��ǥ������ �־��ش�,
    //        Piece.GetComponent<PieceScript>().Coordinate = coordinate;
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
    //public PieceScript TakeOffPiece()
    //{
    //    Piece.transform.parent = null;
    //    PieceScript ReturnPiece = Piece.GetComponent<PieceScript>();
    //    Piece = null;
    //    return ReturnPiece;
    //}
    //public void RemovePiece()
    //{
    //    if(Piece is not null)
    //    {
    //        Piece.GetComponent<PieceScript>().ObjectDestory();
    //    }
    //    Piece = null;
    //}

}
public partial class TileScript //�ܺο� ȣ����ϴ� �� ����
{
    float timer = 0;
    public void TileTouch()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }
    public void TileDrag()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
    }
}

public partial class TileScript //�ִϸ����Ͱ��� 
{
    public Animator animator;
}
public partial class TileScript : MonoBehaviour
{

    public Map map1;
    public Vector2Int coordinate;

    public bool IsMouseOnTile = false;
    void OnMouseEnter()
    {
        IsMouseOnTile = true;
    }
    private void OnMouseUp()
    {
        IsMouseOnTile = false;
    }
    private void OnMouseExit()
    {
        IsMouseOnTile = false;
    }
    private void OnMouseDown()
    {
        if(MainScript.MobilMode==true)
        {
            if(Input.touchCount==1)
            {
                map1.userInput.DownTileSwitch(coordinate);
            }
        }
        else
        {
            map1.userInput.DownTileSwitch(coordinate);
        }
    }
    private void OnMouseDrag()
    {
        
    }
}//��ġ����