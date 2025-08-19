using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public GameObject trashPrefab;

    List<List<GameObject>> trashPool; // 쓰래기 종류별로(List 하나)
    Dictionary<int, GameObject> parentObjs = new Dictionary<int, GameObject>(); // 쓰레기 종류별로 부모 오브젝트를 관리하는 딕셔너리
    //종류별로 쓰레기(List 둘)
    List<GameObject> trashList; // 로딩중인 쓰래기 전체를 담는 리스트 (언로드 할때 필요함)

    public void Init()
    {
        //start에서 풀 생성
        this.trashPool = new List<List<GameObject>>();
        this.trashList = new List<GameObject>();
        for (int i = 0; i < 3; i++) // 종류가 다 정해지고 난 다음 저 3을 고쳐야한다
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
                trash.SetActive(true);
                trash.GetComponent<TrashData>().Info = info; // info를 넣어줌
                this.trashList.Add(trash); // 로딩중인 쓰래기 리스트에 추가
                return trash;
            }
        }
        //여기로 나왔다는건 모든 풀이 활성화
        //새로 생성 후 그걸 반환
        CreateObject(info.kind);
        this.trashPool[info.kind][trashPool[info.kind].Count - 1].GetComponent<TrashData>().Info = info;
        this.trashPool[info.kind][trashPool[info.kind].Count - 1].SetActive(true);
        this.trashList.Add(this.trashPool[info.kind][trashPool[info.kind].Count - 1]); // 로딩중인 쓰래기 리스트에 추가
        return this.trashPool[info.kind][trashPool[info.kind].Count - 1];
    }

    public void CreateObject(int kind)
    {
        var obj = Instantiate(this.trashPrefab); // kind값에 따라서 다른 프리팹을 생성하도록 변경 예정
        if (!this.parentObjs.ContainsKey(kind))
        {
            var parent = new GameObject("TrashParent_" + kind); // 쓰레기 종류별로 부모 오브젝트 생성
            parentObjs.Add(kind, parent);
        }
        obj.transform.SetParent(this.parentObjs[kind].transform); // 부모 오브젝트에 자식으로 설정
        obj.SetActive(false);
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
