using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class HitShark : MonoBehaviour,IHittable
{
    [Header("상어 체력")]
    [SerializeField] int maxHp = 5;
    [SerializeField] int curentHp;

    [Header("애님 & 이펙트")]
    Animator animator;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] float deathAnimDuration = 2f;

    readonly int dieHash = Animator.StringToHash("Die");
    void Awake()
    {
        if(animator == null)
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        curentHp = maxHp;
    }
    void Die()
    {
        print("상어 죽음");
        animator.SetTrigger(dieHash);
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
    }

    void IHittable.TakeHit(Transform hitPoint)
    {
        if (curentHp <= 0) return; // 이미 죽었다면 리턴

        // 체력 감소
        curentHp--;
        print($"상어 피격! 남은 체력 : {curentHp}/{maxHp}");

        // 피격 이펙트 재생
        if (hitEffect != null)
        {
            var effect = Instantiate(hitEffect, hitPoint.position, Quaternion.identity);
            Destroy(effect.gameObject, 1f);
        }

        if (curentHp <= 0)
        {
            Die();
        }
    }
}
