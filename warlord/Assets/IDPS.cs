using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Net;


public class IDPS : MonoBehaviour
{
    public TMP_InputField ID;
    public TMP_InputField PS;
    private string idtest = "user";
    private string pstest = "password";
    public TMP_Text 로그인문구;


    private void Start()
    {
    }
    public void 입력버튼()
    {
        if (string.Equals(ID.text, idtest) && string.Equals(PS.text, pstest))
        {


            Debug.Log("로그인성공!");
            로그인문구.text = "로그인성공!";
  
        
        }
        else
        {
            Debug.Log("로그인실패...");
            로그인문구.text = "로그인실패...";
        }
        ID.text = "";
        PS.text = "";
    }

}
