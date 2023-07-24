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
         * Ŭ���Է��� ���߿� ��ġ�Է����� �ٲ����.
         * ��ġ�� ������ �ð��� ��µ� �ð��� �帣�� ���� �ٸ� Ÿ����ǥ�� �ʹٱ��� Ŭ���� �߻��ϸ�
         * ,break�� �Է��� ����� ��.
         *  ��ǥ���� �ϰ� ����� �ɾ �����峢�� �浹���� �ʰ��ϱ�
         *  
         *  ��� ��ġ�� 0.7�� ������ ��ġ�� �������� ��ġ�������
         *  ��� ��ġ�� 0.7�� ������ ��ġ�� ���������ʴ� ���·� �������(�����̾�߸�)1ĭ��ǥ���� �巡�׵Ǹ� �б�������
         *  ��� ��ġ�� 0.7�ʰ� ������ �巡�׸�� ����
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

    //�Ʒ��� �ٸ� ������ ȣ����ϴ� �Լ�
    Vector2Int XY = new Vector2Int();
    bool XY_rock = true;
    public void XYSwitch(Vector2Int coordinate)
    {
        XY = coordinate;//7��23�� �������..............
    }
}
public partial class GameScript //�̵�ó��
{

}
