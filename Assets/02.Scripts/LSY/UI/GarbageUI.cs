using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GarbageUI : MonoBehaviour
{
    [Header("Garbage UI")]
    int cleanTrashCount = 0; // û���� ������ ��
    int totalCnt = 0; // ��ü ������ ��   
    [SerializeField] TMP_Text countText; // û���� ������ �� �ؽ�Ʈ
    [SerializeField] TMP_Text scoreText; // ���� �ؽ�Ʈ

    [Header("��ũ��Ʈ ����")]
    [SerializeField] Garbage garbage; // �������� ��ũ��Ʈ
 
    void Start()
    {
        garbage = GameObject.FindObjectOfType<Garbage>(true);
        //������ ������ UI ���ΰ�ħ �϶�� ����� ��� UpdateUI �Լ� ����
        if(garbage != null)
            garbage.onTrashSubmitted += UpdateUI;
    }
    private void OnDestroy()
    {
        if (garbage != null)
            garbage.onTrashSubmitted -= UpdateUI;
    }
    private void OnEnable()
    {
        StartCoroutine(UiUpdateDelay());
    }
    private IEnumerator UiUpdateDelay()
    {
        while(DataManager.Instance == null)
        {
            yield return null;
        }
        while (!DataManager.Instance.IsLoadingFinish)
        {
            yield return null;
        }
        UpdateUI();
    }

    void Update()
    {
        //if (boatBoarding != null && gameObject.activeSelf != boatBoarding.isBoarding)
        //{
        //    this.gameObject.SetActive(boatBoarding.isBoarding);
        //}
    }
    private void UpdateUI()
    {
        cleanTrashCount = 0;
        totalCnt = 0;
        foreach (var trashs in DataManager.Instance.dicTrash)
        {
            cleanTrashCount += trashs.Value.Count(x => x.status == (int)TrashStatus.Clean);
            totalCnt += trashs.Value.Count();
        }

        countText.text = $"{cleanTrashCount} / {totalCnt}";
        scoreText.text = $"Total Score : {DataManager.Instance.PlayerData.score}";

    }
}
