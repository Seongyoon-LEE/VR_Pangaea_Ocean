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

    SharkPatrol shark;

    readonly int dieHash = Animator.StringToHash("Die");
    void Awake()
    {
        if(animator == null)
        animator = GetComponent<Animator>();
        this.shark = gameObject.GetComponent<SharkPatrol>();
    }
    private void Start()
    {
        curentHp = maxHp;
    }
    void Die()
    {
        print("상어 죽음");
        animator.SetTrigger(dieHash);
        this.shark.Info.active = false;
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

        GameObject slashFX = EffectPoolingManager.Instance.GetFromPool(
            EffectPoolingManager.Instance.slashEffectPoolList,
            EffectPoolingManager.Instance.slashEffectPrefab);
        if (slashFX != null)
        {
            // 위치와 방향 설정
            slashFX.transform.position = hitPoint.position;
            slashFX.transform.rotation = Quaternion.LookRotation(-hitPoint.forward);
            slashFX.SetActive(true); // 이펙트 활성화
        }

        if (curentHp <= 0)
        {
            Die();
        }
    }
}
