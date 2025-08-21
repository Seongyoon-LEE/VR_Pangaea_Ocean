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

    void Update()
    {
        FollowUI();
        
    }
    protected override void FollowUI()
    {
        base.FollowUI();
    }

    protected override void UIEnable(bool isEnable)
    {
        base.UIEnable(isEnable);
    }
    public override void Close()
    {
        base.Close();
    }

    public void GetOff()
    {
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
