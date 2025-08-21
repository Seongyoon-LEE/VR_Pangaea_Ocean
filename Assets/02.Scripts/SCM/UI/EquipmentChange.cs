using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class EquipmentChange : MonoBehaviour
{
    public GameObject[] equipmentList; // 드래그 앤 드롭


    public void EquipmentSelect(int select)
    {
        for (int i = 0; i < equipmentList.Length - 1; i++)
        {
            if (i == select) equipmentList[i].SetActive(true);
            else equipmentList[i].SetActive(false);

            // Hand 왼손, 오른손 같이 활성, 비활성화
            equipmentList[equipmentList.Length - 1].SetActive(equipmentList[0].activeSelf);
        }
        GetComponent<MenuEnable>().Close();
    }

}
