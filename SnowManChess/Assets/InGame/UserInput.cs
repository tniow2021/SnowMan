using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour//입력처리
{
    Map map1;

    public float TouchDecisionTime = 0.7f;
    public float timer = 0;
    public CameraScript cameraScript;
    enum InputStateKind
    {
        StandBy,
        Touch,
        Drag
    }
    InputStateKind inputState = InputStateKind.StandBy;
    HelpCalculation calculation = new HelpCalculation();
    public UserInput(Map _map1)
    {
        map1 = _map1;
        cameraScript = GameObject.Find("Main Camera").GetComponent<CameraScript>();
    }

    //---------------------------Switch 함수(하위 클래스에서 호출당하는 함수)--------------------------
    /*
     * 타일은 고정불변임으로 타일스위치는 좌표값으로 값을 받고
     * 기물은 기물스크립트로 값을 받는다.
     */
    public Vector2Int downTileXY;
    bool TileSwitchChange = true;
    bool IsTouchTile = false;
    public void DownTileSwitch(Vector2Int _tileXY)
    {
        IsTouchTile = true;
        if (_tileXY != downTileXY) TileSwitchChange = true;
        else TileSwitchChange = false;
        downTileXY = _tileXY;
    }

    //타일루트는 입력상태가 터치또는 스탠바이상태가 되었을때 싹 지운다.
    //7월 27일 오늘은 여기까지...............................
    public List<Vector2Int> TileRoute = new List<Vector2Int>();
    public void RouteTileSwitch(Vector2Int TileXY)
    {
        //중복되지않은 타일좌표경로만 루트리스트에 더한다.
        if(!Vector2Int.Equals(TileRoute[TileRoute.Count], TileXY))
        {
            TileRoute.Add(TileXY);
        }
    }

    public PieceScript pieceScript;
    public void DownPieceSwitch(PieceScript _pieceScript)
    {
        print(_pieceScript.gameObject);
        pieceScript = _pieceScript;
    }
    //--------------------------------------일반함수-----------------------------------

    public void AllInput()
    {
        TouchInput();
    }
    void TileTouch(Vector2Int coordinate)
    {
       if(IsTouchTile is true)//드래그일땐 하지않는다
        {
            map1.List2TileScript[coordinate.x][coordinate.y].TileTouch();
        }
    }
    void TileDrag(Vector2Int coordinate)
    {
        IsTouchTile = false;
        map1.List2TileScript[coordinate.x][coordinate.y].TileDrag();
    }
    


    void TouchInput()
    {
        /*
         * 클릭입력은 나중에 터치입력으로 바꿔야함.
         * 터치가 있으면 시간을 재는데 시간이 흐르는 동안 다른 타일좌표나 맵바깥에 클릭이 발생하면
         * ,break로 입력을 취소할 것.
         *  좌표값은 록과 언록을 걸어서 쓰레드끼리 충돌나지 않게하기
         * 
         *  터치후 0.7초 이내로 moved상태가 되면 화면이동
         *  터치후 0.7초 이내로 ended상태가 되면 기물선택
         *  터치후 0.7초가 지나도 Stationary상태면 기물경로 드래그
         *  
         *  더블터치는 어떨까.
         */
        if (MainScript.MobilMode == true)
        {

            List<Touch> touchList = new List<Touch>();
            for (int i = 0; i < Input.touchCount; i++)
            {
                touchList.Add(Input.GetTouch(i));
            }

            if (touchList.Count == 0)//모든 걸 초기화
            {
                inputState = InputStateKind.StandBy;
                //초기화
                cameraScript.CameraMoveAble = true;
                timer = 0;
                calculation.PlusPlusVector2(true);//리셋
            }
            else if (touchList.Count == 1)
            {
                /*손가락을 뗀후. 손가락이 닿아져있을 때에 이동한 움직임의 거리가
                * 50픽셀이하면 터치,(카메라 멈춤)
                * 0.7초 안에 50픽셀이상이면 화면이동(카메라 이동 허용)
                * 0.7초 가만히 있다가 50픽셀이상 움직이면 경로지정(카메라 멈춤)
                */
                timer += Time.deltaTime;
                TouchPhase state = touchList[0].phase;
                //move에서 판단하되 터치가 사라지거나 2개가 되면 모든 판단을 초기화
                if (state == TouchPhase.Moved || state == TouchPhase.Stationary)
                {
                    //이동거리 합산
                    
                    calculation.PlusPlusVector2(touchList[0].deltaPosition);

                    cameraScript.CameraMoveAble = false;

                   
                    if (calculation.PlusPlusVector2(false) < 50)
                    {
                        if (timer > TouchDecisionTime)
                        {
                            //드래그
                            inputState = InputStateKind.Drag;
                        }
                    }
                    if (inputState != InputStateKind.Drag)
                    {
                        if (calculation.PlusPlusVector2(false) > 50)
                        {

                            //화면 이동중인 상태
                            inputState = InputStateKind.StandBy;
                        }
                    }


                }
                else if (touchList[0].phase == TouchPhase.Ended)
                {
                    if (inputState != InputStateKind.Drag)
                    {
                        if (timer < TouchDecisionTime)//0.7초이상 클릭하면 무조건 드래그로 넘어간다.
                        {
                            if (calculation.PlusPlusVector2(false) < 50)//터치
                            {
                                inputState = InputStateKind.Touch;

                                //초기화
                                calculation.PlusPlusVector2(true);
                                timer = 0;
                            }
                        }
                    }
                }
            }
            else if (touchList.Count == 2)
            {
                cameraScript.CameraMoveAble = true;
            }
            else if (touchList.Count > 2)
            {
                cameraScript.CameraMoveAble = true;
            }

            //inputstate에 따른 처리
            if (inputState == InputStateKind.Touch)
            {
                //터치를 하는 동안에는 카메라를 멈춘다.
                cameraScript.CameraMoveAble = false;
                TileRoute.Clear();
                TileTouch(downTileXY);
            }
            else if (inputState == InputStateKind.Drag)
            {
                //드래그를 하는 동안에는 카메라를 멈춘다.
                cameraScript.CameraMoveAble = false;
                TileDrag(downTileXY);

            }
            else if (inputState == InputStateKind.StandBy)
            {
                //드래그도 터치도하지않는 대기상태라면 카메라가 움직일 수 있게 풀어준다.
                cameraScript.CameraMoveAble = true;
                TileRoute.Clear();
                
            }
        }
        else
        {

        }
        


    }
}
