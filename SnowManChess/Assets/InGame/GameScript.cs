using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameScript : MonoBehaviour
{
    /*
     * ���ӽ��۽ÿ��� �ʿ��� ����
     * :ü���� ũ��, Ÿ�Ϻ� ������(����, �ڿ�)
     * �ǽð� �������(����� �͸� ���, �ֱ������� ��ü�� �˻�)
     * :
     * �⹰ �̵�
     * Ÿ������ ���泻��
     * 
     * 
     * �������� �� �ܰ��� ��ȹ
     * :
     * clinet script�� ���� �ϴ� ���ڿ� ���.
     * �⹰�̵� ����ȭ
     * ���۽���ŵ�����, �ǽð� ������ ����ü ¥��
     * client script�ۼ�
     * ������ �ϱ�ȯ�� ����ŷ ����
     * �����ۼ�
     * 
     * �巡�� �̵���μ���
     * ��(������Ʈ�������� �ּҴ��� ����. �� 50ms��)
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
public partial class GameScript : MonoBehaviour//�� �� ��� ����
{
    //�׽�Ʈ
    public GameObject testTileObject;

    //Ÿ�� ��������
    public enum TK//Tile kind
    {
        Snow,
        Ice,
        Lake,
        Soil,
        Lava
    }
    public TK[,] customMap=new TK[,]//���߿� ������ ������ �� ��
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
    };//���� ��� ������ ��
    public GameObject SnowTile;
    public GameObject IceTile;
    public GameObject LakeTile;
    public GameObject SoilTile;
    public GameObject LavaTile;

    //Ÿ�� �ִϸ����� ����. (�⺻�����Լ����� �� Getcomponent�� �ִ´�)
    private Animator Ani_SnowTile;
    private Animator Ani_IceTile;
    private Animator Ani_LakeTile;
    private Animator Ani_SoilTile;
    private Animator Ani_LavaTile;

    //��ũ��, �ʿ�����Ʈ ����
    public GameObject MapObject;
    public int MapTileWeight;
    public int MapTileheight;

    //�߿�. �� ����� ����Ʈ
        //Ÿ�Ͽ�����Ʈ�� 2���� ����Ʈ�� ����� �����Ѵ�.
    public List<List<GameObject>> List2TileObject = new List<List<GameObject>>();
        //�� ���� Ÿ�Ͻ�ũ��Ʈ�� 2���� ����Ʈ�� ����� �����Ѵ�.
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

        //��(Ÿ�ϸ���Ʈ)�ʱ�ȭ �� Ÿ�Ͻ�ũ��Ʈ�ʱ�ȭ
        foreach (List<GameObject> obj in List2TileObject)
        {
            foreach(GameObject objj in obj)
            {
                Destroy(objj);//Ÿ�� �� ����
            }
        }
        List2TileObject.Clear();
        List2TileScript.Clear();

        //�Ű����� ���μ��� ũ���� 2���� ����Ʈ�� ä���ִ´�.
        for (int x = 0; x < width; x++)
        {
            List<TileScript> newList1TileScript = new List<TileScript>();
            List<GameObject> newList1TileObject = new List<GameObject>();
            for (int y = 0; y < height; y++)// ������Ʈ ���� �Ҵ�
            {
                GameObject newTileObject = Instantiate(testTileObject);
                TileScript newtileScript = newTileObject.GetComponent<TileScript>();

                //MapObject�� �ڽ����� �����.
                newTileObject.transform.parent = MapObject.transform;
                //MapObject�Ʒ� ��ǥ������ ��ġ�Ѵ�
                newTileObject.transform.localPosition = new Vector3(x, y, 0);

                newtileScript.coordinate = new Vector2Int(x, y);

                newList1TileObject.Add(newTileObject);
                newList1TileScript.Add(newtileScript);
                print("�߸���");
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
public partial class GameScript //�Է�ó��
{
    public void AllInput()
    {
            TouchInput();
    }
    //XYSwitch()�� Ÿ�� ��ũ��Ʈ���� ȣ����ϴ� �Լ�
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
         * Ŭ���Է��� ���߿� ��ġ�Է����� �ٲ����.
         * ��ġ�� ������ �ð��� ��µ� �ð��� �帣�� ���� �ٸ� Ÿ����ǥ�� �ʹٱ��� Ŭ���� �߻��ϸ�
         * ,break�� �Է��� ����� ��.
         *  ��ǥ���� �ϰ� ����� �ɾ �����峢�� �浹���� �ʰ��ϱ�
         * 
         *  ��ġ�� 0.7�� �̳��� moved���°� �Ǹ� ȭ���̵�
         *  ��ġ�� 0.7�� �̳��� ended���°� �Ǹ� �⹰����
         *  ��ġ�� 0.7�ʰ� ������ Stationary���¸� �⹰��� �巡��
         *  
         *  ������ġ�� ���.
         */
        if (MainScript.MobilMode==true)
        {
            
            List<Touch> touchList = new List<Touch>();
            for (int i = 0; i < Input.touchCount; i++)
            {
                touchList.Add(Input.GetTouch(i));
            }

            if(touchList.Count==0)//��� �� �ʱ�ȭ
            {
                inputState = InputStateKind.StandBy;
                //�ʱ�ȭ
                cameraScript.CameraMoveAble = true;
                timer = 0;
                calculation.PlusPlusVector2(true);//����
            }
            else if(touchList.Count == 1)
            {
                /*�հ����� ����. �հ����� ��������� ���� �̵��� �������� �Ÿ���
                * 50�ȼ����ϸ� ��ġ,(ī�޶� ����)
                * 0.7�� �ȿ� 50�ȼ��̻��̸� ȭ���̵�(ī�޶� �̵� ���)
                * 0.7�� ������ �ִٰ� 50�ȼ��̻� �����̸� �������(ī�޶� ����)
                */
                timer += Time.deltaTime;
                TouchPhase state = touchList[0].phase;
                //move���� �Ǵ��ϵ� ��ġ�� ������ų� 2���� �Ǹ� ��� �Ǵ��� �ʱ�ȭ
                if (state == TouchPhase.Moved|| state == TouchPhase.Stationary)
                {
                    //�̵��Ÿ� �ջ�
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
                            
                            //ȭ�� �̵����� ����
                            inputState = InputStateKind.StandBy;
                            cameraScript.CameraMoveAble = true;
                        }
                    }
                 
                    
                }
                else if(touchList[0].phase == TouchPhase.Ended)
                {
                    if(inputState!=InputStateKind.Drag)
                    {
                        if (timer < TouchDecisionTime)//0.7���̻� Ŭ���ϸ� ������ �巡�׷� �Ѿ��.
                        {
                            print("fffffffffffffffffff");
                            if (calculation.PlusPlusVector2(false) < 50)//��ġ
                            {
                                inputState = InputStateKind.Touch;
                                List2TileObject[XY.x][XY.y].GetComponent<SpriteRenderer>().color = Color.red;
                                cameraScript.CameraMoveAble = false;

                                //�ʱ�ȭ
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
            
            //inputstate�� ���� ó��
            if(inputState==InputStateKind.Touch)
            {
                //��ġ�� �ϴ� ���ȿ��� ī�޶� �����.
                cameraScript.CameraMoveAble = false;
            }
            else if(inputState==InputStateKind.Drag)
            {
                //�巡�׸� �ϴ� ���ȿ��� ī�޶� �����.
                cameraScript.CameraMoveAble = false;
            }
            else if(inputState==InputStateKind.StandBy)
            {
                //�巡�׵� ��ġ�������ʴ� �����¶�� ī�޶� ������ �� �ְ� Ǯ���ش�.
                //cameraScript.CameraMoveAble = true;
            }
        }
        else
        {

        }
        
            
       
    }
}
public partial class GameScript //�̵�ó��
{

}
