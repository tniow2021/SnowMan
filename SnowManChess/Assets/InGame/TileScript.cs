using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class TileScript : MonoBehaviour
{
    public GameObject Piece;
    //여기서 LocalPositionOfPieceOntile는 타일 아래 로컬위치로 나중에 적당히 조정하자
    public static Vector3 LocalPositionOfPieceOntile=new Vector3(0,0,0);
    //+ 아이템스크립트도 이후에 추가
    public void PutPiece(GameObject PieceValue)
    {
        //기물을 타일위에 올린다.
        Piece = PieceValue;
        Piece.transform.parent = transform;
        //여기서 LocalPositionOfPieceOntile는 타일 아래 로컬위치로 나중에 적당히 조정하자.
        Piece.transform.localPosition = LocalPositionOfPieceOntile;
        //좌표정보를 넣어준다,
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