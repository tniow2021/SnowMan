using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CameraScript : MonoBehaviour
{
    public float scrollSpeed;//��� ���߿� ��������ĭ���� ���������Է°����ϰ��Ѵ�.
    public float ī�޶��ִ�ũ��;
    public float ī�޶��ּ�ũ��;

    public float �����̵��Ѱ�;
    public float �����̵��Ѱ�;
    public float ���̵��Ѱ�;
    public float �Ʒ��̵��Ѱ�;


    //ī�޶���Ⱦ����
    public float cameramovespeed;//��� �������� �ִ´�.
    bool temp2 = false;
    Vector3 temp3;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(Input.mousePosition);


        //ī�޶���Ⱦ����
        temp3 = Input.mousePosition;
   
    }
   
    // Update is called once per frame
    void Update()//���� ������� ���� ���μӵ��� �������� ������ �ذ��ؾ��Ѵ�.
    {
        Camera mycamera = GetComponent<Camera>();

        float temp = -Input.GetAxis("Mouse ScrollWheel");
        if (temp >= 0 && mycamera.orthographicSize < ī�޶��ִ�ũ��)
            mycamera.orthographicSize+= temp * Time.deltaTime * scrollSpeed;
        else if(temp<0 && mycamera.orthographicSize > ī�޶��ּ�ũ��)
            mycamera.orthographicSize += temp * Time.deltaTime * scrollSpeed;


        //ī�޶� ��Ⱦ����
        if (Input.GetMouseButtonDown(2)) temp2 = true;//(2)�� ��ũ�� ��ư Ŭ���� ����.
        if (Input.GetMouseButtonUp(2)) temp2 = false;
        if(temp2==true)
        {
           
            Vector3 �����Ӵ�_�� = temp3 - Input.mousePosition;
            //�ȼ����� Screen.width
            //�ȼ����� Screen.height
            //Vector2 �̵����� = new Vector2(�����Ӵ�_��.x* mycamera.orthographicSize * 2 / Screen.width,
            //    �����Ӵ�_��.y * mycamera.orthographicSize * mycamera.aspect / Screen.height);
            Vector2 �̵����� = �����Ӵ�_�� * cameramovespeed* mycamera.orthographicSize / 1000;


            //ī�޶� �̵������� ����
            if (transform.position.x < �����̵��Ѱ� && �̵�����.x < 0) �̵�����.x = 0;
            if (transform.position.x > �����̵��Ѱ� && �̵�����.x > 0) �̵�����.x = 0;
            if (transform.position.y > ���̵��Ѱ� && �̵�����.y > 0) �̵�����.y = 0;
            if (transform.position.y < �Ʒ��̵��Ѱ� && �̵�����.y < 0) �̵�����.y = 0;
            transform.Translate(�̵����� );
            //ȭ��� �̵�����= �����Ӵ�_��/new vector3(�ȼ����� Screen.width,�ȼ����� Screen.height,0)
            //�̵�����=ȭ����ģ ������ ��Ⱦũ��*ȭ����̵�����

        }
        //ī�޶���Ⱦ����
        temp3 = Input.mousePosition;
    }
}
