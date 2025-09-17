using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCanvas : MonoBehaviour
{
    protected GameObject canvas;
    public GameObject ray;
    private Transform head; // 카메라 위치
    private float spawnDistance = 2f; // 거리
    protected List<GameObject> equipmentsList = new List<GameObject>(); // 장비
    public Transform equipments; // 장비 그룹 위치 저장
    protected bool isLeft = false;
    protected virtual IEnumerator Start()
    {
        // 로딩 될 때까지 대기
        while (!DataManager.Instance.IsLoadingFinish)
        {
            yield return null;
        }

        head = GameObject.Find("XR Origin (XR Rig)").transform.GetChild(0).GetChild(0).transform;
        if (!isLeft) ray = GameObject.Find("XR Origin (XR Rig)").transform.GetChild(0).GetChild(3).gameObject;
        if (canvas != null) canvas.SetActive(false);
        // 처음 게임 실행했을 때 보트에 탑승중이 아니면 비활성화
        if (ray != null && !DataManager.Instance.PlayerData.isBoarding) ray.SetActive(false);

        SetEquipments();
    }

    // 장비들 가져오기
    private void SetEquipments()
    {
        //var leftHand = GameObject.Find("Left Hand Model");

        //if (leftHand != null)
        //    equipmentsList.Add(leftHand);

        equipments = GameObject.Find("Equipments").transform;

        if (equipments != null)
        {
            for (int i = 0; i < equipments.childCount; i++)
            {
                equipmentsList.Add(equipments.GetChild(i).gameObject);
            }
        }
    }

    protected virtual void FollowUI()
    {
        if (canvas.activeSelf)
        {
            canvas.transform.position = head.position + new Vector3(head.forward.x, head.forward.y, head.forward.z).normalized * spawnDistance;
            Quaternion rot = Quaternion.LookRotation(canvas.transform.position - head.position);
            canvas.transform.rotation = Quaternion.Slerp(canvas.transform.rotation, rot, 3f * Time.deltaTime);
        }
    }

    protected virtual void Update()
    {
        if (canvas != null && head != null)
        {
            FollowUI();
        }
        
    }
    // 캔버스, 레이 OnOff
    protected void UIEnable(bool isEnable, bool isLeft = false)
    {
        bool curRay = isEnable;
        if (DataManager.Instance.PlayerData.isBoarding && transform.GetComponent<EquipmentChange>() != null)
            return;

        // 왼손 UI 활성화시 장비 비활성화
        if (isEnable && isLeft)
            LeftUISetting();

        var allCanvas = GameObject.FindObjectsOfType<ShowCanvas>();
        // 다른 캔버스를 끄기 위한 로직
        foreach (ShowCanvas c in allCanvas)
        {
            if (c.canvas != null && c.canvas != canvas)
            {
                c.canvas.SetActive(false);
                // 보트에 탑승중에 UI를 비활성화하면 Ray를 활성화
                if (c.ray != null)
                {
                    c.ray.SetActive(DataManager.Instance.PlayerData.isBoarding && !isEnable);
                }
            }
        }
      
        // 현재 UI 보트 탑승하고 비활성상태 레이 활성화
        if (DataManager.Instance.PlayerData.isBoarding && !isEnable)
        {
            curRay = true;
        }

        if (canvas != null) canvas.SetActive(isEnable);
        if (ray != null) ray.SetActive(curRay);
    }

    private void LeftUISetting()
    {
        for (int i = 0; i < equipmentsList.Count; i++)
        {
            //equipmentsList[i].SetActive(i == 0 || i == 1);
            equipmentsList[i].SetActive(false);
        }
    }

    public void Close()
    {
        UIEnable(false);
    }

    
}
