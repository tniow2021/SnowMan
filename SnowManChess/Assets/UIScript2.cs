using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript2 : MonoBehaviour
{
    // Start is called before the first frame update
    public RectTransform ���ǳ�;
    public RectTransform �Ʒ��ǳ�;
    void Start()
    {
        int width = Screen.width;
        int height = Screen.height;
        ���ǳ�.sizeDelta = new Vector2(���ǳ�.sizeDelta.x, (height - width) / 2);
        �Ʒ��ǳ�.sizeDelta = new Vector2(�Ʒ��ǳ�.sizeDelta.x, (height - width) / 2);

    }
    int tempi = 0;
    // Update is called once per frame
    void Update()
    {
        tempi++;
        if (tempi > 1000)
        {

        }
    }
}
