using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User 
{
    public User(UserKind _kind, int _ID, string _name = "")
    {
        name = _name;
        ID = _ID;
        kind = _kind;
    }
    public UserKind kind { get; }
    public int ID { get; }
    public string name { get; }
}
