using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    UserInput userInput;
    Map map;
    public enum CommandKind
    {
        GameMap, //���� ���� �ı���.GameScript������ ó��
        Move //�⹰,Ÿ��,�ǹ�,�����۵��� ������,����� ���� ����, Map ������ ó��
    }
    public class Map
    {
        public enum ActionKind
        {
            Create,
            Delete,
        }
        public class Post
        {
            ActionKind AtionKind;
            MapSet mapset;
        }
    }
    public class ThingOntile
    {
        public enum ActionKind
        {
            None,
            HardMove,//���İ��� ��ǥ�̵�
            SoftMove,//�������ϰ� �̵�
            Delete,
            Create
        }
        public enum ObjectKind
        {
            Piece,
            Item,
            Bulid
        }
        public class Post
        {
            public ObjectKind objectKind = ObjectKind.Piece;
            public GameObject gameObject;
            public ActionKind actionKind = ActionKind.None;
            public Vector2Int FromXY;//�׶� �׶� �ʿ��� �����μ� �����ϰ� �ؼ�.
            public Vector2Int ToXY;
        }
    }
}

