using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentChange : MonoBehaviour
{
    public List<GameObject> equipmentList; // 드래그 앤 드롭


    public void EquipmentSelect(int select)
    {
        for (int i = 0; i < equipmentList.Count - 1; i++)
        {
            equipmentList[i].SetActive(i == select);

            // Hand 왼손, 오른손 같이 활성, 비활성화
        }
        equipmentList[equipmentList.Count - 1].SetActive(equipmentList[0].activeSelf);
        GetComponent<MenuEnable>().Close();
    }

}
