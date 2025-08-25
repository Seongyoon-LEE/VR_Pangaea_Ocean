using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrashCount : MonoBehaviour
{
    [SerializeField] TMP_Text trashCountText;
    [SerializeField]
     VacuumCleaner vacuumCleaner;

    void Start()
    {
        trashCountText = transform.GetChild(0).GetComponent<TMP_Text>();
        vacuumCleaner = FindObjectOfType<VacuumCleaner>(true);
        trashCountText.text = DataManager.Instance.playerData.weight.ToString("F2") + " kg / 100";
        vacuumCleaner.onCleanAction += () =>
        {
            trashCountText.text = DataManager.Instance.playerData.weight.ToString("F2") + " kg / 100";
        };
    }
}
