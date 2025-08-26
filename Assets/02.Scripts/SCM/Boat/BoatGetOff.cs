using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatGetOff : ShowCanvas
{
    private readonly string playerTag = "Player";
    private Transform playerTr;

    protected override void Start()
    {
        base.Start();
        playerTr = GameObject.FindWithTag(playerTag).transform;
    }

    // 버튼 이벤트
    public void GetOff()
    {
        // 바라보고 있는 방향 유지 한채 이동
        Vector3 dir = (playerTr.position - transform.parent.position).normalized;
        playerTr.position += dir * 4f;
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
