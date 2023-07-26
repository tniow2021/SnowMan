using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class TileScript : MonoBehaviour
{
    public GameObject Piece;
    //���⼭ LocalPositionOfPieceOntile�� Ÿ�� �Ʒ� ������ġ�� ���߿� ������ ��������
    public static Vector3 LocalPositionOfPieceOntile=new Vector3(0,0,0);
    //+ �����۽�ũ��Ʈ�� ���Ŀ� �߰�
    public void PutPiece(GameObject PieceValue)
    {
        //�⹰�� Ÿ������ �ø���.
        Piece = PieceValue;
        Piece.transform.parent = transform;
        //���⼭ LocalPositionOfPieceOntile�� Ÿ�� �Ʒ� ������ġ�� ���߿� ������ ��������.
        Piece.transform.localPosition = LocalPositionOfPieceOntile;
        //��ǥ������ �־��ش�,
        Piece.GetComponent<PieceScript>().Coordinate = coordinate;
    }

}

public partial class TileScript : MonoBehaviour
{

    public GameScript gameScript;
    public Vector2Int coordinate=new Vector2Int(0,0);
    void OnMouseEnter()
    {
        
    }
    private void OnMouseDown()
    {
        if(MainScript.MobilMode==true)
        {
            if(Input.touchCount==1)
            {
                gameScript.XYSwitch(coordinate);
            }
        }
        else
        {
            gameScript.XYSwitch(coordinate);
        }
        


    }
    private void OnMouseDrag()
    {
        
    }
}