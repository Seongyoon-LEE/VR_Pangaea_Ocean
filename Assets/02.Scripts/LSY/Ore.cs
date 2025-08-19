using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    [Header("내구도(2~3)")]
    public int minHits = 2;
    public int maxHits = 3;

    [Header("이펙트")]
    public ParticleSystem hitEffect; // 히트 이펙트
    public ParticleSystem breakEffect; // 파괴 이펙트

    int hitsLeft;
    void Start()
    {
        // 2~3회 랜덤 설정
        hitsLeft = Random.Range(minHits, maxHits + 1);
        print($"Ore HP : {hitsLeft}");
    }
    public void TakeHit()
    {
        hitsLeft--;
        //if(hitEffect != null) hitEffect.Play(); // 히트 이펙트 재생
        //if (hitsLeft <= 0) BreakEffect();
    }
    void BreakEffect()
    {
        //if(breakEffect != null) Instantiate(breakEffect,transform.position, Quaternion.identity);
    }
}
