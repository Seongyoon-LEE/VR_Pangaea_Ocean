using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentChange : MonoBehaviour
{
    public List<GameObject> equipmentList = new List<GameObject>();

    private void Start()
    {
        var menu = transform.GetComponent<MenuEnable>();
        equipmentList = menu.GetEquipments();
    }
    public void EquipmentSelect(int select) // select 값은 1 ~ 6
    {
        if (select > equipmentList.Count + 1) return; // 비어있다면 return
        for (int i = 0; i < equipmentList.Count; i++)
        {
            if (select == 1) break;
            equipmentList[i].SetActive(i + 2 == select);
        }
        // Hand 왼손, 오른손 같이 활성, 비활성화
        //equipmentList[0].SetActive(equipmentList[1].activeSelf);
        GetComponent<MenuEnable>().Close(); // UI 종료

    }

}
