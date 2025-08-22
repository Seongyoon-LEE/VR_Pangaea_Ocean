using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BoatCockpit : ShowCanvas
{
    private readonly string playerTag = "Player";
    private Transform playerTr;
    private Transform cockpitPos;
    public GameObject[] equipmentList; // 드래그 앤 드롭
    public int curEquipment;
    public bool isCockpit = false;
    protected override void Start()
    {
        base.Start();
        playerTr = GameObject.FindWithTag(playerTag).transform;
        cockpitPos = transform.parent.GetChild(4).transform;
    }

    void Update()
    {
        FollowUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            UIEnable(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            UIEnable(false);
        }
    }

    // 보트 탑승 버튼 
    public void BoatControll()
    {
        // 현재 장착 중인 장비 인덱스 값 저장
        for (int i = 0; i < equipmentList.Length; i++)
        {
            if (equipmentList[i].activeSelf)
            {
                curEquipment = i;
                break;
            }
        }

        PlayerEnable(false);
    }
    // 플레이어 이동
    public void PlayerEnable(bool isSetting)
    {
        isCockpit = !isSetting;
        playerTr.position = isSetting ? transform.position : cockpitPos.position; // 위치
        Quaternion rot = cockpitPos.rotation * Quaternion.Euler(0f, 180f, 0f);
        playerTr.rotation = isSetting ? rot : cockpitPos.rotation; // 회전 - 후방 : 전방
        playerTr.GetComponent<ContinuousMoveProviderBase>().enabled = isSetting;
        playerTr.GetComponent<ContinuousTurnProviderBase>().enabled = isSetting;
        playerTr.parent = isSetting ? null : transform.parent;

        equipmentList[curEquipment].SetActive(isSetting);
        if (curEquipment == 0) equipmentList[4].SetActive(isSetting);
    }

}
