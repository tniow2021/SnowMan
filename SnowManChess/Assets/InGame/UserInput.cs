using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MoveOrder
{
    public Vector2Int Piece;
    public Vector2Int ToTile;
}
public partial class UserInput
{
    enum PieceChoiseMode
    {
        pickAble,
        PutAble
    }
    PieceChoiseMode PieceMode = PieceChoiseMode.pickAble;
    MoveOrder moveOrder = new MoveOrder();
    public InputMode inputMode = InputMode.Pick;
    private void Update()
    {
        InputStateKind InputState2 = TouchInput();
        BeFixedCamera(InputState2);

        if (inputMode == InputMode.Pick)
        {
            if (InputState2 == InputStateKind.Touch)
            {
                if (Map.insatance.List2TileScript[EnterTileXY.x][EnterTileXY.y].BeMouseOnTile is true)//마우스가 해당타일 위에 있을때만.
                {
                    if (PieceMode is PieceChoiseMode.pickAble)
                    {
                        //시각표시
                        Map.insatance.List2TileScript[EnterTileXY.x][EnterTileXY.y].TileTouch();

                        moveOrder.Piece = EnterTileXY;
                        PieceMode = PieceChoiseMode.PutAble;
                    }
                    else if (PieceMode is PieceChoiseMode.PutAble)
                    {
                        if (!Vector2Int.Equals(moveOrder.Piece, EnterTileXY))
                        {
                            moveOrder.ToTile = EnterTileXY;
                            PieceMode = PieceChoiseMode.pickAble;

                            print($"qqqq{moveOrder.Piece},,{moveOrder.ToTile}");
                        }

                    }
                }


            }
            else if (InputState2 == InputStateKind.Drag)
            {

                if (Map.insatance.mapArea[EnterTileXY.x][EnterTileXY.y].Piece is not null)//터치한 타일에 기물이 있어야 루트모드로 진입한다
                {
                    post.FromXY = EnterTileXY;

                    inputMode = InputMode.Route;//루트모드진입 
                }

            }
            else if (InputState2 == InputStateKind.StandBy)
            {
            }
        }
        else if (inputMode == InputMode.Route)
        {
            if (InputState2 == InputStateKind.Drag)
            {


                if (RouteList.Count == 0)
                {

                    Map.insatance.GetTile(EnterTileXY).TileDrag();

                    //명령
                    post.actionKind = Command.ThingOntile.ActionKind.HardMove;
                    post.ToXY = EnterTileXY;
                    post.objectKind = Command.ThingOntile.ObjectKind.Piece;

                    //명령전송
                    Map.insatance.FromInput(post);


                    RouteList.Add(EnterTileXY);
                }
                else if (Vector2Int.Equals(RouteList[RouteList.Count - 1], EnterTileXY) is false)
                {

                    Map.insatance.GetTile(EnterTileXY).TileDrag();

                    RouteList.Add(EnterTileXY);
                }

            }
            else if (InputState2 == InputStateKind.StandBy || InputState2 == InputStateKind.Touch)
            {
                RouteList.Clear();
                inputMode = InputMode.Pick;
            }
        }
    }
}

public partial class UserInput : MonoBehaviour//입력처리
{

    //-----------------------------------------------------------------------------
    public static UserInput instance;


    //---------------------------Switch 함수(하위 클래스에서 호출당하는 함수)--------------------------
    public bool IsTouchTile = false;
     Vector2Int EnterTileXY=new Vector2Int();
    public void EnterTileSwitch(Vector2Int _EnterTileXY)
    {
        if (_EnterTileXY != EnterTileXY)
        {
            EnterTileXY = _EnterTileXY;
        }
      
        print(EnterTileXY);
    }

    //--------------------------------------일반함수-----------------------------------

    public enum InputMode
    {
        Pick,
        appoint,//이동할장소를 콕집어 지정
        Route
    }
    enum InputStateKind
    {
        StandBy,
        Touch,
        Drag
    }
    public List<Vector2Int> RouteList = new List<Vector2Int>();


    Command.ThingOntile.Post post = new Command.ThingOntile.Post();
    

    void BeFixedCamera(InputStateKind state)
    {
        if (state == InputStateKind.Touch)
        {

            //터치를 하는 동안에는 카메라를 멈춘다.
            cameraScript.CameraMoveAble = false;

        }
        else if (state == InputStateKind.Drag)
        {
            //드래그를 하는 동안에는 카메라를 멈춘다.
            cameraScript.CameraMoveAble = false;
        }
        else if (state == InputStateKind.StandBy)
        {
            //드래그도 터치도하지않는 대기상태라면 카메라가 움직일 수 있게 풀어준다.
            cameraScript.CameraMoveAble = true;

        }
    }

    public float TouchDecisionTime = GameScript.TouchDecisionTime;
    public float timer = 0;
    public CameraScript cameraScript = GameScript.cameraScript;
    HelpCalculation calculation = new HelpCalculation();
    InputStateKind inputState = InputStateKind.StandBy;
    InputStateKind TouchInput()
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
                        if(Map.insatance.mapArea[EnterTileXY.x][EnterTileXY.y].Piece is not null)
                        {
                            inputState = InputStateKind.Drag;
                        }
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
        return inputState;

    }
}
