using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BortExitButton : MonoBehaviour
{
    [SerializeField] Transform bortTr; // 보트 트랜스폼
    [SerializeField] Transform playerTr; // 플레이어 트랜스폼
    [SerializeField] GameObject garbageUIObj; // 쓰레기 UI 오브젝트
    [SerializeField] GameObject rightRay; // 오른손 레이 오브젝트
    Button button;
    void Start()
    {
        button = GetComponent<Button>();
        if(playerTr == null)
            playerTr = GameObject.FindWithTag("Player").transform;

        button.onClick.AddListener(() => // 버튼 클릭 시
        {
            playerTr.position = bortTr.position + Vector3.forward * 4; // 플레이어 위치를 쓰레기통 앞쪽으로 이동
            DataManager.Instance.PlayerData.isBoarding = false; // 탑승 상태 해제
            garbageUIObj.SetActive(false); // 쓰레기 UI 비활성화
            rightRay.SetActive(false); // 오른손 레이 비활성화
        });
    }

    void Update()
    {
        
    }
}
