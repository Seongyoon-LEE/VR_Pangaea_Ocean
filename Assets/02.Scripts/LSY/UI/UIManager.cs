using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject garbageUIObj; // 쓰레기 UI 오브젝트

    void Update()
    {
        if(garbageUIObj != null)
        {
            // 보트 탑승 상태와 UI의 활성화 상태가 다르면, 상태를 동기화
            if (garbageUIObj.activeSelf != DataManager.Instance.PlayerData.isBoarding)
                garbageUIObj.SetActive(DataManager.Instance.PlayerData.isBoarding);
        }
    }
}
