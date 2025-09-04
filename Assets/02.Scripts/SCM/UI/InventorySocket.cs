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

    protected override void Start()
    {
        base.Start();
        keyImg = transform.GetChild(0).GetChild(5).GetComponent<Image>();
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
            keyTr.gameObject.SetActive(false);
            var menu = transform.parent.GetComponent<MenuEnable>();
            menu.SetEquipment(keyTr.gameObject);
            keyTr.parent = menu.equipments;
            keyImg.color = Color.white;
        }
    }
}
