using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    UserInput userInput;
    Map map;
    public enum CommandKind
    {
        GameMap, //맵의 생성 파괴등.GameScript선에서 처리
        Move //기물,타일,건물,아이템등의 움직임,드랍에 대한 정보, Map 선에서 처리
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
            HardMove,//순식간에 좌표이동
            SoftMove,//스무스하게 이동
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
            public Vector2Int FromXY;//그때 그때 필요한 정보로서 유연하게 해석.
            public Vector2Int ToXY;
        }
    }
}

