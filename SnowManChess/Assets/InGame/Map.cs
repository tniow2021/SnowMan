using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * 사용방법:
 * Map map1 인스턴스를 맴의 껍데기를 만든다.
 * 
 * MapCreate에 MapSet변수를 파라미터로 넘김으로서 
 * 맵에 타일과 기물이 종류별로 세팅되고
 * List<List<MapArea>>를 반환받는다.
 * 
 * map1 인스턴스를 통해서 맵에 명령을 내릴 수 있고
 * 반환받은 MapArea 인스턴스를 통해서 맵의 현재상황을 알 수 있다.
 * 
 * Map클래스 형태:
 * MapArea에 대한 입력(값수정)은 오로지 서버만 할 수 있다.(원격으로 메소드를 호출하는 걸 짜자)
 * 정확히 말하자면
 * 게임시작
 * :
 * 서버가 맵셋데이터 넘김->mapcreate->초기 기물배치함->다시 서버한테 맵셋데이터 넘김.
 * 서버가 두 유저의 맵셋데이터를 모두 받아들었다면 게임시작신호를 보냄
 * 이후 
 * 유저입력->바뀐것만 서버전달->검사 및 게임로직처리->서버입력(서버가 클라이언트에게 입력)
 * ->서버가 준 수정값과 현재 MapArea값을 비교해 변화값만 
 * Map클래스 선에서 기물이동(),타일변화(),기물죽음,아이템드랍()등의 함수를 호출시켜 맵에 반영한다
 * 
 * 즉 GameScript는 직접적으로 맵을 바꿀 수 없고 서버를 거쳐서 간다.
 * GameScript는 업데이트를 통해(1틱당.) MapArea반영()함수를 계속 호출해서 맵을 변화시킨다.
 * 
 * 속성
 * :
 * mapArea
 * mapSet
 * 유저가 현재 선택한 타일
 * 유저가 현재 선택한 기물체크
 * 
 * 메소드
 * 유저가입력():
 * 유저가 현재 선택한 타일체크함수(gameScript에서 타일스크립트를 out으로 받음)
 * 유저가 현재 선택한 기물체크함수(gameScript에서 기물스크립트를 out으로 받음)
 * ->gameScript에서 계속 update돌려가며 확인.
 * 
 * 
 * 유저에게 출력():
 * MapArea반영함수
 * -> 유저의 게임화면을 업데이트함
 * 서버가입력():
 * 서버에게 MapArea형식의 맵데이터를 전달받고 그걸 MapArea에 넣음
 * 서버에게 출력():
 * 유저가 내린 명령을 서버에게 전달.ㄴ
 * 서버로직:
 * 
 */

public class MapSet
{
    public Vector2Int size;
    public TK[,] TileSet;
    public IK[,] ItemSet;
    public (PK pk, Vector2Int xy, User user)[] PieceSet;
    public (BK bk, Vector2Int xy, User user)[] BuildingSet;
}
public class MapArdeas
{
    
}

public partial class Map : MonoBehaviour
{


    //맵 세트
    MapSet mapSet;
    public MapAreas mapArea;//main MapAreas

    //----------------------------------------메소드-------------------------------------

    public void MapCreate(MapSet _mapSet)
    {
        
        mapSet = _mapSet;
        mapArea = new MapAreas(this, mapSet.size);

        //메모리 만들기
        for (int x = 0; x < mapSet.size.x; x++)
        {
            for (int y = 0; y < mapSet.size.y; y++)// 오브젝트 단위 할당
            {
                //타일 집어넣기
                //맵셋의 타일셋에서 TK값을 불러와 그걸로 타일딕셔너리에서 검색


                Area area = FindArea(x, y);


                Create(mapSet.TileSet[x, y], area);
                Create(mapSet.ItemSet[x, y], area);
                //기물과 건물에는 유저클래스까지 넣어준다.
 

                mapArea.Insert(new Vector2Int(x, y), area);
            }
        }
        if(mapSet.PieceSet is not null)for(int i=0;i<mapSet.PieceSet.Length;i++)
        {
            (PK pk, Vector2Int xy, User user) a = mapSet.PieceSet[i];
            Create(a.pk, a.user, mapArea.Find(a.xy));
        }
        if (mapSet.BuildingSet is not null) for (int i = 0; i < mapSet.BuildingSet.Length; i++)
        {
            (BK bk, Vector2Int xy, User user) a = mapSet.BuildingSet[i];
            Create(a.bk, a.user, mapArea.Find(a.xy));
        }
    }
    public void Turn(int turnNumber)
    {
        mapArea.Turn(turnNumber);
    }
    public Area FindArea(Vector2Int index)
    {
        return mapArea.Find(index);
    }
    public Area FindArea(int x,int y)
    {
        return mapArea.Find(new Vector2Int(x,y));
    }
}
public partial class Map//서버로 올라가는 길
{

}
public partial class Map//내려가는 길
{



}
public partial class Map
{   

    public void BeAllTileWhite()
    {
        for(int i=0;i<mapSet.size.x;i++)
        {
            for (int j = 0;j < mapSet.size.y; j++)
            {
                mapArea.Find(i, j).tile.BetileWilte();
            }
        }
    }

    public void Create(PK pk,User user,Area area)
    {
        if(ObjectDict.Instance.FindObject(pk,out GameObject obj))
        {
            GameObject newPieceObject = Instantiate(obj);
            PieceScript newPieceScript = newPieceObject.GetComponent<PieceScript>();
            newPieceScript.user = user;
            if (area.Put(newPieceScript,out PieceScript oldPiece))
            {
                oldPiece.ObjectDestory();
            }
        }
    }
    public void Create(BK bk, User user, Area area)
    {
        if (ObjectDict.Instance.FindObject(bk, out GameObject obj))
        {
            GameObject newBuildingObject = Instantiate(obj);
            BuildingScript newBuildingScript = newBuildingObject.GetComponent<BuildingScript>();
            newBuildingScript.user = user;
            if (area.Put(newBuildingScript, out BuildingScript oldBuilding))
            {
                oldBuilding.ObjectDestory();
            }
        }
    }
    public void Create(TK tk, Area area)
    {
        if(ObjectDict.Instance.FindObject(tk,out GameObject obj))
        {
            GameObject newTileObject = Instantiate(obj);
            TileScript newTileScript = newTileObject.GetComponent<TileScript>();
            if(area.Put(newTileScript, out TileScript oldtile))
            {
                oldtile.ObjectDestory();
            }
        }
    }   
    public void Create(IK ik, Area area)
    {
        if(ObjectDict.Instance.FindObject(ik,out GameObject obj))
        {
            GameObject newItemObject = Instantiate(obj);
            ItemScript newItemScript = newItemObject.GetComponent<ItemScript>();
            if(area.Put(newItemScript, out ItemScript oldItem))
            {
                oldItem.ObjectDestory();
            }
        }
    }
}



