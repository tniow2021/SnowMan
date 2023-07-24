using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Runtime.InteropServices;


public class IDPS : MonoBehaviour
{
    public TMP_InputField ID;
    public TMP_InputField PS;
    private string idtest = "user";
    private string pstest = "password";
    public TMP_Text 로그인문구;

    public string IP = "";

    private void Start()
    {
        
    }

    //communication.Client client;
    //public void 서버연결()
    //{
    //    client = new communication.Client();
    //    client.ClientStart(7777, IP);

    //}
    //public void 입력버튼()
    //{
    //    //if (string.Equals(ID.text, idtest) && string.Equals(PS.text, pstest))
    //    if(true)
    //    {
    //        if(client.Connectable())
    //        {
    //            client.Send($"INSERT INTO Players(UserName,UserPs) VALUES('{ID.text}','{PS.text}');");
    //            Debug.Log("로그인성공!");
    //            로그인문구.text = "로그인성공!";
                
    //        }
    //        else
    //        {
    //            Debug.Log("서버연결에 실패!");
    //            로그인문구.text = "서버연결에 실패!";
    //        }
           
  
        
    //    }
    //    else
    //    {
    //        Debug.Log("로그인실패...");
    //        로그인문구.text = "로그인실패...";
    //    }
    //    ID.text = "";
    //    PS.text = "";
    //}

}
