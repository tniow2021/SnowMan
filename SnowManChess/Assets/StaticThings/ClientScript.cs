using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
public class communication
{
    /*
     -�����ͱ���ü1,2,3
    ���� ������
    �ǽð� ���� ������
    -������ ����ü �˻�
    -����Ȯ��
    ����ȵ������� �翬��õ�
    �ٷ� �翬��ȵǸ� �翬��ɶ����� ��� �۾��� ������ ����Ÿ�Ӷ� ���·� �ǵ����� 
    ȭ�鿡 ����� ���� �̶� ������ ��ư�� �־����

     ������()
    {
    }
     */
}
public class ClientScript : MonoBehaviour
{
    
    public GameObject ClinetGameObject;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(ClinetGameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
