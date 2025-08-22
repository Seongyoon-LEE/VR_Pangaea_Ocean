using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BoatBoarding : ShowCanvas
{
    private readonly string playerTag = "Player";
    private Transform playerTr;
    protected override void Start()
    {
        base.Start();
        playerTr = GameObject.FindWithTag(playerTag).transform;
    }

    private void Update()
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
    public void BoardingBtn()
    {
        if (playerTr != null)
        {
            playerTr.position = transform.parent.position + new Vector3(0, 1f, -2f);
            
        }
    }
}
