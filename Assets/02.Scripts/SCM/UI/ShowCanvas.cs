using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShowCanvas : MonoBehaviour
{
    public GameObject canvas;
    public GameObject ray; // 사용하는 방향 Ray - 드래그 앤 드롭
    private Transform head;
    private float spawnDistance = 2f;

    protected virtual void Start()
    {
        head = GameObject.Find("XR Origin (XR Rig)").transform.GetChild(0).GetChild(0).transform;
        canvas.SetActive(false);
        ray.SetActive(false);
    }

    

    protected void FollowUI()
    {
        // 캔더스가 플레이어 따라다니는 로직
        if (canvas.activeSelf)
        {
            canvas.transform.position = head.position + new Vector3(head.forward.x, head.forward.y, head.forward.z).normalized * spawnDistance;
            Quaternion rot = Quaternion.LookRotation(canvas.transform.position - head.position);
            canvas.transform.rotation = Quaternion.Slerp(canvas.transform.rotation, rot, 3f * Time.deltaTime);
        }
    }

    protected virtual void Update()
    {
        if (canvas != null)
        {
            FollowUI();
        }
        
    }
    // 캔버스랑 레이 OnOff
    protected void UIEnable(bool isEnable, bool isLeft = false)
    {
        CanvasOnOff(isEnable, isLeft);

        //if (isEnable)
        //{ 
        //    CanvasOnOff(isLeft);
        //}
        //canvas.SetActive(isEnable);
        //ray.SetActive(isEnable);
    }

    // 다른 캔버스 종료하는 로직
    private void CanvasOnOff(bool isEnable, bool isLeft)
    {
        var allCanvas = GameObject.FindObjectsOfType<ShowCanvas>();
        // 현재 상속 받고 있는 다른 캔버스 종료
        foreach (ShowCanvas c in allCanvas)
        {
            if (c.canvas != canvas)
            {
                //c.UIEnable(false);
                c.canvas.SetActive(false);
                if (DataManager.Instance.playerData.isBoarding && isLeft && !isEnable)
                {
                    c.ray.SetActive(c.ray.CompareTag("Right"));
                }
                else
                {
                    c.ray.SetActive(false);
                }
            }
        }
        // 현재 캔버스
        canvas.SetActive(isEnable);
        ray.SetActive(isEnable);
    }
    public void Close()
    {
        UIEnable(false);
    }

    
}
