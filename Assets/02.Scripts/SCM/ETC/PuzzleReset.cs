using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class PuzzleReset : MonoBehaviour
{
    private XRLever lever;
    private Puzzle puzzle;
    void Start()
    {
        lever = GetComponent<XRLever>();
        lever.selectExited.AddListener(x => LightClear());
    }

    void LightClear()
    {
        if (lever.value)
        {

        }
    }
}
