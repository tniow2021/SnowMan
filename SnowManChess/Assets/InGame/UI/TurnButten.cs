using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnButten : MonoBehaviour
{
    public User user;//���ӽ�ũ��Ʈ���� ����
    public void Turnning()
    {
        GameScript.instance.userManager.TurnChange(user);
    }
}
