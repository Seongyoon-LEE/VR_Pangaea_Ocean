using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class EquipmentChange : MonoBehaviour
{
    public XRBaseInteractable[] equipmentList;
    public Image[] equipmentImage;
    public InputActionProperty inputChange;
    private int changeNum = 0;
    private Canvas canvas;
    void Start()
    {
        canvas = GameObject.Find("Canvas_ChangUI").GetComponent<Canvas>(); ;
        canvas.gameObject.SetActive(false);

    }


    private void Update()
    {
        if (inputChange.action.WasPressedThisFrame())
        {
            StartCoroutine(ChangeUI());
            equipmentList[changeNum++].gameObject.SetActive(false);
            if (changeNum == equipmentList.Length) changeNum = 0;
            equipmentList[changeNum].gameObject.SetActive(true);
        }
    }


    IEnumerator ChangeUI()
    {
        canvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        canvas.gameObject.SetActive(false);
    }
}
