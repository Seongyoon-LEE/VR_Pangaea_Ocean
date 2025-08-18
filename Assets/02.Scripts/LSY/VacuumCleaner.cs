using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VacuumCleaner : MonoBehaviour
{
    [SerializeField] ParticleSystem inhalationPS;
    [SerializeField] LayerMask layerMask; // 레이저가 충돌할 레이어를 지정합니다.
    [SerializeField] Transform shootSource; // 레이저가 발사되는 위치
    [SerializeField] float rayDistance = 10f; // 레이저의 최대 거리
    bool rayActivate  = false;
    void Start()
    {
      XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.activated.AddListener((args) =>
        {
            if (inhalationPS != null)
                inhalationPS.Play();
            rayActivate = true; // 레이저 활성화
        });

        grabInteractable.deactivated.AddListener(x => StopShoot());
        
    }

    void Update()
    {
        if (rayActivate)
            RaycastCheck();
    }
    public void StopShoot()
    {
        // 광선 쏘고 나서 바로 파티클 잔여물 사라지게
        inhalationPS.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
        rayActivate = false; // 레이저 비활성화
    }
    void RaycastCheck()
    {
        RaycastHit hit;
        bool hasHit = Physics.Raycast(shootSource.position, shootSource.forward, out hit, rayDistance, layerMask);
        if (hasHit)
        {
            hit.collider.gameObject.SendMessage("Break", SendMessageOptions.DontRequireReceiver);
        }
    }
}
