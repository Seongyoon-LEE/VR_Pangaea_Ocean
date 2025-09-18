using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class BoatBoarding : ShowCanvas
{
    private readonly string playerTag = "Player";
    [SerializeField] private Transform playerTr;
    [SerializeField] private GameObject trashBoard;
    protected override IEnumerator Start()
    {
        playerTr = GameObject.FindWithTag(playerTag).transform;
        canvas = GameObject.Find("Canvas_Boarding");
        canvas.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(BoardingBtn);
        canvas.transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(Close);
        trashBoard = GameObject.Find("GarbageCanvas");
        yield return base.Start();

        // 보트에 탑승중이지 않을 때 UI비활성화
        if (!DataManager.Instance.PlayerData.isBoarding)
            trashBoard.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            DataManager.Instance.PlayerData.isBoarding = false;
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
            DataManager.Instance.PlayerData.isBoarding = true;

            playerTr.position = transform.parent.position + new Vector3(-3f, 1f, 0f);
            Vector3 rot = transform.parent.localEulerAngles;
            rot.y -= 180f;
            playerTr.localEulerAngles = rot;
            if (trashBoard != null)
            {
                trashBoard.SetActive(true);
            }
     
            // 탑승 후 장비를 Hand모델로 변경
            for (int i = 0; i < equipmentsList.Count; i++)
            {
                //equipmentsList[i].SetActive(i == 0 || i == 1);
                equipmentsList[i].SetActive(false);
            }
        }
    }
}
