using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public List<GameObject> trashPrefabs;

    List<List<GameObject>> trashPool; // 쓰래기 종류별로(List 하나)
    Dictionary<int, GameObject> parentObjs = new Dictionary<int, GameObject>(); // 쓰레기 종류별로 부모 오브젝트를 관리하는 딕셔너리
    //종류별로 쓰레기(List 둘)
    List<GameObject> trashList; // 로딩중인 쓰래기 전체를 담는 리스트 (언로드 할때 필요함)

    public void Init()
    {
        //start에서 풀 생성
        this.trashPool = new List<List<GameObject>>();
        this.trashList = new List<GameObject>();
        //this.trashPrefabs = new List<GameObject>();
        this.trashPrefabs = Resources.LoadAll<GameObject>("Prefabs").ToList().FindAll(x => x.GetComponent<TrashData>() != null);
        for (int i = 0; i < this.trashPrefabs.Count; i++)
        {
            this.trashPool.Add(new List<GameObject>()); // 쓰레기 종류별로 풀 생성
        }
    }
    public GameObject GetTrash(TrashInfo info)
    {
        foreach (GameObject trash in this.trashPool[info.kind])
        {
            if (!trash.activeSelf)
            {
                trash.GetComponent<TrashData>().Info = info; // info를 넣어줌
                trash.SetActive(true);
                this.trashList.Add(trash); // 로딩중인 쓰레기 리스트에 추가
                return trash;
            }
        }
        //여기로 나왔다는건 모든 풀이 활성화
        //새로 생성 후 그걸 반환
        CreateTrash(info.kind);
        this.trashPool[info.kind][trashPool[info.kind].Count - 1].GetComponent<TrashData>().Info = info;
        this.trashPool[info.kind][trashPool[info.kind].Count - 1].SetActive(true);
        this.trashList.Add(this.trashPool[info.kind][trashPool[info.kind].Count - 1]); // 로딩중인 쓰래기 리스트에 추가
        return this.trashPool[info.kind][trashPool[info.kind].Count - 1];
    }
    public void GetBreakableTrash(List<TrashInfo> infos)
    {
        // info값의 kind가 부숴지는 종류라면,
        // 생성 후 infos를 부숴지는 쓰레기 내부의 코드쪽으로 넘긴다.
        // 여기로 온 시점에서 전부 clean이 아니라서 생성은 해야한단 소리다.

        foreach (GameObject trash in this.trashPool[infos[0].kind])
        {
            if (!trash.activeSelf)
            {
                trash.SetActive(true);
                // 부숴지는 쓰레기 내부의 코드에 infos 전달
                trash.GetComponent<BreakableTrash>().SetInnerTrash(infos); // 부숴지는 쓰레기 내부의 코드에 infos 전달
                this.trashList.Add(trash); // 로딩중인 쓰레기 리스트에 추가
                return;
            }
        }
        CreateTrash(infos[0].kind); // 저 종류값으로 불러오면 prefab이 애초에 부숴지지않은 전체 기준이다.
        this.trashPool[infos[0].kind][trashPool[infos[0].kind].Count - 1].SetActive(true);
        

        // 그 후 부숴지는 쓰레기 내부의 코드에 infos 전달
        this.trashPool[infos[0].kind][trashPool[infos[0].kind].Count - 1].GetComponent<BreakableTrash>().SetInnerTrash(infos); // 부숴지는 쓰레기 내부의 코드에 infos 전달
        this.trashList.Add(this.trashPool[infos[0].kind][trashPool[infos[0].kind].Count - 1]); // 로딩중인 쓰래기 리스트에 추가
    }
    public void GetGrassTrash(List<TrashInfo> infos)
    {
        //해초 쓰레기의 경우
        //일단 여기로 넘어온 시점에서 생성은 해야한다는 뜻
        //생성할때 내부에 기타 다른 쓰레기가 있을것이기 때문에,

        // 우선 풀 생성, 내부에 있는 쓰레기들의 위치가 전부 같기때문에 그걸 배치하기 위해서라도
        //풀이 필요는 하다

        List<GameObject> innerTrashList = new List<GameObject>();
        for (int i = 1; i < infos.Count; i++)
        {
            if (infos[i].status == (int)TrashStatus.Clean)
            {
                continue; // 청소된 쓰레기는 제외
            }
            innerTrashList.Add(this.GetTrash(infos[i]));
        }

        foreach (GameObject trash in this.trashPool[infos[0].kind])
        {
            if (!trash.activeSelf)
            {
                trash.SetActive(true);
                

                // 그 다음 해초의 info를 할당해서 Init을 실행시킨다.
                trash.GetComponent<TrashData>().Info = infos[0]; // info를 넣어줌
                //그 후 해초 내부 코드에 이너 리스트 전달
                trash.GetComponent<GrassTiedTrash>().SetInnerTrash(innerTrashList);

                this.trashList.Add(trash); // 로딩중인 쓰레기 리스트에 추가
                return;
            }
        }
        CreateTrash(infos[0].kind);
        this.trashPool[infos[0].kind][trashPool[infos[0].kind].Count - 1].SetActive(true);
        

        // 그 다음 해초의 info를 할당해서 Init을 실행시킨다.
        this.trashPool[infos[0].kind][trashPool[infos[0].kind].Count - 1].GetComponent<TrashData>().Info = infos[0]; // info를 넣어줌

        //그 후 해초 내부 코드에 이너 리스트 전달
        this.trashPool[infos[0].kind][trashPool[infos[0].kind].Count - 1].GetComponent<GrassTiedTrash>().SetInnerTrash(innerTrashList);

        this.trashList.Add(this.trashPool[infos[0].kind][trashPool[infos[0].kind].Count - 1]); // 로딩중인 쓰래기 리스트에 추가

    }

    public void CreateTrash(int kind)
    {
        var obj = Instantiate(this.trashPrefabs[kind]); // kind값에 따라서 다른 프리팹을 생성
        if (!this.parentObjs.ContainsKey(kind))
        {
            var parent = new GameObject("TrashParent_" + kind); // 쓰레기 종류별로 부모 오브젝트 생성
            parentObjs.Add(kind, parent);
        }
        obj.transform.SetParent(this.parentObjs[kind].transform); // 부모 오브젝트에 자식으로 설정
        var trashData = obj.GetComponent<TrashData>();
        this.trashPool[kind].Add(obj);//풀에 오브젝트 생성하기
    }
    public void UnloadTrash(Vector2Int cell) // 셀값 받아서 해당 셀 언로드
    {
        var unloadTrash = this.trashList.FindAll(x =>
            cell == new Vector2Int(x.GetComponent<TrashData>().Info.cellX, x.GetComponent<TrashData>().Info.cellY) // 셀이 info에 있는 cell값과 같은것을 찾아서
        );
        foreach (var trash in unloadTrash)
        {
            trash.GetComponent<TrashData>().DisActivate(); // 해당 쓰레기 비활성화
            this.trashList.Remove(trash); // 로딩중인 쓰래기 리스트에서 제거
        }
    }
}
