using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GarbageUI : MonoBehaviour
{
   int cleanTrashCount = 0;
    int totalCnt = 0;
    [SerializeField] TMP_Text countText;
    [SerializeField] TMP_Text scoreText;
    private void OnEnable()
    {
        foreach (var trashs in DataManager.Instance.dicTrash)
        {
            cleanTrashCount += trashs.Value.Count(x => x.status == (int)TrashStatus.Clean);
            totalCnt += trashs.Value.Count();
        }

        countText.text = $"{cleanTrashCount} / {totalCnt}";
        scoreText.text = $"{DataManager.Instance.PlayerData.weight}";
    }
    void Start()
    {
    }

    void Update()
    {
        
    }
}
