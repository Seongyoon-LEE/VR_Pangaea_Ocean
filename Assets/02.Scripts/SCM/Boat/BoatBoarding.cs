using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class BoatBoarding : ShowCanvas
{
    private readonly string playerTag = "Player";
    private Transform playerTr;
    private bool isBoarding = false;
    private BoatCockpit hand;
    protected override void Start()
    {
        base.Start();
        playerTr = GameObject.FindWithTag(playerTag).transform;
        canvas.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(BoardingBtn);
        canvas.transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(Close);
        hand = transform.parent.transform.GetChild(2).GetComponent<BoatCockpit>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isBoarding = false;
            UIEnable(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            UIEnable(false);
            ray.SetActive(isBoarding);
            isBoarding = false;
        }
    }

    // 보트 탑승 버튼
    public void BoardingBtn()
    {
        if (playerTr != null)
        {
            isBoarding = true;
            playerTr.position = transform.parent.position + new Vector3(0, 1f, -2f);
            for (int i = 0; i < hand.equipmentList.Length; i++)
            {
                if (i == 0 || i == 4)
                    hand.equipmentList[i].SetActive(true);
                else
                    hand.equipmentList[i].SetActive(false);
            }
        }
    }
}
