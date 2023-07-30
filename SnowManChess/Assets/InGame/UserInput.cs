using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UserInput : MonoBehaviour//�Է�ó��
{
    //--------------------------------------�̱���-----------------------------
    static UserInput Instance = new UserInput();
    public static UserInput GetInstance()
    {
        return Instance;
    }

    public float TouchDecisionTime = GameScript.TouchDecisionTime;
    public float timer = 0;
    public CameraScript cameraScript = GameScript.cameraScript;
    //---------------------------Switch �Լ�(���� Ŭ�������� ȣ����ϴ� �Լ�)--------------------------
    public bool IsTouchTile = false;
    public Vector2Int EnterTileXY;
    GameObject tileObj;
    public void EnterTileSwitch(GameObject _tileObj)
    {
        if (_tileObj != tileObj)
        {
            tileObj = _tileObj;
            EnterTileXY = _tileObj.GetComponent<TileScript>().coordinate;
        }
        print(EnterTileXY);
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
    public List<Vector2Int> RouteList = new List<Vector2Int>();


    Command.ThingOntile.Post post = new Command.ThingOntile.Post();
    private void Update()
    {
        InputStateKind InputState2 = TouchInput();
        BeFixedCamera(InputState2);


        if (inputMode == InputMode.Pick)
        {
            if (InputState2 == InputStateKind.Touch)
            {
                
                if (Map.GetInstance().List2TileScript[EnterTileXY.x][EnterTileXY.y].BeMouseOnTile is true)//���콺�� �ش�Ÿ�� ���� ��������.
                {
                    Map.GetInstance().List2TileScript[EnterTileXY.x][EnterTileXY.y].TileTouch();
                }
                

            }
            else if (InputState2 == InputStateKind.Drag)
            {
               
                if (Map.GetInstance().mapArea[EnterTileXY.x][EnterTileXY.y].Piece is not null)//��ġ�� Ÿ�Ͽ� �⹰�� �־�� ��Ʈ���� �����Ѵ�
                {
                    post.FromXY = EnterTileXY;

                    inputMode = InputMode.Route;//��Ʈ������� 
                }
                
            }
            else if(InputState2==InputStateKind.StandBy)
            {
            }
        }
        else if(inputMode== InputMode.Route)
        {
            if (InputState2 == InputStateKind.Drag)
            {


                if (RouteList.Count == 0)
                {

                    Map.GetInstance().List2TileScript[EnterTileXY.x][EnterTileXY.y].TileDrag();
                    
                    //���
                    post.actionKind = Command.ThingOntile.ActionKind.HardMove;
                    post.ToXY = EnterTileXY;
                    post.objectKind = Command.ThingOntile.ObjectKind.Piece;

                    //�������
                    Map.GetInstance().FromInput(post);


                    RouteList.Add(EnterTileXY);
                }
                else if (Vector2Int.Equals(RouteList[RouteList.Count - 1], EnterTileXY) is false)
                {

                    Map.GetInstance().List2TileScript[EnterTileXY.x][EnterTileXY.y].TileDrag();
                    
                    RouteList.Add(EnterTileXY);
                }

            }
            else if(InputState2==InputStateKind.StandBy|| InputState2==InputStateKind.Touch)
            {
                RouteList.Clear();
                inputMode = InputMode.Pick;
            }
        }
    }
    void BeFixedCamera(InputStateKind state)
    {
        if (state == InputStateKind.Touch)
        {

            //��ġ�� �ϴ� ���ȿ��� ī�޶� �����.
            cameraScript.CameraMoveAble = false;

        }
        else if (state == InputStateKind.Drag)
        {
            //�巡�׸� �ϴ� ���ȿ��� ī�޶� �����.
            cameraScript.CameraMoveAble = false;
        }
        else if (state == InputStateKind.StandBy)
        {
            //�巡�׵� ��ġ�������ʴ� �����¶�� ī�޶� ������ �� �ְ� Ǯ���ش�.
            cameraScript.CameraMoveAble = true;

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
