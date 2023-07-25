using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpCalculation//계산 보조 도구들 모임
{
    //호출할때마다의 숫자 변동값을 반환하는 친구들 모임
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
            return 0;//첫 반환에는 움직이지않음.
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
        RememberValue2 = new Vector2(0, 0);
        Count2 = 0;
        return returnValue;

    }

    //벡터의 이동거리(말그대로 움직인 발자국 만큼의 거리)를 합산해서 반환해준다
    Vector2 RememberValue3 = new Vector2(0, 0);
    public float PlusPlusVector2(Vector2 value)
    {
        RememberValue3 += value;

        float xdistance = RememberValue3.x;
        float ydistance = RememberValue3.y;
        xdistance *= xdistance;//제곱;
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
        xdistance *= xdistance;//제곱;
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
            xdistance *= xdistance;//제곱;
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
