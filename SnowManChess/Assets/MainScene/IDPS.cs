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
    public TMP_Text �α��ι���;

    public string IP = "";

    private void Start()
    {
        
    }

    //communication.Client client;
    //public void ��������()
    //{
    //    client = new communication.Client();
    //    client.ClientStart(7777, IP);

    //}
    //public void �Է¹�ư()
    //{
    //    //if (string.Equals(ID.text, idtest) && string.Equals(PS.text, pstest))
    //    if(true)
    //    {
    //        if(client.Connectable())
    //        {
    //            client.Send($"INSERT INTO Players(UserName,UserPs) VALUES('{ID.text}','{PS.text}');");
    //            Debug.Log("�α��μ���!");
    //            �α��ι���.text = "�α��μ���!";
                
    //        }
    //        else
    //        {
    //            Debug.Log("�������ῡ ����!");
    //            �α��ι���.text = "�������ῡ ����!";
    //        }
           
  
        
    //    }
    //    else
    //    {
    //        Debug.Log("�α��ν���...");
    //        �α��ι���.text = "�α��ν���...";
    //    }
    //    ID.text = "";
    //    PS.text = "";
    //}

}
