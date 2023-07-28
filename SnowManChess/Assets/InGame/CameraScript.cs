using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/*
 * �ذ��ؾ��� ��: ȭ���̵� ������ �߰��� x�Ǵ� y�������� ���ڱ� ���ߴ� ��, ��� �������ȼ����� ���ġ�� ���ѰŶ� ������ ���ڿ������� ��
 */
public class CameraScript : MonoBehaviour
{

    public float ī�޶��ִ�ũ��;
    public float ī�޶��ּ�ũ��;

    public float �����̵��Ѱ�;
    public float �����̵��Ѱ�;
    public float ���̵��Ѱ�;
    public float �Ʒ��̵��Ѱ�;

    bool temp2 = false;
    Vector3 temp3;
    void Start()
    {
        
        if(MainScript.MobilMode==true)
        {
            
        }
        else
        {
            //ī�޶���Ⱦ����
            temp3 = Input.mousePosition;
        }
       
   
    }

    public float movespeed;//��� �������� �ִ´�.
    public Camera mycamera;

    public float ZoomInMovementCoefficient;
    public float ZoomInSpeed;//���߿� ����â�� ���� ��.
    public float ZoomInBasicSpeed;


    float ZoomInMovement(float speed,float basicSpeed)//mycamera.orthographicSize�� ���� ���� ���� ���� ��ȯ
    {
        //print(mycamera.orthographicSize * ZoomInMovementCoefficient * speed + basicSpeed);
        return mycamera.orthographicSize * ZoomInMovementCoefficient * speed + basicSpeed;
    }
    HelpCalculation calculation = new HelpCalculation();

    public float PlusMinusXY;
    Vector3 �����Ӵ�_�� = new Vector3();

    public bool CameraMoveAble = true;
    void Update()
    {
        //�ٸ� ��ũ��Ʈ���� CameraMoveAble�� false�� �ٲٸ� ī�޶� �������� �����.
        if (!CameraMoveAble)
        {
            calculation.AccruePlusVector2();//����
            calculation.RememberFloatDelta(0, true);

            return;
        }

        Vector2 �̵����� = new Vector2();
        
        float ZoomInDelta=0;


        if (MainScript.MobilMode == true)
        {
    
            
            //ī�޶� ��Ⱦ����
            if(Input.touchCount==0)
            {
                Vector2 ������ = calculation.AccruePlusVector2();//���ġ���ϱ�.
                //���� ȭ���� �����̴ٰ� �������� ����ٸ� ������ ����� ������...
                if (�����Ӵ�_��.x == 0) ������.x = 0;
                if (�����Ӵ�_��.y == 0) ������.y = 0;

                //������ �����Ӵ� ���� �Ű��ش�
                //�ѹ� ��ȯ�� �����ڴ� ���� �� 0������ 0�� �������ش�
                if (������!=new Vector2(0,0))�����Ӵ�_�� = ������;

                if (�����Ӵ�_��.x>0)
                {
                    �����Ӵ�_��.x -= PlusMinusXY;
                    if (�����Ӵ�_��.x - PlusMinusXY < 0) �����Ӵ�_��.x = 0;
                }
                else if(�����Ӵ�_��.x < 0)
                {
                    �����Ӵ�_��.x += PlusMinusXY;
                    if (�����Ӵ�_��.x > 0) �����Ӵ�_��.x = 0;
                }
                if (�����Ӵ�_��.y > 0)
                {
                    //�� �������� ������ ���ư��� ���� aspect�� ������.
                    �����Ӵ�_��.y -= PlusMinusXY * mycamera.aspect;
                    if (�����Ӵ�_��.y - PlusMinusXY < 0) �����Ӵ�_��.y = 0;
                }
                else if (�����Ӵ�_��.y < 0)
                {
                    �����Ӵ�_��.y += PlusMinusXY * mycamera.aspect;
                    if (�����Ӵ�_��.y > 0) �����Ӵ�_��.y = 0;
                }
            }
            else if(Input.touchCount==1)
            {
                //�ȼ� ���̰�*(mycamera.orthographicSize/ȭ��ũ�Ⱚ)=���� ���� ���� *����
                Touch touch = Input.GetTouch(0);
                if(touch.phase==TouchPhase.Moved)
                {
                    �����Ӵ�_�� = touch.deltaPosition;
                    �����Ӵ�_��.x *= -1f;
                    �����Ӵ�_��.y *= -1f;
                    �����Ӵ�_��.z = 0;
                    //������ ���� ���ġ ���ϱ�
                    calculation.AccruePlusVector2(�����Ӵ�_��);
                }
                else if(touch.phase==TouchPhase.Stationary)
                {
                    �����Ӵ�_��.x = 0;
                    �����Ӵ�_��.y = 0;
                    �����Ӵ�_��.z = 0;
                }
               
            }
            else if(Input.touchCount>=2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                ZoomInDelta = calculation.RememberFloatDelta(
                    calculation.Vector2Distance(touch1.position, touch2.position));
                ZoomInDelta *= -1;//�غ��� �ݴ������
                ZoomInDelta *= ZoomInMovement(ZoomInSpeed,ZoomInBasicSpeed);
                
            }
            if(!(Input.touchCount >= 2)) calculation.RememberFloatDelta(0, true);//0�� ������� �ι�°���� �ʱ�ȭ �μ�;
        }
        else
        {
            //����Ʈ�� �����ϸ� �ٿ��ý��� �ȸ�����.
            ZoomInDelta = -Input.GetAxis("Mouse ScrollWheel")
                *ZoomInMovement(120,30);

            //ī�޶� ��Ⱦ����
            if (Input.GetMouseButtonDown(2)) temp2 = true;//(2)�� ��ũ�� ��ư Ŭ���� ����.
            if (Input.GetMouseButtonUp(2)) temp2 = false;
            if (temp2 == true)
            {

                �����Ӵ�_�� = temp3 - Input.mousePosition;

            }
            //ī�޶���Ⱦ����
            temp3 = Input.mousePosition;
        }
     
        //����
        if (ZoomInDelta >= 0 && mycamera.orthographicSize <= ī�޶��ִ�ũ��)
        {
            mycamera.orthographicSize += ZoomInDelta * Time.deltaTime;
            if (mycamera.orthographicSize > ī�޶��ִ�ũ��) mycamera.orthographicSize = ī�޶��ִ�ũ��;
        }
        else if (ZoomInDelta < 0 && mycamera.orthographicSize >= ī�޶��ּ�ũ��)
        {
            mycamera.orthographicSize += ZoomInDelta * Time.deltaTime;
            if (mycamera.orthographicSize < ī�޶��ּ�ũ��)
            {
               
                mycamera.orthographicSize = ī�޶��ּ�ũ��;
            }
        }
            
        


        //ī�޶��̵�
        �̵�����.x = (�����Ӵ�_��.x / mycamera.pixelHeight) * mycamera.orthographicSize * 2 * movespeed;
        �̵�����.y = (�����Ӵ�_��.y / mycamera.pixelHeight) * mycamera.orthographicSize * 2 * movespeed;

        //ī�޶� �̵������� ����
        if (transform.localPosition.x < �����̵��Ѱ� && �̵�����.x < 0) �̵�����.x = 0;
        if (transform.localPosition.x > �����̵��Ѱ� && �̵�����.x > 0) �̵�����.x = 0;
        if (transform.localPosition.y > ���̵��Ѱ� && �̵�����.y > 0) �̵�����.y = 0;
        if (transform.localPosition.y < �Ʒ��̵��Ѱ� && �̵�����.y < 0) �̵�����.y = 0;

   
        mycamera.transform.Translate(�̵�����);
        



    }
    
}

//Touch touch = Input.GetTouch(0);
//switch (touch.phase)
//{
//    case (TouchPhase.Began):
//        print("began:����");
//        print($"��ġ��ǥ:{touch.position}");
//        break;
//    case (TouchPhase.Moved):
//        print("moved:�����̴� ��");
//        break;
//    case (TouchPhase.Ended):
//        print("Ended:�հ��� ��");
//        break;
//    case (TouchPhase.Stationary):
//        print("Stationary:�հ��� ���������ʿ�");
//        break;
//    case (TouchPhase.Canceled):
//        print("cenceled:��ġ�ߴ�");
//        break;
//}