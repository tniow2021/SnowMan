using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public partial class UserInput : MonoBehaviour//입력처리
{

    //-----------------------------------------------------------------------------
    public static UserInput instance;

    //---------------------------Switch 함수(하위 클래스에서 호출당하는 함수)--------------------------
    Vector2Int EnterTileXY=new Vector2Int();
    public void EnterTileSwitch(Vector2Int _EnterTileXY)
    {
        if (_EnterTileXY != EnterTileXY)
        {
            EnterTileXY = _EnterTileXY;
        }
    }

    //--------------------------------------일반함수-----------------------------------

 
 
    public float timer = 0;
    HelpCalculation calculation = new HelpCalculation();
    public float TouchDecisionTime = 0.7f;
    public int StateCherk()
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
        if (Input.touchCount == 0)//모든 걸 초기화
        {
            //초기화
            timer = 0;
            calculation.PlusPlusVector2(true);//리셋

            return InputStateKind.StandBy;
        }
        else if (Input.touchCount == 1)
        {
            /*손가락을 뗀후. 손가락이 닿아져있을 때에 이동한 움직임의 거리가
            * 50픽셀이하면 터치,(카메라 멈춤)
            * 0.7초 안에 50픽셀이상이면 화면이동(카메라 이동 허용)
            * 0.7초 가만히 있다가 50픽셀이상 움직이면 경로지정(카메라 멈춤)
            */
            timer += Time.deltaTime;
            TouchPhase state = Input.GetTouch(0).phase;
            //move에서 판단하되 터치가 사라지거나 2개가 되면 모든 판단을 초기화
            if (state == TouchPhase.Moved || state == TouchPhase.Stationary)
            {
                //이동거리 합산
                calculation.PlusPlusVector2(Input.GetTouch(0).deltaPosition);
                if (calculation.PlusPlusVector2(false) < 50)
                {
                    if (timer > TouchDecisionTime)
                    {
                        //드래그
                        return InputStateKind.LongTouch;
                    }
                }
                else
                {
                    //화면 이동중인 상태
                    return InputStateKind.StandBy;
                }
            }
            else if (state == TouchPhase.Ended)
            {
                if (timer <= TouchDecisionTime)//0.7초이상 클릭하면 무조건 드래그로 넘어간다.
                {
                    if (calculation.PlusPlusVector2(false) < 50)//터치
                    {
                        //초기화
                        calculation.PlusPlusVector2(true);
                        timer = 0;
                        return InputStateKind.Touch;
                    }
                }
                else
                {
                    return InputStateKind.Ended;
                }
            }
        }
        return InputStateKind.StandBy;
    }
}
