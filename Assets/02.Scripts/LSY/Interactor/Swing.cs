using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    [Header("팁(끝부분) 위치")]
    public Transform tip;

    [Header("속도 판정")]
    public float hitSpeedThreshold = 10f; // 이 속도 이상일때만 히트
    public float hitCooldown = 0.15f; // 히트 쿨타임(연타 판정 방지)

    [Header("레이어")]
    public LayerMask hittableLayers;

    [Header("IK 관련")]
    IKGrabbable ikGrabbable;

    Vector3 prevTipPos; // 이전 프레임 팁 위치
    float curSpeed; // 이번 프레임 팁 속도(거리/시간)
    float lastHitTime = -999f;

    private void Awake()
    {
        ikGrabbable = GetComponent<IKGrabbable>();
    }
    private void OnEnable()
    {
        ikGrabbable.Grab();
    }
    private void OnDisable()
    {
        ikGrabbable.Release();
    }
    void Start()
    {
        if (tip == null) tip = this.transform;
        prevTipPos = tip.position;
    }
    void Update()
    {
        SwingSpeed();
        //Debug.Log(curSpeed);
    }

    private void SwingSpeed()
    {
        // 팁 이동속도 계산 = (이번 프레임 -이전 프레임) 거리 / 시간
        Vector3 delta = tip.position - prevTipPos;
        curSpeed = delta.magnitude / Time.deltaTime;
        prevTipPos = tip.position;
    }

    bool CanHitNow()
    {
        if (curSpeed < hitSpeedThreshold) return false; // 속도 미달
        if(Time.time - lastHitTime<hitCooldown) return false; // 쿨다운중
        return true;
    }
    void TryHit(Collider other)
    {
        print("hit" + other.name);
        // 부딪힌 레이어가 때릴수 있는 레이어인지 확인 
        if ((hittableLayers.value & (1 << other.gameObject.layer)) == 0) return;
        // 속도 및 쿨타임 확인
        print(other.gameObject);
        if (!CanHitNow()) return; // 히트 가능 여부 확인

        IHittable hittableObj = other.GetComponent<IHittable>();
        if (hittableObj != null)
        {
            Debug.Log("d");
            hittableObj.TakeHit(this.tip);
            lastHitTime = Time.time; // 히트 시간 갱신
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        TryHit(other);
    }
}
