using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

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
        
        print(MainScript.MobilMode);
        

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
    public float ZoomInSpeed;//��� ���߿� ��������ĭ���� ���������Է°����ϰ��Ѵ�.
    public float ZoomInBasicSpeed;
    float ZoomInMovement()
    {
        print(mycamera.orthographicSize * ZoomInMovementCoefficient * ZoomInSpeed + ZoomInBasicSpeed);
        return mycamera.orthographicSize * ZoomInMovementCoefficient * ZoomInSpeed + ZoomInBasicSpeed;
    }
    void Update()//���� ������� ���� ���μӵ��� �������� ������ �ذ��ؾ��Ѵ�.
    {
        Vector3 �����Ӵ�_��=new Vector3();
        Vector2 �̵�����=new Vector2();


        if (MainScript.MobilMode == true)
        {
    
            //ī�޶� ��Ⱦ����
            if(Input.touchCount>0)
            {
                //�ȼ� ���̰�*(mycamera.orthographicSize/ȭ��ũ�Ⱚ)=���� ���� ���� *����

                Touch touch = Input.GetTouch(0);
                if(touch.phase==TouchPhase.Moved)
                {
                    print($"��ũ��: ����:{Screen.width},����:{Screen.height},,{mycamera.aspect}");
                    �����Ӵ�_�� = touch.deltaPosition;
                    �����Ӵ�_��.x *= -1f;
                    �����Ӵ�_��.y *= -1f;
                    �����Ӵ�_��.z = 0;
                    
                }
            }
        }
        else
        {
            float temp = -Input.GetAxis("Mouse ScrollWheel") * ZoomInMovement();
            print(temp);
            if (temp >= 0 && mycamera.orthographicSize < ī�޶��ִ�ũ��)
                mycamera.orthographicSize += temp * Time.deltaTime;
            else if (temp < 0 && mycamera.orthographicSize > ī�޶��ּ�ũ��)
                mycamera.orthographicSize += temp * Time.deltaTime;

            if (mycamera.orthographicSize < ī�޶��ּ�ũ��) mycamera.orthographicSize = ī�޶��ּ�ũ��;



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