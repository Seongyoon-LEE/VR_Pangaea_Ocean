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
        if (canvas != null) canvas.SetActive(false);
        if (ray != null) ray.SetActive(false);
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
        var allCanvas = GameObject.FindObjectsOfType<ShowCanvas>();
        // 현재 상속 받고 있는 다른 캔버스 종료
        foreach (ShowCanvas c in allCanvas)
        {
            if (c.canvas != canvas)
            {
                c.canvas.SetActive(false);
                // 탑승중에 UI가 꺼질 때 오른쪽 레이는 켜두기
                c.ray.SetActive(DataManager.Instance.playerData.isBoarding && c.ray.CompareTag("Right") && !isEnable);
            }
        }
        bool curRay = isEnable;
        // 탑승중에 오른손이 현재 UI와 같이 꺼지는 것을 막기
        if (DataManager.Instance.playerData.isBoarding && !isEnable && !isLeft)
        {
            curRay = true;
        }

        // 현재 캔버스
        canvas.SetActive(isEnable);
        ray.SetActive(curRay);
    }
    public void Close()
    {
        UIEnable(false);
    }

    
}
