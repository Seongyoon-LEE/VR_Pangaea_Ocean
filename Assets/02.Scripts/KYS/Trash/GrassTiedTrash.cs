using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTiedTrash : TrashData
{
    private Transform grassObj;
    private List<GameObject> innerTrash;

    protected override void Init()
    {
        this.grassObj = this.transform.GetChild(0);
        this.transform.position = new Vector3(this.Info.posX, this.Info.posY, this.Info.posZ);
        this.grassObj.localScale = new Vector3(1, this.Info.height * 2, 1);
    }
    public void SetInnerTrash(List<GameObject> trashes)
    {
        this.innerTrash = trashes;
        foreach(var trash in trashes)
        {
            var x = Random.Range(-0.5f, 0.5f); // -0.5 ~ 0.5 사이의 랜덤값
            var y = Random.Range(-0.5f, 0.5f); // -0.5 ~ 0.5 사이의 랜덤값
            var z = Random.Range(-0.5f, 0.5f); // -0.5 ~ 0.5 사이의 랜덤값

            trash.transform.Translate(x, this.Info.height + y, z); // 높이값을 적용
        }
        if (this.Info.status == (int)TrashStatus.Damaged)
        {
            this.grassObj.gameObject.SetActive(false); // 손상된 상태라면 풀 오브젝트를 비활성화
        }
        else
        {
            this.grassObj.gameObject.SetActive(true); // 손상되지 않았다면 풀 오브젝트를 활성화
            foreach (var trash in trashes)
            {
                trash.layer = 9; // 묶인 쓰레기 레이어로 설정
            }
        }
        // 내부 쓰레기들의 청소상태를 체크해서 전부 청소 됐다면 이 쓰레기도 clean상태로 변경
        // 라는 내용의 코루틴 시작 시키고, 청소 다 됐으면 코루틴 종료,
        // 비활성화될때 코루틴 종료
        StartCoroutine(CleanCheckRoutine());
    }
    IEnumerator CleanCheckRoutine()
    {
        while (this.innerTrash == null)
        {
            yield return null;
        }
        while(this.innerTrash.Find(x => x.GetComponent<TrashData>().Info.status != (int)TrashStatus.Clean) != null)
        {
            yield return null;
        }
        // 모든 내부 쓰레기가 청소되었다면
        this.Info.status = (int)TrashStatus.Clean; // 이 쓰레기도 청소 상태로 변경
    }
    public void GrassCut()
    {
        // 풀을 벴을때 내부에 있는 쓰래기의 레이어를 바꾸는 로직
        
        this.grassObj.gameObject.SetActive(false); // 풀 오브젝트를 비활성화
        DataManager.Instance.dicTrash[new Vector2Int(this.Info.cellX, this.Info.cellY)]
            .Find(x => x.id == this.Info.id).status = (int)TrashStatus.Damaged; // 쓰레기 상태를 손상으로 변경
        foreach (var trash in this.innerTrash)
        {
            trash.layer = 6; // 일반 쓰레기 레이어로 변경
        }
    }
    public override void DisActivate()
    {
        //비활성화될때 레이어를 다시 원래대로 바꿔줘서 오브젝트 풀에 이상이 없게함
        foreach (var trash in this.innerTrash)
        {
            trash.layer = 6; // 일반 쓰레기 레이어로 변경
        }
        base.DisActivate();
    }
}
