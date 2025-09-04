using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class VacuumCleaner : MonoBehaviour
{
    [Header("청소기 관련")]
    [SerializeField] ParticleSystem inhalationPS;
    [SerializeField] LayerMask layerMask; // 레이저가 충돌할 레이어를 지정합니다.
    [SerializeField] Transform FirePos; // 레이저가 발사되는 위치
    [SerializeField] float rayDistance = 10f; // 레이저의 최대 거리
    [SerializeField] InputActionProperty rightTriggerAction; // 오른쪽 트리거 액션
    bool isFiring;
    public Action onCleanAction;

    [Header("IK 관련")]
    public Transform rightHandGrip; // 손이 따라갈 목표 지점
    CharacterIKController characterIK; // 캐릭터 IK 컨트롤러

    private void OnEnable()
    {
        if (rightTriggerAction != null && rightTriggerAction.action != null)
        {
            rightTriggerAction.action.performed += OnTriggerPressed;
            rightTriggerAction.action.canceled += OnTriggerReleased;
        }
    }
        void OnDisable()
        {
            if (rightTriggerAction != null && rightTriggerAction.action != null)
            {
                rightTriggerAction.action.performed -= OnTriggerPressed;
                rightTriggerAction.action.canceled -= OnTriggerReleased;
            }
    }
    void OnTriggerPressed(InputAction.CallbackContext ctx)
    {
        // 시작하기전에 무게가 300이 넘으면 발사 안됨
        if (DataManager.Instance.PlayerData.weight >= 300)
        {
            StopShoot();
            return;
        }

        if (inhalationPS != null && !isFiring)
        {
            inhalationPS.Play();
            isFiring = true;
        }
    }
        void OnTriggerReleased(InputAction.CallbackContext ctx)
        {
            StopShoot();
        }
        void Start()
        {
            if (inhalationPS != null)
                inhalationPS.Stop();
            print("Stop");
        FirePos = transform.GetChild(2);
        characterIK = FindObjectOfType<CharacterIKController>();
    }

    void Update()
    {
        if (isFiring) RaycastCheck();
        inhalationPS.Simulate(10, true, false); // 파티클 시스템을 시뮬레이션합니다.
    }
    // 아이템을 장착 했을때 호출되는 함수
    public void OnEquip()
    {
        // 캐릭터에게 손잡이 잡으라고 명령
        if (characterIK != null && rightHandGrip != null)
            characterIK.SetHandTarget(rightHandGrip);
    }
    // 아이템을 해재 했을때 호출되는 함수
    public void OnUnEquip()
    {
        // 캐릭터에게 손잡이 놓으라고 명령
        if (characterIK != null)
            characterIK.ClearHandTarget();
        StopShoot();
    }
    public void StopShoot()
    {
        isFiring = false; // 발사 중지
        // 광선 쏘고 나서 바로 파티클 잔여물 사라지게
        if (inhalationPS)
            inhalationPS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);        
    }
    void RaycastCheck()
    {
        if (!FirePos) return;
        if (Physics.Raycast(FirePos.position, FirePos.forward, out var hit, rayDistance, layerMask))
        {
            Debug.DrawLine(FirePos.position, hit.point, Color.red); // 레이저가 충돌한 지점을 시각적으로 표시합니다.
            hit.collider.GetComponent<TrashData>().Clean();
            hit.collider.GetComponent<TrashData>().DisActivate(); // 레이저가 충돌한 오브젝트를 비활성화합니다.
            DataManager.Instance.PlayerData.weight += hit.collider.GetComponent<TrashData>().Info.weight;
            if(DataManager.Instance.PlayerData.weight >= 300)
            {
                StopShoot();
            }
            onCleanAction();
        }
    }
}


