using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBoarding : ShowCanvas
{
    private readonly string boatTag = "Boat";
    private Transform boatTr;
    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        FollowUI();
    }

    protected override void FollowUI()
    {
        base.FollowUI();
    }

    protected override void UIEnable()
    {
        base.UIEnable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(boatTag))
        {
            UIEnable();
            boatTr = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(boatTag))
        {
            UIEnable();
            boatTr = null;
        }
    }

    public override void Close()
    {
        base.Close();
    }

    public void BoardingBtn()
    {
        if (boatTr != null)
        {
            UIEnable();
            transform.position = boatTr.position + new Vector3(0, 1f, -2f);
        }
    }

}
