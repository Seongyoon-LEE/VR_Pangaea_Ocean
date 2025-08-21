using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTiedTrash : TrashData
{
    private Transform grassObj;
    private Transform trashObj;

    public override void Init()
    {
        this.grassObj = this.transform.GetChild(0);
        this.trashObj = this.transform.GetChild(1);

        this.grassObj.localScale = new Vector3(1, this.Info.height, 1);
        this.trashObj.localPosition = new Vector3(0, this.Info.height, 0);
        if (this.Info.status == (int)TrashStatus.Damaged)
        {
            this.grassObj.gameObject.SetActive(false); // 손상된 상태라면 풀 오브젝트를 비활성화
            this.trashObj.gameObject.layer = 6; // 쓰레기 레이어로 변경
        }
    }
}
