using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTrashTakeHit : MonoBehaviour
{
    [Header("내구도(2~3)")]
    public int minHits = 1;
    public int maxHits = 3;

    [Header("이펙트")]
    public ParticleSystem hitEffect; // 히트 이펙트
    public ParticleSystem breakEffect; // 파괴 이펙트
    Transform tip; // 곡괭이 끝부분 트랜스폼

    public int hitsLeft;
    void Start()
    {
        //2~3회 랜덤 설정
        hitsLeft = Random.Range(minHits, maxHits + 1);
        tip = GameObject.FindWithTag("Pickaxe").transform;   
    }


    public void PlayParticle()
    {
            var hitFX = Instantiate(hitEffect, tip.position, Quaternion.LookRotation(-tip.forward));
            hitFX.Play(); // 이펙트 재생
            print($"Hits left: {hitsLeft}");
            hitsLeft--;
            //Destroy(hitFX, 1f); // 이펙트 1초 후 제거  
            if (hitsLeft <= 0) BreakEffect(); // 파괴 이펙트 실행
        
    }

    public void BreakEffect()
    {
        var breakFX = Instantiate(breakEffect, transform.position, Quaternion.identity);
        //breakEffect.Play(); // 파괴 이펙트 재생
        Destroy(breakFX,1f); // 오브젝트 파괴
    }
}
