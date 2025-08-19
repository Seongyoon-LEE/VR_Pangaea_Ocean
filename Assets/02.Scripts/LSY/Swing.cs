using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    [Header("팁(끝부분) 위치")]
    public Transform tip;

    [Header("속도 판정")]
    public float hitSpeedThreshold = 2.5f; // 이 속도 이상일때만 히트
    public float hitCooldown = 0.15f; // 히트 쿨타임(연타 판정 방지)

    [Header("태그")]
    public string oreTag = "Ore"; // 광석 태그

    Vector3 prevTipPos; // 이전 프레임 팁 위치
    float curSpeed; // 이번 프레임 팁 속도(거리/시간)
    float lastHitTime = -999f;

    void Start()
    {
        if (tip == null) tip = this.transform;
        prevTipPos = tip.position;
    }

    void Update()
    {
        // 팁 이동속도 계산 = (이번 프레임 -이전 프레임) 거리 / 시간
        Vector3 delta = tip.position - prevTipPos;
        curSpeed = delta.magnitude / Time.deltaTime;
        prevTipPos = tip.position;
        print($"speed : {curSpeed:F2} m/s");
    }
    bool CanHitNow()
    {
        if (curSpeed < hitSpeedThreshold) return false; // 속도 미달
        if(Time.time - lastHitTime<hitCooldown) return false; // 쿨다운중
        return true;
    }
    void TryHit(Collider other)
    {
        if (!string.IsNullOrEmpty(oreTag))
        {
            if(!other.CompareTag(oreTag)) return; // 태그가 ore가 아니면 리턴
        }
        if(!CanHitNow()) return; // 히트 가능 여부 확인
        
        // 광석에 히트 전달
        Ore ore = other.GetComponent<Ore>();
        if(ore != null)
        {
            //ore.takeHit();
            lastHitTime = Time.time; // 히트 시간 갱신
            //이펙트/사운드 호출
        }
    }
    private void OnCollisionEnter(Collision c)
    {
        TryHit(c.collider);
    }
}
