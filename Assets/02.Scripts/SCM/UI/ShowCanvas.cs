using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCanvas : MonoBehaviour
{
    public GameObject canvas; // 드래그 앤 드롭
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

    // 캔버스랑 레이 OnOff
    protected void UIEnable(bool isEnable)
    {
        canvas.SetActive(isEnable);

        if (isEnable)
            CanvasOnOff();
        // 다른 캔버스에서 레이 끌 때 같이 꺼지는 것을 방지하기 위해 마지막에 실행
        ray.SetActive(isEnable);
    }

    // 다른 캔버스 종료하는 로직
    private void CanvasOnOff()
    {
        var allCanvas = GameObject.FindObjectsOfType<ShowCanvas>();
        // 현재 상속 받고 있는 캔버스와 다른 캔버스 종료
        foreach (ShowCanvas c in allCanvas)
        {
            if (c.canvas != canvas)
                c.UIEnable(false);
        }
    }
    public void Close()
    {
        UIEnable(false);
    }

    
}
