using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

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
        
        print(MainScript.MobilMode);
        

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
    public float ZoomInSpeed;//얘는 나중에 유저설정칸으로 유저직접입력가능하게한다.
    public float ZoomInBasicSpeed;
    float ZoomInMovement()
    {
        print(mycamera.orthographicSize * ZoomInMovementCoefficient * ZoomInSpeed + ZoomInBasicSpeed);
        return mycamera.orthographicSize * ZoomInMovementCoefficient * ZoomInSpeed + ZoomInBasicSpeed;
    }
    void Update()//판이 가까워질 수록 줌인속도가 빨라지는 현상을 해결해야한다.
    {
        Vector3 프레임당_차=new Vector3();
        Vector2 이동벡터=new Vector2();


        if (MainScript.MobilMode == true)
        {
    
            //카메라 종횡무브
            if(Input.touchCount>0)
            {
                //픽셀 차이값*(mycamera.orthographicSize/화면크기값)=적정 감도 이후 *감도

                Touch touch = Input.GetTouch(0);
                if(touch.phase==TouchPhase.Moved)
                {
                    print($"스크린: 가로:{Screen.width},세로:{Screen.height},,{mycamera.aspect}");
                    프레임당_차 = touch.deltaPosition;
                    프레임당_차.x *= -1f;
                    프레임당_차.y *= -1f;
                    프레임당_차.z = 0;
                    
                }
            }
        }
        else
        {
            float temp = -Input.GetAxis("Mouse ScrollWheel") * ZoomInMovement();
            print(temp);
            if (temp >= 0 && mycamera.orthographicSize < 카메라최대크기)
                mycamera.orthographicSize += temp * Time.deltaTime;
            else if (temp < 0 && mycamera.orthographicSize > 카메라최소크기)
                mycamera.orthographicSize += temp * Time.deltaTime;

            if (mycamera.orthographicSize < 카메라최소크기) mycamera.orthographicSize = 카메라최소크기;



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