using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Ray mouseRay;
    public List<int> trashIdList = new List<int>(); // 쓰레기 ID를 저장하는 리스트
    //레이로 쓰래기 클릭 시 흡수
    private void Update()
    {
        this.mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(mouseRay.origin, mouseRay.direction * 100, Color.red);

        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭
        {
            RaycastHit hit;
            if (Physics.Raycast(mouseRay, out hit, 100f, 1 << 6)) // 6 : 쓰레기 레이어
            {
                TrashData trashData = hit.collider.GetComponent<TrashData>();
                if (trashData != null)
                {
                    trashData.Info.status = (int)TrashStatus.Clean;
                    this.trashIdList.Add(trashData.Info.id); // 쓰레기 ID를 리스트에 추가, 플레이어 사망 시 원래대로 돌려놓음
                    trashData.Clean(); // 쓰레기 청소 함수 호출
                    trashData.gameObject.SetActive(false);
                }
            }
        }
    }
}