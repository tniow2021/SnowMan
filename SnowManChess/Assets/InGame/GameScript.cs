using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameScript : MonoBehaviour
{
    /*
     * 게임시작시에만 필요한 정보
     * :체스판 크기, 타일별 정보들(지형, 자원)
     * 실시간 통신정보(변경된 것만 통신, 주기적으로 전체를 검사)
     * :
     * 기물 이동
     * 타일정보 변경내역
     * 
     * 
     * 만들어야할 것 단계적 계획
     * :
     * clinet script를 허브로 하는 문자열 통신.
     * 기물이동 동기화
     * 시작시통신데이터, 실시간 데이터 구조체 짜기
     * client script작성
     * 잠재적 턴교환과 마스킹 구현
     * 서버작성
     * 
     * 드래그 이동경로설정
     * 핑(오브젝트움직임의 최소단위 구현. 한 50ms로)
     */
    void Start()
    {
        MapSetting(MapTileWeight, MapTileheight);
    }
    void Update()
    {
     
        AllInput();
    }
}
public partial class GameScript : MonoBehaviour//맵 및 요소 관련
{
    //테스트
    public GameObject testTileObject;

    //타일 종류관련
    public enum TK//Tile kind
    {
        Snow,
        Ice,
        Lake,
        Soil,
        Lava
    }
    public TK[,] customMap=new TK[,]//나중에 데이터 셋으로 쓸 것
    {
        {TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow},
        {TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow},
        {TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow},
        {TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow},
        {TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow},
        {TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow},
        {TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow},
        {TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow},
        {TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow,TK.Snow}
    };//예비 통신 데이터 셋
    public GameObject SnowTile;
    public GameObject IceTile;
    public GameObject LakeTile;
    public GameObject SoilTile;
    public GameObject LavaTile;

    //타일 애니메이터 관련. (기본세팅함수에서 다 Getcomponent로 넣는다)
    private Animator Ani_SnowTile;
    private Animator Ani_IceTile;
    private Animator Ani_LakeTile;
    private Animator Ani_SoilTile;
    private Animator Ani_LavaTile;

    //맵크기, 맵오브젝트 관련
    public GameObject MapObject;
    public int MapTileWeight;
    public int MapTileheight;

    //중요. 맵 제어용 리스트
        //타일오브젝트를 2차원 리스트로 만들어 관리한다.
    public List<List<GameObject>> List2TileObject = new List<List<GameObject>>();
        //그 속의 타일스크립트를 2차원 리스트로 만들어 관리한다.
    public List<List<TileScript>> List2TileScript = new List<List<TileScript>>();

    private void BasicSetting()
    {
        Ani_SnowTile = SnowTile.GetComponent<Animator>();
        Ani_IceTile = IceTile.GetComponent<Animator>();
        Ani_LakeTile = LakeTile.GetComponent<Animator>();
        Ani_SoilTile = SoilTile.GetComponent<Animator>();
        Ani_LavaTile = LavaTile.GetComponent<Animator>();
    }
    void MapSetting(int width, int height)
    {
        print($"{width} {height}");
        //BasicSetting();

        //맵(타일리스트)초기화 및 타일스크립트초기화
        foreach (List<GameObject> obj in List2TileObject)
        {
            foreach(GameObject objj in obj)
            {
                Destroy(objj);//타일 다 없앰
            }
        }
        List2TileObject.Clear();
        List2TileScript.Clear();

        //매개변수 가로세로 크기대로 2차원 리스트를 채워넣는다.
        for (int x = 0; x < width; x++)
        {
            List<TileScript> newList1TileScript = new List<TileScript>();
            List<GameObject> newList1TileObject = new List<GameObject>();
            for (int y = 0; y < height; y++)// 오브젝트 단위 할당
            {
                GameObject newTileObject = Instantiate(testTileObject);
                TileScript newtileScript = newTileObject.GetComponent<TileScript>();

                //MapObject의 자식으로 만든다.
                newTileObject.transform.parent = MapObject.transform;
                //MapObject아래 좌표값으로 배치한다
                newTileObject.transform.localPosition = new Vector3(x, y, 0);

                newtileScript.coordinate = new Vector2Int(x, y);

                newList1TileObject.Add(newTileObject);
                newList1TileScript.Add(newtileScript);
                print("삐리리");
            }
            List2TileObject.Add(newList1TileObject);
            List2TileScript.Add(newList1TileScript);
        }
        for(int x=0;x<width;x++)
            for(int y=0;y<height;y++)
            {
                //print($"{List2TileObject[x][y].GetComponent<TileScript>().coordinate}:{x},{y}");
                
            }
    }
}
public partial class GameScript //입력처리
{

    enum InputStateKind
    {
        Touch,
        Drag,
        Push,
    }
    public string inputState = "";
    public float TouchDecisionTime = 0.7f;
    public void AllInput()
    {
        if (MainScript.MobilMode == true)
        {
            TouchInput();
        }
        else
        {
            MouseInput();
            KeyBoardInput();
        }

    }
    float timer;
    void MouseInput()
    {

    }
    void TouchInput()
    {
        /*
         * 클릭입력은 나중에 터치입력으로 바꿔야함.
         * 터치가 있으면 시간을 재는데 시간이 흐르는 동안 다른 타일좌표나 맵바깥에 클릭이 발생하면
         * ,break로 입력을 취소할 것.
         *  좌표값은 록과 언록을 걸어서 쓰레드끼리 충돌나지 않게하기
         *  
         *  블록 터치후 0.7초 이전에 터치가 끊어지면 터치모드진입
         *  블록 터치후 0.7초 이전에 터치가 끊어지지않는 상태로 인접블록(빙판이어야만)1칸좌표으로 드래그되면 밀기모드진입
         *  블록 터치후 0.7초가 지나면 드래그모드 진입
         */
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            testTileObject.GetComponent<SpriteRenderer>().color = Color.blue;
        }
            
        timer += Time.deltaTime;
    }
    void KeyBoardInput()
    {

    }

    //아래는 다른 곳에서 호출당하는 함수
    Vector2Int XY = new Vector2Int();
    bool XY_rock = true;
    public void XYSwitch(Vector2Int coordinate)
    {
        XY = coordinate;//7월23일 여기까지..............
    }
}
public partial class GameScript //이동처리
{

}
