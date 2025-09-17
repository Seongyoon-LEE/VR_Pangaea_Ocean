using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Ray mouseRay;
    private PlayerData _playerData; // 플레이어 데이터
    public PlayerData PlayerData
    {
        get
        {
            return this._playerData;
        }
        set
        {
            this._playerData = value;
            this.transform.position = new Vector3(value.posX, value.posY, value.posZ);
            this.transform.rotation = Quaternion.Euler(value.rotX, value.rotY, value.rotZ);
        }
    }
    private IEnumerator Start()
    {
        while (!DataManager.Instance.IsPlayerLoading) // 데이터 매니저가 로딩이 끝날 때까지 대기
        {
            yield return null;
        }
        if (DataManager.Instance.IsPlayerDataExist) // 플레이어 데이터가 존재한다면
        {
            this.PlayerData = DataManager.Instance.PlayerData; // 플레이어 데이터를 할당
        }
        else // 플레이어 데이터가 없다면
        {
            this.PlayerData = new PlayerData(); // 새로운 플레이어 데이터 생성
            DataManager.Instance.PlayerData = this.PlayerData; // 데이터 매니저에 플레이어 데이터 할당
        }
    }
    //레이로 쓰래기 클릭 시 흡수
    private void Update()
    {
        this.mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(mouseRay.origin, mouseRay.direction * 100, Color.red);

        /*if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭(테스트용)
        {
            RaycastHit hit;
            if (Physics.Raycast(mouseRay, out hit, 100f, 1 << 6)) // 6 : 쓰레기 레이어
            {
                TrashData trashData = hit.collider.GetComponent<TrashData>();
                if (trashData != null)
                {
                    this.PlayerData.trashIdList.Add(trashData.Info.id,new Vector2Int(trashData.Info.cellX , trashData.Info.cellY)); 
                    // 쓰레기 ID를 리스트에 추가, 플레이어 사망 시 원래대로 돌려놓음
                    this.PlayerData.weight += trashData.Info.weight; // 가방 용량에 추가
                    trashData.Clean(); // 쓰레기 청소 함수 호출
                    trashData.DisActivate();
                }
            }
            else if(Physics.Raycast(mouseRay, out hit, 100f, 1 << 8)) // 8 : 큰 쓰레기 레이어
            {
                var breakableTrash = hit.collider.GetComponent<BreakableTrash>();
                if(breakableTrash != null)
                {
                    breakableTrash.Break(); // 쓰레기 파괴 함수 호출
                }
            }
            else if(Physics.Raycast(mouseRay, out hit, 100f, 1 << 10)) // 10 : 풀 레이어
            {
                var grassTiedTrash = hit.collider.transform.GetComponentInParent<GrassTiedTrash>();
                if (grassTiedTrash != null)
                {
                    grassTiedTrash.GrassCut(); // 풀 벨 때 함수 호출
                }
            } 
        }*/
    }
    public void PlayerDie()
    {
        var boat = GameObject.Find("Boat").transform;
        boat.GetChild(1).GetComponent<BoatBoarding>().BoardingBtn();
        // 플레이어가 죽었을 때의 로직
        //DataManager.Instance.PlayerData.isBoarding = true;
        //this.transform.position = boat.position + new Vector3(0, 1f, -2f); // 플레이어 위치 초기화
        //this.transform.rotation = boat.rotation; // 플레이어 회전 초기화

        // 저장된 거북이 데이터 초기화
        GameManager.Instance.puzzleDicReset("Turtle");
        // 적용된 버프 초기화
        GameManager.Instance.BuffApp();

        foreach (var trash in this.PlayerData.trashIdList)
        {
            DataManager.Instance.dicTrash[trash.Value].Find(x => x.id == trash.Key).status = (int)TrashStatus.Dirty; // 쓰레기 상태를 더러움으로 변경
        }
        this.PlayerData.trashIdList.Clear(); // 쓰레기 ID 리스트 초기화
        this.PlayerData.weight = 0; // 가방 용량 초기화
        this.PlayerData.oxygen = 100; // 산소 초기화 (테스트용)
    }
    public void PlayerPosSave() // 플레이어 위치 저장용
    {
        DataManager.Instance.PlayerData.rotX = this.transform.rotation.eulerAngles.x;
        DataManager.Instance.PlayerData.rotY = this.transform.rotation.eulerAngles.y;
        DataManager.Instance.PlayerData.rotZ = this.transform.rotation.eulerAngles.z;
        DataManager.Instance.PlayerData.posX = this.transform.position.x;
        DataManager.Instance.PlayerData.posY = this.transform.position.y;
        DataManager.Instance.PlayerData.posZ = this.transform.position.z;
        DataManager.Instance.PlayerDataSaved = true;
    }
    private void OnApplicationQuit()
    {
        this.PlayerPosSave();
        DataManager.Instance.SaveData();
    }
}