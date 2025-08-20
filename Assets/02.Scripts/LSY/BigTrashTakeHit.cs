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

    public int hitsLeft;
    void Start()
    {
        //2~3회 랜덤 설정
        hitsLeft = Random.Range(minHits, maxHits + 1);
        //hitEffect.Stop(); // 시작시 이펙트 정지
        //breakEffect.Stop(); // 시작시 이펙트 정지
    }
    private void OnCollisionEnter(Collision c)
    {
        if (c.collider.CompareTag("Pickaxe"))
        {
            var contact = c.contacts[0]; // 충돌 지점
                                         //if (hitEffect)
            var hitFX = Instantiate(hitEffect, contact.point, Quaternion.LookRotation(contact.normal));
            hitFX.Play(); // 이펙트 재생
            print(hitFX);
            print($"Hits left: {hitsLeft}");
            hitsLeft--;
            //Destroy(hitFX, 1f); // 이펙트 1초 후 제거  
            if (hitsLeft <= 0) BreakEffect(); // 파괴 이펙트 실행
        }
    }
    void BreakEffect()
    {
        if(breakEffect != null) Instantiate(breakEffect,transform.position, Quaternion.identity);
        //breakEffect.Play(); // 파괴 이펙트 재생
        Destroy(gameObject); // 오브젝트 파괴
    }
}
