using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject garbageUIObj; // 쓰레기 UI 오브젝트
    [SerializeField] GameObject dimImageCanvas; // 화면 가리개 캔버스
    private void Start()
    {
        this.dimImageCanvas.GetComponentInChildren<Image>().DOFade(0, 0.5f).OnComplete(() =>
        {
            Debug.Log("로딩 완료");
            this.dimImageCanvas.SetActive(false);
        });
    }
    void Update()
    {
        if(garbageUIObj != null)
        {
            // 보트 탑승 상태와 UI의 활성화 상태가 다르면, 상태를 동기화
            if (garbageUIObj.activeSelf != DataManager.Instance.PlayerData.isBoarding)
                garbageUIObj.SetActive(DataManager.Instance.PlayerData.isBoarding);
        }
    }
}
