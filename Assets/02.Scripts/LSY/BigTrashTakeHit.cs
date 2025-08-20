using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTrashTakeHit : MonoBehaviour
{
    [Header("내구도(2~3)")]
    public int minHits = 1;
    public int maxHits = 3;

    //[Header("이펙트")]
    //public ParticleSystem hitEffect; // 히트 이펙트
    //public ParticleSystem breakEffect; // 파괴 이펙트

    public int hitsLeft;
    void Start()
    {
        // 2~3회 랜덤 설정
        hitsLeft = Random.Range(minHits, maxHits + 1);
    }
    public void TakeHit()
    {
        hitsLeft--;
        print(hitsLeft);
        //if(hitEffect != null) hitEffect.Play(); // 히트 이펙트 재생
        //if (hitsLeft <= 0) BreakEffect();
    }
    void BreakEffect()
    {
        //if(breakEffect != null) Instantiate(breakEffect,transform.position, Quaternion.identity);
    }
}
