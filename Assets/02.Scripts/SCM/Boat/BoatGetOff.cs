using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatGetOff : ShowCanvas
{
    private readonly string playerTag = "Player";
    private Transform playerTr;

    protected override void Start()
    {
        playerTr = GameObject.FindWithTag(playerTag).transform;
        canvas = GameObject.Find("Canvas_GetOff").gameObject;
        canvas.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(GetOff);
        canvas.transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(Close);
        base.Start();
    }

    // 버튼 이벤트
    public void GetOff()
    {
        // 바라보고 있는 방향 유지 한채 이동
        Vector3 dir = (playerTr.position - transform.parent.position).normalized;
        playerTr.position += dir * 4f;
        DataManager.Instance.playerData.isBoarding = false;
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


}
