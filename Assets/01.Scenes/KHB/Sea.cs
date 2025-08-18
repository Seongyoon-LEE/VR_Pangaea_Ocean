using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Sea : MonoBehaviour
{

    public Transform mainCamera;
    public int seaLevel = 0;
    public Volume Volume;

    public VolumeProfile surface;
    public VolumeProfile underwater;
    void Update()
    {
        if(mainCamera.position.y < seaLevel)
        {
            effectEnable(true);
        }
        else
        {
            effectEnable(false);
        }
    }

    void effectEnable(bool activate)
    {
        if (activate)
        {
            Volume.profile = underwater;
        }
        else
        {
            Volume.profile = surface;
        }
    }
}
