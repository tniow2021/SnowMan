using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour//�Է�ó��
{
    public Map map1;

    public float TouchDecisionTime = GameScript.TouchDecisionTime;
    public float timer = 0;
    public CameraScript cameraScript = GameScript.cameraScript;
    //---------------------------Switch �Լ�(���� Ŭ�������� ȣ����ϴ� �Լ�)--------------------------
    public Vector2Int downTileXY;
    bool TileSwitchChange = true;
    public bool IsTouchTile = false;
    public void DownTileSwitch(Vector2Int _tileXY)
    {
        print(_tileXY);
        if (_tileXY != downTileXY) TileSwitchChange = true;
        else TileSwitchChange = false;
        downTileXY = _tileXY;
        IsTouchTile = true;



    }
    public void DragTileSwitch()
    {
        IsTouchTile = true;
    }

    Vector2Int EnterTileXY;
    public void EnterTileSwitch(Vector2Int _tileXY)
    {
        if (_tileXY != EnterTileXY) EnterTileXY = _tileXY;
    }
    public void ExitTileSwitch(Vector2Int _tileXY)
    {
        //�̹������ Ÿ����ǥ�� ���콺�� ���� Ÿ����ǥ�� ���ٶ�°� Ÿ�ϸʹ��� ��ġ�ߴٴ� ��,
        if (Vector2Int.Equals(_tileXY, downTileXY))
        {
            IsTouchTile = false;
        }
    }

    //Ÿ�Ϸ�Ʈ�� �Է»��°� ��ġ�Ǵ� ���Ĺ��̻��°� �Ǿ����� �� �����.
    public List<Vector2Int> TileRoute = new List<Vector2Int>();
    public void RouteTileSwitch(Vector2Int TileXY)
    {
        //�ߺ��������� Ÿ����ǥ��θ� ��Ʈ����Ʈ�� ���Ѵ�.
        if (!Vector2Int.Equals(TileRoute[TileRoute.Count], TileXY))
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
    //--------------------------------------�Ϲ��Լ�-----------------------------------

    public enum InputMode
    {
        Pick,
        appoint,//�̵�����Ҹ� ������ ����
        Route
    }
    enum InputStateKind
    {
        StandBy,
        Touch,
        Drag
    }
    public InputMode inputMode = InputMode.Pick;
    private void Update()
    {
        InputStateKind InputState2 = TouchInput();
        if (InputState2 == InputStateKind.Touch)
        {
            //��ġ�� �ϴ� ���ȿ��� ī�޶� �����.
            cameraScript.CameraMoveAble = false;
            TileRoute.Clear();
            TileRoute.Add(downTileXY);
        }
        else if (InputState2 == InputStateKind.Drag)
        {
            //�巡�׸� �ϴ� ���ȿ��� ī�޶� �����.
            cameraScript.CameraMoveAble = false;
        }
        else if (InputState2 == InputStateKind.StandBy)
        {
            //�巡�׵� ��ġ�������ʴ� �����¶�� ī�޶� ������ �� �ְ� Ǯ���ش�.
            cameraScript.CameraMoveAble = true;
            TileRoute.Clear();

        }

        if (inputMode == InputMode.Pick)
        {
            if (InputState2 == InputStateKind.Touch)
            {
                if (IsTouchTile is true)
                {
                    map1.List2TileScript[downTileXY.x][downTileXY.y].TileTouch();
                }
            }
            else if (InputState2 == InputStateKind.Drag)
            {
                inputMode = InputMode.Route;//��Ʈ������� 
            }
        }
        else if(inputMode== InputMode.Route)
        {
            if (InputState2 == InputStateKind.Drag)
            {
                map1.List2TileScript[EnterTileXY.x][EnterTileXY.y].TileDrag();
            }
        }
    }
    HelpCalculation calculation = new HelpCalculation();

    InputStateKind inputState = InputStateKind.StandBy;
    InputStateKind TouchInput()
    {
        /*
         * Ŭ���Է��� ���߿� ��ġ�Է����� �ٲ����.
         * ��ġ�� ������ �ð��� ��µ� �ð��� �帣�� ���� �ٸ� Ÿ����ǥ�� �ʹٱ��� Ŭ���� �߻��ϸ�
         * ,break�� �Է��� ����� ��.
         *  ��ǥ���� �ϰ� ����� �ɾ �����峢�� �浹���� �ʰ��ϱ�
         * 
         *  ��ġ�� 0.7�� �̳��� moved���°� �Ǹ� ȭ���̵�
         *  ��ġ�� 0.7�� �̳��� ended���°� �Ǹ� �⹰����
         *  ��ġ�� 0.7�ʰ� ������ Stationary���¸� �⹰��� �巡��
         *  
         *  ������ġ�� ���.
         */


        List<Touch> touchList = new List<Touch>();
        for (int i = 0; i < Input.touchCount; i++)
        {
            touchList.Add(Input.GetTouch(i));
        }
        if (touchList.Count == 0)//��� �� �ʱ�ȭ
        {
            inputState = InputStateKind.StandBy;
            //�ʱ�ȭ
            cameraScript.CameraMoveAble = true;
            timer = 0;
            calculation.PlusPlusVector2(true);//����
        }
        else if (touchList.Count == 1)
        {
            /*�հ����� ����. �հ����� ��������� ���� �̵��� �������� �Ÿ���
            * 50�ȼ����ϸ� ��ġ,(ī�޶� ����)
            * 0.7�� �ȿ� 50�ȼ��̻��̸� ȭ���̵�(ī�޶� �̵� ���)
            * 0.7�� ������ �ִٰ� 50�ȼ��̻� �����̸� �������(ī�޶� ����)
            */
            timer += Time.deltaTime;
            TouchPhase state = touchList[0].phase;
            //move���� �Ǵ��ϵ� ��ġ�� ������ų� 2���� �Ǹ� ��� �Ǵ��� �ʱ�ȭ
            if (state == TouchPhase.Moved || state == TouchPhase.Stationary)
            {
                //�̵��Ÿ� �ջ�

                calculation.PlusPlusVector2(touchList[0].deltaPosition);
                cameraScript.CameraMoveAble = false;
                if (calculation.PlusPlusVector2(false) < 50)
                {
                    if (timer > TouchDecisionTime)
                    {
                        //�巡��
                        inputState = InputStateKind.Drag;
                    }
                }
                if (inputState != InputStateKind.Drag)
                {
                    if (calculation.PlusPlusVector2(false) > 50)
                    {

                        //ȭ�� �̵����� ����
                        inputState = InputStateKind.StandBy;
                    }
                }
            }
            else if (touchList[0].phase == TouchPhase.Ended)
            {
                if (inputState != InputStateKind.Drag)
                {
                    if (timer < TouchDecisionTime)//0.7���̻� Ŭ���ϸ� ������ �巡�׷� �Ѿ��.
                    {
                        if (calculation.PlusPlusVector2(false) < 50)//��ġ
                        {
                            inputState = InputStateKind.Touch;

                            //�ʱ�ȭ
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
