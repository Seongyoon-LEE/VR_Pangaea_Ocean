using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuEnable : MonoBehaviour
{
    public InputActionProperty mainInput; // 사용할 손 - 드래그 앤 드롭
    public InputActionProperty subInput; // 사용하지 않는 손 - 드래그 앤 드롭
    private GameObject canvas;
    private Transform head;
    public GameObject ray; // 사용하는 방향 Ray - 드래그 앤 드롭
    private float spawnDistance = 2f;
    void Start()
    {
        canvas = transform.GetChild(0).gameObject;
        head = GameObject.Find("XR Origin (XR Rig)").transform.GetChild(0).GetChild(0).transform;
        canvas.SetActive(false);
        ray.SetActive(false);
        
    }

    private void OnEnable()
    {
        mainInput.action.started += x => UIEnable();
    }
    private void OnDisable()
    {
        mainInput.action.started -= x => UIEnable();
        mainInput.action.Disable();
    }
    void Update()
    {
        // 캔더스가 플레이어 따라다니는 로직
        if (canvas.activeSelf)
        {
            canvas.transform.position = head.position + new Vector3(head.forward.x, head.forward.y, head.forward.z).normalized * spawnDistance;
            Quaternion rot = Quaternion.LookRotation(canvas.transform.position - head.position);
            canvas.transform.rotation = Quaternion.Slerp(canvas.transform.rotation, rot, 3f * Time.deltaTime);

            // 다른 메뉴버튼 누르면 사라지게하기
            if (subInput.action.WasPressedThisFrame())
                UIEnable();
        }
    }

    // 캔버스랑 레이 제거
    private void UIEnable()
    {
        canvas.SetActive(!canvas.activeSelf);
        ray.SetActive(!ray.activeSelf);
    }

    // 버튼 or 외부에서 사용
    public void Close()
    {
        UIEnable();
    }
}
