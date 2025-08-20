using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTiedTrash : MonoBehaviour
{
    private Transform grassObj;
    private Transform trashObj;

    public float grassHeight;

    private void Update() //테스트라서 업데이트에서 높이 조절중
                          //실제로는 info값 넣을때 Init에서 설정
    {
        this.Init(); // 테스트
    }
    public void Init()
    {
        this.grassObj = this.transform.GetChild(0);
        this.trashObj = this.transform.GetChild(1);

        this.grassObj.localScale = new Vector3(1, this.grassHeight, 1);
        this.trashObj.localPosition = new Vector3(0, this.grassHeight, 0);
    }
}
