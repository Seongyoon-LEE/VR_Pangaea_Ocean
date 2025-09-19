using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTrashTakeHit : MonoBehaviour,IHittable
{
    [Header("내구도(2~3)")]
    public int minHits = 1;
    public int maxHits = 3;

    [Header("이펙트")]
    public ParticleSystem hitEffect; // 히트 이펙트
    public ParticleSystem breakEffect; // 파괴 이펙트

    [Header("사운드")]
    public AudioClip hitSound; // 히트 사운드
    public AudioClip breakSound; // 파괴 사운드

    BreakableTrash breakable;
    public event Action OnBigTrashBroken;
    public int hitsLeft;

    void Start()
    {
        //2~3회 랜덤 설정
        hitsLeft = UnityEngine.Random.Range(minHits, maxHits + 1);
        breakable = GetComponent<BreakableTrash>();
    }
    public void TakeHit(Transform hitPoint)
    {
        
        hitsLeft--;
        print($"BigTrash HitLeft : {hitsLeft}");

        if (hitsLeft <= 0)
        {
            if (breakable != null)
            {
                breakable.Break();
            }
            BreakEffect(); // 파괴 이펙트 함수
            OnBigTrashBroken.Invoke(); // 큰쓰레기 파괴 이벤트
        }
        else
        {
            SoundManager.s_Instance.PlaySfx(transform.position,hitSound,false);
            // 이펙트 풀링에서 hit 이펙트 꺼내옴
            GameObject hitFX = EffectPoolingManager.Instance.GetFromPool(
                EffectPoolingManager.Instance.hitEffectPoolList,
                EffectPoolingManager.Instance.hitEffectPrefab);
            if (hitFX != null)
            {
                // 위치와 방향 설정
                hitFX.transform.position = hitPoint.position;
                hitFX.transform.rotation = Quaternion.LookRotation(-hitPoint.forward);
                hitFX.SetActive(true); // 이펙트 활성화
            }
        }
    }

    public void BreakEffect()
    {
        SoundManager.s_Instance.PlaySfx(transform.position, breakSound, false);
        GameObject breakFX = EffectPoolingManager.Instance.GetFromPool(
            EffectPoolingManager.Instance.breakEffectPoolList,
            EffectPoolingManager.Instance.breakEffectPrefab);
        if (breakFX != null)
        {
            breakFX.transform.position = transform.position;
            breakFX.transform.rotation = Quaternion.identity;
            breakFX.SetActive(true);
        }
    }
}
