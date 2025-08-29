using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class BoatCockpit : ShowCanvas
{
    private readonly string playerTag = "Player";
    private Transform playerTr;
    private Transform cockpitPos;
    public List<GameObject> equipmentList; // 드래그 앤 드롭
    public int curEquipment;
    public bool isCockpit = false;
    protected override void Start()
    {
        playerTr = GameObject.FindWithTag(playerTag).transform;
        cockpitPos = transform.parent.GetChild(4).transform;
        canvas = GameObject.Find("Canvas_BoatCtrl").gameObject;
        canvas.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(BoatControll);
        canvas.transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(Close);
        base.Start();
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
        curEquipment = equipmentList.FindIndex(e => e.activeSelf);
        curEquipment = curEquipment != -1 ? curEquipment : 4;


        PlayerEnable(false);
    }
    
    // 플레이어 이동
    public void PlayerEnable(bool isSetting)
    {
        isCockpit = !isSetting;
        playerTr.position = isSetting ? transform.position : cockpitPos.position; // 위치
        Quaternion rot = cockpitPos.rotation * Quaternion.Euler(0f, 180f, 0f); // 방향 설정
        playerTr.rotation = isSetting ? rot : cockpitPos.rotation; // 회전 - 후방 : 전방
        playerTr.GetComponent<ContinuousMoveProviderBase>().enabled = isSetting;
        playerTr.GetComponent<ContinuousTurnProviderBase>().enabled = isSetting;
        playerTr.parent = isSetting ? null : transform.parent;
        equipmentList[curEquipment].SetActive(isSetting);
        if (curEquipment == 0) equipmentList[4].SetActive(isSetting);
    }

}
