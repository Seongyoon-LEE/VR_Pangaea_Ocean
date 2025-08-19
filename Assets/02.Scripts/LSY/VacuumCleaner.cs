using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class VacuumCleaner : MonoBehaviour
{
    [Header("청소기 발사")]
    [SerializeField] ParticleSystem inhalationPS;
    [SerializeField] LayerMask layerMask; // 레이저가 충돌할 레이어를 지정합니다.
    [SerializeField] Transform shootSource; // 레이저가 발사되는 위치
    [SerializeField] float rayDistance = 10f; // 레이저의 최대 거리
    bool rayActivate = false;

    [SerializeField] InputActionProperty rightTriggerAction; // 오른쪽 트리거 액션

    bool isFiring;

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

            //XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
            //grabInteractable.activated.AddListener((args) =>
            //{
            //    if (inhalationPS != null)
            //        inhalationPS.Play();
            //    print("asdf");
            //    rayActivate = true; // 레이저 활성화
            //});

            //grabInteractable.deactivated.AddListener(x => StopShoot());

        }

        void Update()
        {
            if (isFiring) RaycastCheck();
        }
    public void StopShoot()
    {
        isFiring = false; // 발사 중지
        // 광선 쏘고 나서 바로 파티클 잔여물 사라지게
        if(inhalationPS)
            inhalationPS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);        
    }
    void RaycastCheck()
    {
        if (!shootSource) return;
        if (Physics.Raycast(shootSource.position, shootSource.forward, out var hit, rayDistance, layerMask))
        {
            hit.collider.gameObject.SetActive(false); // 레이저가 충돌한 오브젝트를 비활성화합니다.
        }

    }
}


