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
                if (Map.insatance.List2TileScript[EnterTileXY.x][EnterTileXY.y].BeMouseOnTile is true)//���콺�� �ش�Ÿ�� ���� ��������.
                {
                    if (PieceMode is PieceChoiseMode.pickAble)
                    {
                        //�ð�ǥ��
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

                if (Map.insatance.mapArea[EnterTileXY.x][EnterTileXY.y].Piece is not null)//��ġ�� Ÿ�Ͽ� �⹰�� �־�� ��Ʈ���� �����Ѵ�
                {
                    post.FromXY = EnterTileXY;

                    inputMode = InputMode.Route;//��Ʈ������� 
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

                    //���
                    post.actionKind = Command.ThingOntile.ActionKind.HardMove;
                    post.ToXY = EnterTileXY;
                    post.objectKind = Command.ThingOntile.ObjectKind.Piece;

                    //�������
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

public partial class UserInput : MonoBehaviour//�Է�ó��
{

    //-----------------------------------------------------------------------------
    public static UserInput instance;


    //---------------------------Switch �Լ�(���� Ŭ�������� ȣ����ϴ� �Լ�)--------------------------
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
    public List<Vector2Int> RouteList = new List<Vector2Int>();


    Command.ThingOntile.Post post = new Command.ThingOntile.Post();
    

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

    public float TouchDecisionTime = GameScript.TouchDecisionTime;
    public float timer = 0;
    public CameraScript cameraScript = GameScript.cameraScript;
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
