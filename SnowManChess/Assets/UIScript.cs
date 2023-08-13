using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    // Start is called before the first frame update
    public RectTransform 윗판넬;
    public RectTransform 아래판넬;
    public void 시작()
    {
        SceneManager.LoadScene("Game");

    }
    public void 메인화면()
    {
        SceneManager.LoadScene("main");
    }
    public void 나가기()
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



