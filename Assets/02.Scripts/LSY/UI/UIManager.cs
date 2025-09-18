using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{ 
    [SerializeField] GameObject dimImageCanvas; // 화면 가리개 캔버스
    private void Start()
    {
        this.dimImageCanvas.GetComponentInChildren<Image>().DOFade(0, 2f).OnComplete(() =>
        {
            Debug.Log("로딩 완료");
            this.dimImageCanvas.SetActive(false);
        });
    }
}
