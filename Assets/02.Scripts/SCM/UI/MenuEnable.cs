using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 왼손에 사용되는 메뉴들
public class MenuEnable : ShowCanvas
{
    public InputActionProperty mainInput; // 사용하는 버튼 - 드래그 앤 드롭
    private Transform hand; // UI 위치할 위치
    protected override IEnumerator Start()
    {
        canvas = transform.GetChild(0).gameObject;
        hand = GameObject.Find("LeftHandUIPos").transform;
        yield return base.Start();
        
    }

    protected override void FollowUI()
    {
        if (canvas.activeSelf)
        {
            canvas.transform.position = hand.position + hand.up * 0.3f;
            //Quaternion rot = Quaternion.LookRotation(canvas.transform.position - hand.position);
            canvas.transform.rotation = hand.rotation * Quaternion.Euler(0, -90, 0);
        }
        //base.FollowUI();
    }

    private void OnEnable()
    {
        mainInput.action.started += x => UIEnable(!canvas.activeSelf, true);
    }
    private void OnDisable()
    {
        mainInput.action.started -= x => UIEnable(!canvas.activeSelf, true);
        mainInput.action.Disable();
    }

    public List<GameObject> GetEquipments()
    {
        return equipmentsList;
    }
    public void SetEquipment(GameObject item)
    {
        equipmentsList.Add(item);
    }
}
