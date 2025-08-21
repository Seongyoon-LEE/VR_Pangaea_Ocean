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

    protected override void FollowUI()
    {
        base.FollowUI();
    }

    protected override void UIEnable(bool isEnable)
    {
        base.UIEnable(isEnable);
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

    public override void Close()
    {
        base.Close();
    }

    public void BoardingBtn()
    {
        if (playerTr != null)
        {
            playerTr.position = transform.parent.position + new Vector3(0, 1f, -2f);
            
        }
    }
}
