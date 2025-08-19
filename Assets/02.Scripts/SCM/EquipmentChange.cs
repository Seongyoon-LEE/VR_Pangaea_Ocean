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
    void Start()
    {
        canvas = GameObject.Find("Canvas_ChangUI").GetComponent<Canvas>().gameObject;
        canvas.SetActive(false);
        head = GameObject.Find("XR Origin (XR Rig)").transform.GetChild(0).GetChild(0).transform;
        // 자식수 보고 세팅하기
        ray = GameObject.Find("XR Origin (XR Rig)").transform.GetChild(0).GetChild(3).gameObject;
        ray.SetActive(false);
    }


    private void Update()
    {
        if (inputChange.action.WasPressedThisFrame())
        {
            UIEnable();

        }
        if (canvas.activeSelf)
        {
            canvas.transform.position = head.position + new Vector3(head.forward.x, head.forward.y, head.forward.z).normalized * spawnDistance;
            Quaternion rot = Quaternion.LookRotation(canvas.transform.position - head.position);
            canvas.transform.rotation = Quaternion.Slerp(canvas.transform.rotation, rot, 5f * Time.deltaTime);
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
        UIEnable();
    }

    public void CloseBtn()
    {
        UIEnable();
    }
    private void UIEnable()
    {
        canvas.SetActive(!canvas.activeSelf);
        ray.SetActive(!ray.activeSelf);
    }
}
