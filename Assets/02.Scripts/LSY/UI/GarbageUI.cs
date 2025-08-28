using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GarbageUI : MonoBehaviour
{
    [Header("Garbage UI")]
    int cleanTrashCount = 0; // 청소한 쓰레기 수
    int totalCnt = 0; // 전체 쓰레기 수
    [SerializeField] TMP_Text countText; // 청소한 쓰레기 수 텍스트
    [SerializeField] TMP_Text scoreText; // 점수 텍스트

    [Header("스크립트 참조")]
    [SerializeField] BoatBoarding boatBoarding; // 보트 탑승 스크립트
    [SerializeField] Garbage garbage; // 쓰레기통 스크립트
    private void OnEnable()
    {
        foreach (var trashs in DataManager.Instance.dicTrash)
        {
            cleanTrashCount += trashs.Value.Count(x => x.status == (int)TrashStatus.Clean);
            totalCnt += trashs.Value.Count();
        }

        countText.text = $"{cleanTrashCount} / {totalCnt}";
        scoreText.text = $"{DataManager.Instance.playerData.weight}";
    }
    void Start()
    {
        boatBoarding = GameObject.FindObjectOfType<BoatBoarding>();
        garbage = GameObject.FindObjectOfType<Garbage>();
        // 제출이 끝나면 UI 새로고침 하라는 방송을 듣고 UpdateUI 함수 실행
        //garbage.onTrashSubmitted += UpdateUI;

        //// 처음 켰을 때 UI 새로고침
        //SetActiveUI(boatBoarding)
        //UpdateUI();
    }

    void Update()
    {
        
    }
}
