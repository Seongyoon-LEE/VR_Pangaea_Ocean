using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
public class EquipmentChange : MonoBehaviour
{
    public GameObject[] equipmentList;
    public InputActionProperty inputChange;
    private GameObject canvas;
    private Transform head;
    private GameObject ray;
    private float spawnDistance = 2f;
    private int selectNum = 0;
    void Start()
    {
        canvas = GameObject.Find("Canvas_ChangUI").GetComponent<Canvas>().gameObject;
        canvas.SetActive(false);
        head = GameObject.Find("XR Origin (XR Rig)").transform.GetChild(0).transform;
        // 자식수 보고 세팅하기
        ray = GameObject.Find("XR Origin (XR Rig)").transform.GetChild(0).GetChild(3).gameObject;
        ray.SetActive(false);
    }


    private void Update()
    {
        if (inputChange.action.WasPressedThisFrame())
        {
            UIEnable();
            canvas.transform.position = head.position + new Vector3(head.forward.x, 0f, head.forward.z).normalized * spawnDistance;

            for (int i = 0; i < equipmentList.Length - 1; i++)
            {
                if (equipmentList[i].activeSelf)
                    selectNum = i;
            }
        }
    }

    public void EquipmentSelect(int select)
    {
        for (int i = 0; i < equipmentList.Length - 1; i++)
        {
            if (i == select) equipmentList[i].SetActive(true);
            else equipmentList[i].SetActive(false);

            equipmentList[equipmentList.Length - 1].SetActive(equipmentList[0].activeSelf);
        }
    }

    public void CloseBtn()
    {
        UIEnable();
        EquipmentSelect(selectNum);
    }
    private void UIEnable()
    {
        canvas.SetActive(!canvas.activeSelf);
        ray.SetActive(!ray.activeSelf);
    }
}
