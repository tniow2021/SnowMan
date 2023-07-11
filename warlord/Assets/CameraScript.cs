using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CameraScript : MonoBehaviour
{
    public float scrollSpeed;//얘는 나중에 유저설정칸으로 유저직접입력가능하게한다.
    public float 카메라최대크기;
    public float 카메라최소크기;

    public float 좌측이동한계;
    public float 우측이동한계;
    public float 위이동한계;
    public float 아래이동한계;


    //카메라종횡무브
    public float cameramovespeed;//얘는 설정으로 넣는다.
    bool temp2 = false;
    Vector3 temp3;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(Input.mousePosition);


        //카메라종횡무브
        temp3 = Input.mousePosition;
   
    }
   
    // Update is called once per frame
    void Update()//판이 가까워질 수록 줌인속도가 빨라지는 현상을 해결해야한다.
    {
        Camera mycamera = GetComponent<Camera>();

        float temp = -Input.GetAxis("Mouse ScrollWheel");
        if (temp >= 0 && mycamera.orthographicSize < 카메라최대크기)
            mycamera.orthographicSize+= temp * Time.deltaTime * scrollSpeed;
        else if(temp<0 && mycamera.orthographicSize > 카메라최소크기)
            mycamera.orthographicSize += temp * Time.deltaTime * scrollSpeed;


        //카메라 종횡무브
        if (Input.GetMouseButtonDown(2)) temp2 = true;//(2)는 스크롤 버튼 클릭인 경우다.
        if (Input.GetMouseButtonUp(2)) temp2 = false;
        if(temp2==true)
        {
           
            Vector3 프레임당_차 = temp3 - Input.mousePosition;
            //픽셀가로 Screen.width
            //픽셀세로 Screen.height
            //Vector2 이동벡터 = new Vector2(프레임당_차.x* mycamera.orthographicSize * 2 / Screen.width,
            //    프레임당_차.y * mycamera.orthographicSize * mycamera.aspect / Screen.height);
            Vector2 이동벡터 = 프레임당_차 * cameramovespeed* mycamera.orthographicSize / 1000;


            //카메라 이동공간을 정의
            if (transform.position.x < 좌측이동한계 && 이동벡터.x < 0) 이동벡터.x = 0;
            if (transform.position.x > 우측이동한계 && 이동벡터.x > 0) 이동벡터.x = 0;
            if (transform.position.y > 위이동한계 && 이동벡터.y > 0) 이동벡터.y = 0;
            if (transform.position.y < 아래이동한계 && 이동벡터.y < 0) 이동벡터.y = 0;
            transform.Translate(이동벡터 );
            //화면상 이동비율= 프레임당_차/new vector3(픽셀가로 Screen.width,픽셀세로 Screen.height,0)
            //이동벡터=화면상비친 면적의 종횡크기*화면상이동비율

        }
        //카메라종횡무브
        temp3 = Input.mousePosition;
    }
}
