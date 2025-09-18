using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
public class InventorySocket : XRSocketInteractor
{
    private readonly string targetTag = "Key";
    private Image keyImg;
    private int maxEquipment = 7;

    protected override void Start()
    {
        base.Start();
        keyImg = transform.GetChild(0).GetChild(0).GetChild(5).GetComponent<Image>();
    }

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.transform.CompareTag(targetTag);
    }
    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.transform.CompareTag(targetTag);
    }

    public void SocketKey(SelectEnterEventArgs key)
    {
        var keyTr = key.interactableObject.transform;
        if (keyTr.CompareTag(targetTag)) // 소켓에 들오건것이 Key일 경우
        {
            var menu = transform.parent.GetComponent<MenuEnable>();
            if (menu.GetEquipments().Count < maxEquipment)
            {
                keyTr.gameObject.SetActive(false); // 소켓에 들어오면 비활성화
                menu.SetEquipment(keyTr.gameObject); // 장비 그룹 리스트에 키 넣기
                keyTr.parent = menu.equipments; // 장비 그룹에 오브젝트 위치 이동
                keyImg.color = Color.white; // UI 색 변경
            }
        }
    }

}
