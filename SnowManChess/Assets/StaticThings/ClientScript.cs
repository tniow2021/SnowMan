using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;

/*
 * Ŭ���̾�Ʈ-���� ���� ��������(7�� 26��)
 * 
 * ������ �帧
 * 
 * -----------------Ŭ���̾�Ʈ--------------------------------------
 * ������:
 * GameScript->MakeDataSet Ŭ������ �� �ֱ�->MakeDataSet�� �Լ�ȣ��
 * ->MakeBinary(���ڵ�)->communication---->[����]
 * 
 * �ޱ�: (�������� �����̴�)
 * 
 * -----------------���� (Ŭ���̾�Ʈ�� �����̴�)--------------
 * �ޱ�:
 * communication->MakeBinary(���ڵ�)->MakeDataSet�� �����������Լ� ȣ��
 * ->MakeDataSet���� DataSet �ν��Ͻ��� �����ϰ� ���ӷ���ó��Ŭ������ ��ȯ
 * �̶� DataSet�� ���ӷ���ó��Ŭ������ ���� �߾˾Ƹ԰� �ణ�� �߰� ���������� ��ģ��.
 * 
 * ������: ( �ޱ��� �����̴�)
 * ---------------------------------------------------------------
 * 
 * 
 */
public static class communication
{
    /*
     * ���� �� 3��°
     * MakeBinary���� ���� ����Ʈ���� �˾Ƽ� �� �� ����ϰ� �����ְ� �����ϰ� ��Ű����� �����ϰ�
     * �����Ѵ�.
     * 
     * communication Ŭ������ �ְ�ޱ⸦ ����Ѵ�.
     * update���� �̿��Ѵ�.
     * 
     * 
     * ������ �ȵǸ� ������ �ٽ� �õ��غ��� �׷��� �ȵǸ� ȭ�鿡 ���ۺ����̸� ����
     * �����ð� �������� ������ �õ��غ��� �ȵǸ� ��Ƽ�÷��� ������ ������ �޼����� ����. 
     * 
     */
}
public class ClientScript : MonoBehaviour
{
    
    public GameObject ClinetGameObject;
    public string ServerIP;//����Կ��� ���� ����� ȭ��ó���� ip�� ��Ʈ�� ������ �� �ְ� �����
    public int ServerPort;

    public string PresentScene;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(ClinetGameObject);
    }

    void Update()
    {

    }
}
public static class MakeBinary
{
    /*
     * ���� �� 2��°
     * MakeDetaSet���� �Ѱܹ��� DataSet �ν��Ͻ��� 
     * ��ü�ؼ� �ϳ��� ����Ʈ���� �����. ������� ����Ʈ ���� 
     * communication Ŭ������ ������.
     * 
     * �� Ŭ�������� ���̳ʸ� �������� ������ �����Ѵ�
     * 
     */
}
public class DataSet
{
    /*
     * �����ͼ��� ���⿡ �����Ѵ�.
     */
}
public static class MakeDetaSet
{
    static DataSet dataSet = new DataSet();
    /*
     * ���� �� 1��°
     * ���ӽ�ũ��Ʈ���� �� ����Ŭ������ �����ؼ� �̾ȿ� �ִ� �������� ä���
     * ������ �� ä������ Ư�� �Լ��� ������� ������� DataSet �ν��Ͻ��� MakeBinary�� ������.
     * 
     */
}



