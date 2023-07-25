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
    public void AllInput()
    {
            TouchInput();
    }
    //XYSwitch()는 타일 스크립트에서 호출당하는 함수
    Vector2Int XY = new Vector2Int();
    bool XYChange = true;
    public void XYSwitch(Vector2Int coordinate)
    {
        if (coordinate != XY) XYChange = true;
        else XYChange = false;
         XY = coordinate;
    }
    //aaaaaaaaaaaaaaaaaaaaaaaaaaa
    enum InputStateKind
    {
        StandBy,
        Touch,
        Drag
    }
    public float TouchDecisionTime = 0.7f;
    public float timer=0;
    public  CameraScript cameraScript;
    InputStateKind inputState = InputStateKind.StandBy;
    HelpCalculation calculation= new HelpCalculation();
    void TouchInput()
    {
        /*
         * 클릭입력은 나중에 터치입력으로 바꿔야함.
         * 터치가 있으면 시간을 재는데 시간이 흐르는 동안 다른 타일좌표나 맵바깥에 클릭이 발생하면
         * ,break로 입력을 취소할 것.
         *  좌표값은 록과 언록을 걸어서 쓰레드끼리 충돌나지 않게하기
         * 
         *  터치후 0.7초 이내로 moved상태가 되면 화면이동
         *  터치후 0.7초 이내로 ended상태가 되면 기물선택
         *  터치후 0.7초가 지나도 Stationary상태면 기물경로 드래그
         *  
         *  더블터치는 어떨까.
         */
        if (MainScript.MobilMode==true)
        {
            
            List<Touch> touchList = new List<Touch>();
            for (int i = 0; i < Input.touchCount; i++)
            {
                touchList.Add(Input.GetTouch(i));
            }

            if(touchList.Count==0)//모든 걸 초기화
            {
                inputState = InputStateKind.StandBy;
                //초기화
                cameraScript.CameraMoveAble = true;
                timer = 0;
                calculation.PlusPlusVector2(true);//리셋
            }
            else if(touchList.Count == 1)
            {
                /*손가락을 뗀후. 손가락이 닿아져있을 때에 이동한 움직임의 거리가
                * 50픽셀이하면 터치,(카메라 멈춤)
                * 0.7초 안에 50픽셀이상이면 화면이동(카메라 이동 허용)
                * 0.7초 가만히 있다가 50픽셀이상 움직이면 경로지정(카메라 멈춤)
                */
                timer += Time.deltaTime;
                TouchPhase state = touchList[0].phase;
                //move에서 판단하되 터치가 사라지거나 2개가 되면 모든 판단을 초기화
                if (state == TouchPhase.Moved|| state == TouchPhase.Stationary)
                {
                    //이동거리 합산
                    print($"ddd{touchList[0].deltaPosition}");
                    calculation.PlusPlusVector2(touchList[0].deltaPosition);

                    cameraScript.CameraMoveAble = false;

                    print(calculation.PlusPlusVector2(false));
                    if (calculation.PlusPlusVector2(false) < 50)
                    {
                        if (timer > TouchDecisionTime)
                        {
                            inputState = InputStateKind.Drag;
                            List2TileObject[XY.x][XY.y].GetComponent<SpriteRenderer>().color = Color.green;
                            cameraScript.CameraMoveAble = false;
                        }
                    }
                    if(inputState != InputStateKind.Drag)
                    {
                        if(calculation.PlusPlusVector2(false) > 50)
                        {
                            
                            //화면 이동중인 상태
                            inputState = InputStateKind.StandBy;
                            cameraScript.CameraMoveAble = true;
                        }
                    }
                 
                    
                }
                else if(touchList[0].phase == TouchPhase.Ended)
                {
                    if(inputState!=InputStateKind.Drag)
                    {
                        if (timer < TouchDecisionTime)//0.7초이상 클릭하면 무조건 드래그로 넘어간다.
                        {
                            print("fffffffffffffffffff");
                            if (calculation.PlusPlusVector2(false) < 50)//터치
                            {
                                inputState = InputStateKind.Touch;
                                List2TileObject[XY.x][XY.y].GetComponent<SpriteRenderer>().color = Color.red;
                                cameraScript.CameraMoveAble = false;

                                //초기화
                                calculation.PlusPlusVector2(true);
                                timer = 0;
                            }
                        }
                    }
                }
            }
            else if(touchList.Count == 2)
            {
                cameraScript.CameraMoveAble = true;
            }
            else if(touchList.Count>2)
            {
                cameraScript.CameraMoveAble = true;
            }
            
            //inputstate에 따른 처리
            if(inputState==InputStateKind.Touch)
            {
                //터치를 하는 동안에는 카메라를 멈춘다.
                cameraScript.CameraMoveAble = false;
            }
            else if(inputState==InputStateKind.Drag)
            {
                //드래그를 하는 동안에는 카메라를 멈춘다.
                cameraScript.CameraMoveAble = false;
            }
            else if(inputState==InputStateKind.StandBy)
            {
                //드래그도 터치도하지않는 대기상태라면 카메라가 움직일 수 있게 풀어준다.
                //cameraScript.CameraMoveAble = true;
            }
        }
        else
        {

        }
        
            
       
    }
}
public partial class GameScript //이동처리
{

}
