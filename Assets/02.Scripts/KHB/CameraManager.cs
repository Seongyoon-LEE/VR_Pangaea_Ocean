using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Underwater mainCamSet;
    private void Awake()
    {
        this.mainCamSet = Camera.main.GetComponent<Underwater>();
    }
    private void Start()
    {
        var camSetting = DataManager.Instance.dicFogConfig[DataManager.Instance.PlayerData.stageIdx];
        
        mainCamSet.minFogDensity = camSetting.minFogDensity;
        mainCamSet.maxFogDensity = camSetting.maxFogDensity;
        mainCamSet.maxDepth = camSetting.maxDepth;
    }
}
