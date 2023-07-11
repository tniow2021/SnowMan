using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript2 : MonoBehaviour
{
    // Start is called before the first frame update
    public RectTransform À­ÆÇ³Ú;
    public RectTransform ¾Æ·¡ÆÇ³Ú;
    void Start()
    {
        int width = Screen.width;
        int height = Screen.height;
        À­ÆÇ³Ú.sizeDelta = new Vector2(À­ÆÇ³Ú.sizeDelta.x, (height - width) / 2);
        ¾Æ·¡ÆÇ³Ú.sizeDelta = new Vector2(¾Æ·¡ÆÇ³Ú.sizeDelta.x, (height - width) / 2);

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
