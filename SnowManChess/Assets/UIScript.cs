using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    // Start is called before the first frame update
    public RectTransform ���ǳ�;
    public RectTransform �Ʒ��ǳ�;
    public void ����()
    {
        SceneManager.LoadScene("Game");

    }
    public void ����ȭ��()
    {
        SceneManager.LoadScene("main");
    }
    public void ������()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
    }
    public void MultiPlay()
    {

    }
    ////////


}



