using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject garbageUIObj; // 쓰레기 UI 오브젝트
    [SerializeField] BoatBoarding boatBoarding; // 보트 탑승 스크립트
    void Start()
    {
        boatBoarding = GameObject.FindObjectOfType<BoatBoarding>(true);

    }

    void Update()
    {
        if(garbageUIObj != null && boatBoarding != null)
        {
            print(boatBoarding.isBoarding);
            // 보트 탑승 상태와 UI의 활성화 상태가 다르면, 상태를 동기화
            if (garbageUIObj.activeSelf != boatBoarding.isBoarding)
                garbageUIObj.SetActive(boatBoarding.isBoarding);
        }
    }
}
