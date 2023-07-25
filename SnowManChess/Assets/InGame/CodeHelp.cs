using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpCalculation//��� ���� ������ ����
{
    //ȣ���Ҷ������� ���� �������� ��ȯ�ϴ� ģ���� ����
    float RememberValue1;
    float Count1 = 0;
    public float RememberFloatDelta(float value, bool reset = false)
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
        float delta = value - RememberValue1;
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
        RememberValue2 = new Vector2(0, 0);
        Count2 = 0;
        return returnValue;

    }

    //������ �̵��Ÿ�(���״�� ������ ���ڱ� ��ŭ�� �Ÿ�)�� �ջ��ؼ� ��ȯ���ش�
    Vector2 RememberValue3 = new Vector2(0, 0);
    public float PlusPlusVector2(Vector2 value)
    {
        RememberValue3 += value;

        float xdistance = RememberValue3.x;
        float ydistance = RememberValue3.y;
        xdistance *= xdistance;//����;
        ydistance *= ydistance;
        float distance = Mathf.Sqrt(xdistance + ydistance);
        return distance;
    }
    public float PlusPlusVector2(Vector3 value)
    {
        RememberValue3.x += value.x;
        RememberValue3.y += value.y;

        float xdistance = RememberValue3.x;
        float ydistance = RememberValue3.y;
        xdistance *= xdistance;//����;
        ydistance *= ydistance;
        float distance = Mathf.Sqrt(xdistance + ydistance);
        return distance;
    }
    public float PlusPlusVector2(bool reset)
    {
        if(reset==true)
        {
            RememberValue3 = new Vector2(0, 0);
            return 0;
        }
        else
        {
            float xdistance = RememberValue3.x;
            float ydistance = RememberValue3.y;
            xdistance *= xdistance;//����;
            ydistance *= ydistance;
            float distance = Mathf.Sqrt(xdistance + ydistance);
            return distance;
        }
    }



}
public class CodeHelp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
