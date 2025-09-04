using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TrashCount : MonoBehaviour
{
    [SerializeField] TMP_Text trashCountText;
    [SerializeField] VacuumCleaner vacuumCleaner;
    [SerializeField] Garbage garbageBox;

    IEnumerator Start()
    {
        while (!DataManager.Instance.IsLoadingFinish)
        {
            yield return null;
        }

        if (trashCountText == null)
            trashCountText = GameObject.Find("StateMenu").transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_Text>();

        if (vacuumCleaner == null)
            vacuumCleaner = FindObjectOfType<VacuumCleaner>(true);

        if (garbageBox == null)
            garbageBox = FindObjectOfType<Garbage>(true);

        trashCountText.text = DataManager.Instance.PlayerData.weight.ToString("F2") + " kg / 300 kg";


        vacuumCleaner.onCleanAction += () =>
        {
            
            trashCountText.text = DataManager.Instance.PlayerData.weight.ToString("F2") + " kg / 300 kg";
            if(DataManager.Instance.PlayerData.weight >= 300)
            {
                trashCountText.color = Color.red;
            }
        };
        garbageBox.onTrashSubmitted += () =>
        {
            trashCountText.text = DataManager.Instance.PlayerData.weight.ToString("F2") + " kg / 300 kg";
            trashCountText.color = Color.white;
        };

    }
}
