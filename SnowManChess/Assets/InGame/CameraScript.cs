using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HelpCalculation//계산 보조 도구들 모임
{
    //호출할때마다의 숫자 변동값을 반환하는 친구들 모임
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
            return 0;//첫 반환에는 움직이지않음.
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
        xGap *= xGap;//제곱;
        yGap *= yGap;
        float Gap = Mathf.Sqrt(xGap + yGap);
        return Gap;
    }

    //이 함수는 계속 값을 더하다가 
    //그 동안의 값을 카운트값으로 나눠서 평균치를 반환한다.
    Vector2 RememberValue2 = new Vector2(0, 0);
    float Count2 = 0;
    public void AccruePlusVector2(Vector2 value)//더하는 쪽
    {
            RememberValue2 += value;
            Count2++;
    }
    public Vector2 AccruePlusVector2()//반환하는 쪽
    {
        if (Count2 == 0) return new Vector2(0, 0);//0으로 나눌 순 없음으로 
        Vector2 returnValue = RememberValue2 / Count2;
        RememberValue2=new Vector2(0,0);
        Count2 = 0;
        return returnValue;

    }



}
public class CameraScript : MonoBehaviour
{

    public float 카메라최대크기;
    public float 카메라최소크기;

    public float 좌측이동한계;
    public float 우측이동한계;
    public float 위이동한계;
    public float 아래이동한계;

    bool temp2 = false;
    Vector3 temp3;
    void Start()
    {
        
        if(MainScript.MobilMode==true)
        {
            
        }
        else
        {
            //카메라종횡무브
            temp3 = Input.mousePosition;
        }
       
   
    }

    public float movespeed;//얘는 설정으로 넣는다.
    public Camera mycamera;

    public float ZoomInMovementCoefficient;
    public float ZoomInSpeed;//나중에 설정창에 넣을 것.
    public float ZoomInBasicSpeed;


    float ZoomInMovement(float speed,float basicSpeed)//mycamera.orthographicSize가 작을 수록 작은 값을 반환
    {
        //print(mycamera.orthographicSize * ZoomInMovementCoefficient * speed + basicSpeed);
        return mycamera.orthographicSize * ZoomInMovementCoefficient * speed + basicSpeed;
    }
    HelpCalculation calculation = new HelpCalculation();

    public float PlusMinusXY;
    Vector3 프레임당_차 = new Vector3();
    void Update()//판이 가까워질 수록 줌인속도가 빨라지는 현상을 해결해야한다.
    {
        Vector2 이동벡터 = new Vector2();
        
        float ZoomInDelta=0;


        if (MainScript.MobilMode == true)
        {
    
            
            //카메라 종횡무브
            if(Input.touchCount==0)
            {
                Vector2 관성자 = calculation.AccruePlusVector2();//평균치구하기.
                if(관성자!=new Vector2(0,0))print(관성자);
                //만약 화면을 움직이다가 마지막에 멈췄다면 관성도 없어야 함으로...
                if (프레임당_차.x == 0) 관성자.x = 0;
                if (프레임당_차.y == 0) 관성자.y = 0;

                //관성을 프레임당 차에 옮겨준다
                //한번 반환된 관성자는 이후 쭉 0임으로 0은 제외해준다
                if (관성자!=new Vector2(0,0))프레임당_차 = 관성자;
                if (프레임당_차.x>0)
                {
                    프레임당_차.x -= PlusMinusXY;
                    if (프레임당_차.x - PlusMinusXY < 0) 프레임당_차.x = 0;
                }
                else if(프레임당_차.x < 0)
                {
                    프레임당_차.x += PlusMinusXY;
                    if (프레임당_차.x > 0) 프레임당_차.x = 0;
                }
                if (프레임당_차.y > 0)
                {
                    프레임당_차.y -= PlusMinusXY;
                    if (프레임당_차.y - PlusMinusXY < 0) 프레임당_차.y = 0;
                }
                else if (프레임당_차.y < 0)
                {
                    프레임당_차.y += PlusMinusXY;
                    if (프레임당_차.y > 0) 프레임당_차.y = 0;
                }
            }
            else if(Input.touchCount==1)
            {
                //픽셀 차이값*(mycamera.orthographicSize/화면크기값)=적정 감도 이후 *감도
                Touch touch = Input.GetTouch(0);
                if(touch.phase==TouchPhase.Moved)
                {
                    프레임당_차 = touch.deltaPosition;
                    프레임당_차.x *= -1f;
                    프레임당_차.y *= -1f;
                    프레임당_차.z = 0;
                    //관성을 위한 평균치 구하기
                    calculation.AccruePlusVector2(프레임당_차);
                }
               
            }
            else if(Input.touchCount>=2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                ZoomInDelta = calculation.RememberFloatDelta(
                    calculation.Vector2Distance(touch1.position, touch2.position));
                ZoomInDelta *= -1;//해보니 반대방향임
                ZoomInDelta *= ZoomInMovement(ZoomInSpeed,ZoomInBasicSpeed);
                
            }
            if(!(Input.touchCount >= 2)) calculation.RememberFloatDelta(0, true);//0은 상관없고 두번째값이 초기화 인수;
        }
        else
        {
            //리모트를 연결하면 겟엑시스가 안먹힌다.
            ZoomInDelta = -Input.GetAxis("Mouse ScrollWheel")
                *ZoomInMovement(120,30);

            //카메라 종횡무브
            if (Input.GetMouseButtonDown(2)) temp2 = true;//(2)는 스크롤 버튼 클릭인 경우다.
            if (Input.GetMouseButtonUp(2)) temp2 = false;
            if (temp2 == true)
            {

                프레임당_차 = temp3 - Input.mousePosition;

            }
            //카메라종횡무브
            temp3 = Input.mousePosition;
        }
     
        //줌인
        if (ZoomInDelta >= 0 && mycamera.orthographicSize <= 카메라최대크기)
        {
            mycamera.orthographicSize += ZoomInDelta * Time.deltaTime;
            if (mycamera.orthographicSize > 카메라최대크기) mycamera.orthographicSize = 카메라최대크기;
        }
        else if (ZoomInDelta < 0 && mycamera.orthographicSize >= 카메라최소크기)
        {
            mycamera.orthographicSize += ZoomInDelta * Time.deltaTime;
            if (mycamera.orthographicSize < 카메라최소크기)
            {
               
                mycamera.orthographicSize = 카메라최소크기;
            }
        }
            
        


        //카메라이동
        이동벡터.x = (프레임당_차.x / mycamera.pixelHeight) * mycamera.orthographicSize * 2 * movespeed;
        이동벡터.y = (프레임당_차.y / mycamera.pixelHeight) * mycamera.orthographicSize * 2 * movespeed;

        //카메라 이동공간을 정의
        if (transform.localPosition.x < 좌측이동한계 && 이동벡터.x < 0) 이동벡터.x = 0;
        if (transform.localPosition.x > 우측이동한계 && 이동벡터.x > 0) 이동벡터.x = 0;
        if (transform.localPosition.y > 위이동한계 && 이동벡터.y > 0) 이동벡터.y = 0;
        if (transform.localPosition.y < 아래이동한계 && 이동벡터.y < 0) 이동벡터.y = 0;

   
        mycamera.transform.Translate(이동벡터);
        



    }
    
}

//Touch touch = Input.GetTouch(0);
//switch (touch.phase)
//{
//    case (TouchPhase.Began):
//        print("began:시작");
//        print($"터치좌표:{touch.position}");
//        break;
//    case (TouchPhase.Moved):
//        print("moved:움직이는 중");
//        break;
//    case (TouchPhase.Ended):
//        print("Ended:손가락 뗌");
//        break;
//    case (TouchPhase.Stationary):
//        print("Stationary:손가락 움직이지않움");
//        break;
//    case (TouchPhase.Canceled):
//        print("cenceled:터치중단");
//        break;
//}