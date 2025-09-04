using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShowCanvas : MonoBehaviour
{
    public GameObject canvas;
    public GameObject ray; // ����ϴ� ���� Ray - �巡�� �� ���
    private Transform head;
    private float spawnDistance = 2f;
    [SerializeField] protected List<GameObject> equipmentsList = new List<GameObject>();
    public Transform equipments;
    protected virtual void Start()
    {
        head = GameObject.Find("XR Origin (XR Rig)").transform.GetChild(0).GetChild(0).transform;
        if (canvas != null) canvas.SetActive(false);
        if (ray != null) ray.SetActive(false);

        SetEquipments();
    }

    private void SetEquipments()
    {
        var leftHand = GameObject.Find("Left Hand Model");

        if (leftHand != null)
            equipmentsList.Add(leftHand);

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
        // ĵ������ �÷��̾� ����ٴϴ� ����
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
    // ĵ������ ���� OnOff
    protected void UIEnable(bool isEnable, bool isLeft = false)
    {
        if (isEnable && isLeft)
            LeftUISetting();

        var allCanvas = GameObject.FindObjectsOfType<ShowCanvas>();
        // ���� ��� �ް� �ִ� �ٸ� ĵ���� ����
        foreach (ShowCanvas c in allCanvas)
        {

            if (c.canvas != null && c.canvas != canvas)
            {
                c.canvas.SetActive(false);
                // ž���߿� UI�� ���� �� ������ ���̴� �ѵα�
                if (c.ray != null) c.ray.SetActive(DataManager.Instance.PlayerData.isBoarding && c.ray.CompareTag("Right") && !isEnable);
            }
        }
        bool curRay = isEnable;
        // ž���߿� �������� ���� UI�� ���� ������ ���� ����
        if (DataManager.Instance.PlayerData.isBoarding && !isEnable && !isLeft)
        {
            curRay = true;
        }

        // ���� ĵ����
        if (canvas != null) canvas.SetActive(isEnable);
        if (ray != null) ray.SetActive(curRay);
    }

    private void LeftUISetting()
    {
        for (int i = 0; i < equipmentsList.Count; i++)
        {
            equipmentsList[i].SetActive(i == 0 || i == 1);
        }
    }

    public void Close()
    {
        UIEnable(false);
    }

    
}
