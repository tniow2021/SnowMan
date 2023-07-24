using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HelpCalculation//��� ���� ������ ����
{
    //ȣ���Ҷ������� ���� �������� ��ȯ�ϴ� ģ���� ����
    float RememberValue1;
    float Count1 = 0;
    public float RememberFloatDelta(float value,bool reset=false)
    {
        //UnityEngine.Debug.Log($"fggggggggg{value}");

        if (reset == true)
        {
            Count1 = 0;
            RememberValue1 = 0;
            return 0;
        }
        if (Count1 == 0)
        {
            RememberValue1 = value;
            Count1++;
            return 0;//ù ��ȯ���� ������������.
        }
        Count1++;
        float delta= value - RememberValue1;
        RememberValue1 = value;
        return delta;
    }

    public float Vector2Distance(Vector2 A, Vector2 B)
    {
        float xGap = A.x - B.x;
        float yGap = A.y - B.y;
        xGap *= xGap;//����;
        yGap *= yGap;
        float Gap = Mathf.Sqrt(xGap + yGap);
        return Gap;
    }

    //�� �Լ��� ��� ���� ���ϴٰ� 
    //�� ������ ���� ī��Ʈ������ ������ ���ġ�� ��ȯ�Ѵ�.
    Vector2 RememberValue2 = new Vector2(0, 0);
    float Count2 = 0;
    public void AccruePlusVector2(Vector2 value)//���ϴ� ��
    {
            RememberValue2 += value;
            Count2++;
    }
    public Vector2 AccruePlusVector2()//��ȯ�ϴ� ��
    {
        if (Count2 == 0) return new Vector2(0, 0);//0���� ���� �� �������� 
        Vector2 returnValue = RememberValue2 / Count2;
        RememberValue2=new Vector2(0,0);
        Count2 = 0;
        return returnValue;

    }



}
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
    void Update()//���� ������� ���� ���μӵ��� �������� ������ �ذ��ؾ��Ѵ�.
    {
        Vector2 �̵����� = new Vector2();
        
        float ZoomInDelta=0;


        if (MainScript.MobilMode == true)
        {
    
            
            //ī�޶� ��Ⱦ����
            if(Input.touchCount==0)
            {
                Vector2 ������ = calculation.AccruePlusVector2();//���ġ���ϱ�.
                if(������!=new Vector2(0,0))print(������);
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
                    �����Ӵ�_��.y -= PlusMinusXY;
                    if (�����Ӵ�_��.y - PlusMinusXY < 0) �����Ӵ�_��.y = 0;
                }
                else if (�����Ӵ�_��.y < 0)
                {
                    �����Ӵ�_��.y += PlusMinusXY;
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