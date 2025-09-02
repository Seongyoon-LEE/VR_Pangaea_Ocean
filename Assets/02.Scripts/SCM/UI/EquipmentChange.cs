using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentChange : MonoBehaviour
{
    public List<GameObject> equipmentList; // 드래그 앤 드롭


    public void EquipmentSelect(int select)
    {
        if (select > equipmentList.Count - 1) return;
        for (int i = 1; i < equipmentList.Count; i++)
        {
            equipmentList[i].SetActive(i == select);
        }
        // Hand 왼손, 오른손 같이 활성, 비활성화
        equipmentList[0].SetActive(equipmentList[1].activeSelf);
        GetComponent<MenuEnable>().Close();
    }

}
