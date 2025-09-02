using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Garbage : MonoBehaviour
{
    [SerializeField] GameObject trashPileObj; // 쓰레기 더미 오브젝트
    Vector3 initPos; // 초기 위치 저장 변수 y값 0.0
    public Action onTrashSubmitted; // 쓰레기 제출시 실행할 델리게이트
    [SerializeField] OutLineCtrl outLineCtrl;
    IEnumerator Start()
    {
        if (trashPileObj != null)
            initPos = trashPileObj.transform.localPosition; // 초기 위치 저장
        if(outLineCtrl == null)
            outLineCtrl = GetComponent<OutLineCtrl>();

        while (!DataManager.Instance.IsLoadingFinish)
        {
            yield return null;
        }

        // 시작할때 한번 현재 상태에 맞게 쓰레기통 모습 업데이트
        UpdateTrashVisuals();

    }

    // 쓰레기 제출시 호출되는 함수
    public void SubmitCollectedTrash()
    {
        float collectedWeight = DataManager.Instance.playerData.weight;
        if (collectedWeight <= 0)
        {
            Debug.Log("제출할 쓰레기가 없습니다.");
            return;
        }
        int scoreToAdd = Mathf.RoundToInt(collectedWeight); // 현재 무게를 점수로 환산 (1:1 비율)
        DataManager.Instance.playerData.score += scoreToAdd; // 점수 추가
        DataManager.Instance.playerData.weight = 0; // 무게 초기화
        DataManager.Instance.playerData.trashIdList.Clear(); // 수집한 쓰레기 ID 리스트 초기화

        Debug.Log($"쓰레기 제출 완료! {scoreToAdd}점 획득! (총 점수: {DataManager.Instance.playerData.score})");
        outLineCtrl?.OffOutline();
        UpdateTrashVisuals();
        onTrashSubmitted?.Invoke();
    }
    public void UpdateTrashVisuals()
    {
        if(trashPileObj == null) return;
        int cleanTrashCount = 0;
        int totalCnt = 0;

        foreach (var trashs in DataManager.Instance.dicTrash)
        {
            cleanTrashCount += trashs.Value.Count(x => x.status == (int)TrashStatus.Clean);
            totalCnt += trashs.Value.Count();
        }
        if (totalCnt == 0)
        {
            trashPileObj.SetActive(false);
            return;
        }
        float percentage = (float)cleanTrashCount / totalCnt;

        // 쓰레기 더미의 y 위치를 청소한 쓰레기 비율에 따라 조정
        if (percentage >= 0.8f)
        {
            SetTrashState(true, 0.6f); // 80% 이상 청소 시 가장 높은 상태

        }
        else if (percentage >= 0.6f)
        {
            SetTrashState(true, 0.3f); // 60% 이상 청소 시 중간 상태
        }
        else if (percentage >= 0.3f)
        {
            SetTrashState(true, 0.0f); // 30% 이상 청소 시 낮은 상태
        }
        else
        {
            SetTrashState(false, 0.0f); // 청소한 쓰레기 없을 시 숨김
        }
    }
    void SetTrashState(bool isActive, float heightOffset)
    {
        trashPileObj.SetActive(isActive); // 활성화 비활성화
        if (isActive)
        {
            // 기존 위치를 기준으로 높이를 더하여 위치 조정
            Vector3 newPos = initPos + new Vector3(0, heightOffset, 0);
            trashPileObj.transform.localPosition = newPos;
        }
    }
}
