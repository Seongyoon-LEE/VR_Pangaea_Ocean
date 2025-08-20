using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCtrl : ShowCanvas
{
    private readonly string playerTag = "Player";
    private Transform cameraOffset;
    private Vector3 offset = new Vector3(0, 1.5f, 3f);
    public GameObject[] equipmentList; // 드래그 앤 드롭
    protected override void Start()
    {
        base.Start();
        cameraOffset = GameObject.Find("Camera Offset").transform;
    }

    void Update()
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
        if (other.CompareTag(playerTag))
        {
            UIEnable();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            UIEnable();
        }
    }
    public override void Close()
    {
        base.Close();
    }

    public void BoatControll()
    {
        UIEnable();
        cameraOffset.position += offset;
    }

}
