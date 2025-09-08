using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    private GameObject canvas;
    private Transform head;
    private float spawnDistance = 2f;

    void  Start()
    {
        canvas = transform.gameObject;
        head = GameObject.Find("XR Origin (XR Rig)").transform.GetChild(0).GetChild(0).transform;

        UIEnable();
    }

    private void UIEnable()
    {
        canvas.transform.position = head.position + new Vector3(head.forward.x, head.forward.y, head.forward.z).normalized * spawnDistance;
        Quaternion rot = Quaternion.LookRotation(canvas.transform.position - head.position);
        canvas.transform.rotation = Quaternion.Slerp(canvas.transform.rotation, rot, 3f * Time.deltaTime);
    }
    
}
