using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OutLineCtrl : MonoBehaviour
{
    public Outline outline;
    void Start()
    {
        outline = GetComponent<Outline>();
    }

    public void OnOutline()
    {
        if(outline != null)
        outline.enabled = true;
    }
    public void OffOutline()
    {
        if (outline != null)
            outline.enabled = false;
    }
}
