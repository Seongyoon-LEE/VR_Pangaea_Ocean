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
        //GameManager.Instance.aliveEnemyCnt++;
        //풀에서 적 가져오기
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
        trashData.cleanAction += () =>
        {
            //여기에 비활성화시 데이터 건드리는 부분 작성
            DataManager.Instance.dicTrash[new Vector2Int(trashData.Info.cellX, trashData.Info.cellY)]
            .Find(x => x.id == trashData.Info.id).status = (int)TrashStatus.Clean; // 쓰레기 상태를 청소로 변경
            Debug.Log("청소됨");
        };
        this.trashPool[kind].Add(obj);//풀에 오브젝트 생성하기
    }
    public void UnloadTrash(Vector2Int cell) // 셀값 받아서 해당 셀 언로드
    {
        var unloadTrash = this.trashList.FindAll(x =>
            cell == new Vector2Int(x.GetComponent<TrashData>().Info.cellX, x.GetComponent<TrashData>().Info.cellY) // 셀이 info에 있는 cell값과 같은것을 찾아서
        );
        foreach (var trash in unloadTrash)
        {
            trash.SetActive(false); // 해당 쓰레기 비활성화
            this.trashList.Remove(trash); // 로딩중인 쓰래기 리스트에서 제거
        }
    }
}
