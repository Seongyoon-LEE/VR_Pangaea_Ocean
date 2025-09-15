using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BortExitButton : MonoBehaviour
{
    [SerializeField] Transform boatTr; // 보트 트랜스폼
    [SerializeField] Transform playerTr; // 플레이어 트랜스폼
    [SerializeField] GameObject garbageUIObj; // 쓰레기 UI 오브젝트
    [SerializeField] GameObject rightRay; // 오른손 레이 오브젝트
    Button button;

    [SerializeField] CharacterController playerController;
    void Start()
    {
        button = GetComponent<Button>();
        if(playerTr == null)
            playerTr = GameObject.FindWithTag("Player").transform;

        if (playerController == null && playerTr != null)
            playerController = playerTr.GetComponent<CharacterController>();

        if (rightRay == null)
            rightRay = GameObject.Find("Ray Interactor");

        button.onClick.AddListener(() => // 버튼 클릭 시
        {
            // 캐릭터 컨트롤러(오토파일럿) 잠시 비활성화
            if (playerController != null)
                playerController.enabled = false;

            playerTr.position = boatTr.position + boatTr.forward * 4;

            // 위치 이동후 캐릭터 컨트롤러 재활성화
            if (playerController != null)
                playerController.enabled = true;

            print($"asd{playerTr.position}" );
            print($"보트탑승 {boatTr.position}");
            DataManager.Instance.PlayerData.isBoarding = false; // 탑승 상태 해제
            print($"asd{playerTr.position}");
            print($"보트탑승 {boatTr.position}");
            garbageUIObj.SetActive(false); // 쓰레기 UI 비활성화
            rightRay.SetActive(false); // 오른손 레이 비활성화
        });
    }
}
