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
    public TMP_Text �α��ι���;


    private void Start()
    {
    }
    public void �Է¹�ư()
    {
        if (string.Equals(ID.text, idtest) && string.Equals(PS.text, pstest))
        {


            Debug.Log("�α��μ���!");
            �α��ι���.text = "�α��μ���!";
  
        
        }
        else
        {
            Debug.Log("�α��ν���...");
            �α��ι���.text = "�α��ν���...";
        }
        ID.text = "";
        PS.text = "";
    }

}
