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

    BreakableTrash breakable;
    public static event Action OnBigTrashBroken;
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
            var hitFX = Instantiate(hitEffect, hitPoint.position, Quaternion.LookRotation(-hitPoint.forward));
            Destroy(hitFX.gameObject, 1f);
        }
    }

    public void BreakEffect()
    {
        var breakFX = Instantiate(breakEffect, transform.position, Quaternion.identity);
        breakEffect.Play(); // 파괴 이펙트 재생
        Destroy(breakFX.gameObject,1f); // 오브젝트 파괴
    }
}
