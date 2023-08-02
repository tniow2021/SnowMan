using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public partial class UserInput : MonoBehaviour//�Է�ó��
{

    //-----------------------------------------------------------------------------
    public static UserInput instance;

    //---------------------------Switch �Լ�(���� Ŭ�������� ȣ����ϴ� �Լ�)--------------------------
    Vector2Int EnterTileXY=new Vector2Int();
    public void EnterTileSwitch(Vector2Int _EnterTileXY)
    {
        if (_EnterTileXY != EnterTileXY)
        {
            EnterTileXY = _EnterTileXY;
        }
    }

    //--------------------------------------�Ϲ��Լ�-----------------------------------

 
 
    public float timer = 0;
    HelpCalculation calculation = new HelpCalculation();
    public float TouchDecisionTime = 0.7f;
    public int StateCherk()
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
        if (Input.touchCount == 0)//��� �� �ʱ�ȭ
        {
            //�ʱ�ȭ
            timer = 0;
            calculation.PlusPlusVector2(true);//����

            return InputStateKind.StandBy;
        }
        else if (Input.touchCount == 1)
        {
            /*�հ����� ����. �հ����� ��������� ���� �̵��� �������� �Ÿ���
            * 50�ȼ����ϸ� ��ġ,(ī�޶� ����)
            * 0.7�� �ȿ� 50�ȼ��̻��̸� ȭ���̵�(ī�޶� �̵� ���)
            * 0.7�� ������ �ִٰ� 50�ȼ��̻� �����̸� �������(ī�޶� ����)
            */
            timer += Time.deltaTime;
            TouchPhase state = Input.GetTouch(0).phase;
            //move���� �Ǵ��ϵ� ��ġ�� ������ų� 2���� �Ǹ� ��� �Ǵ��� �ʱ�ȭ
            if (state == TouchPhase.Moved || state == TouchPhase.Stationary)
            {
                //�̵��Ÿ� �ջ�
                calculation.PlusPlusVector2(Input.GetTouch(0).deltaPosition);
                if (calculation.PlusPlusVector2(false) < 50)
                {
                    if (timer > TouchDecisionTime)
                    {
                        //�巡��
                        return InputStateKind.LongTouch;
                    }
                }
                else
                {
                    //ȭ�� �̵����� ����
                    return InputStateKind.StandBy;
                }
            }
            else if (state == TouchPhase.Ended)
            {
                if (timer <= TouchDecisionTime)//0.7���̻� Ŭ���ϸ� ������ �巡�׷� �Ѿ��.
                {
                    if (calculation.PlusPlusVector2(false) < 50)//��ġ
                    {
                        //�ʱ�ȭ
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
