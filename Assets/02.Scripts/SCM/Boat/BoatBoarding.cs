using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class BoatBoarding : ShowCanvas
{
    private readonly string playerTag = "Player";
    private Transform playerTr;
    private BoatCockpit hand;
    private GameObject trashBoard;
    protected override void Start()
    {
        playerTr = GameObject.FindWithTag(playerTag).transform;
        canvas = GameObject.Find("Canvas_Boarding");
        canvas.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(BoardingBtn);
        canvas.transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(Close);
        hand = transform.parent.transform.GetChild(2).GetComponent<BoatCockpit>();
        trashBoard = GameObject.Find("GarbageCanavs");
        base.Start();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            DataManager.Instance.playerData.isBoarding = false;
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
    public void BoardingBtn()
    {
        if (playerTr != null)
        {
            DataManager.Instance.playerData.isBoarding = true;
            playerTr.position = transform.parent.position + new Vector3(0, 1f, -2f);
            if (trashBoard != null) 
                trashBoard.SetActive(true);
            for (int i = 0; i < hand.equipmentList.Count; i++)
            {
                hand.equipmentList[i].SetActive(i == 0 || i == 4);
            }
        }
    }
}
