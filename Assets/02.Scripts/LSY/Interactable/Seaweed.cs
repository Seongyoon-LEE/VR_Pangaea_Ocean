using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seaweed : MonoBehaviour, IHittable
{
    //[SerializeField] ParticleSystem cutEffect; // 해초가 베일때 이펙트
    
    public void TakeHit(Transform hitPoint)
    {
        print("해초가 베였다 !!");
        
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

            // 해초는 한방에 베이니까 바로 자기 자신 파괴 
            if (TryGetComponent<TrashData>(out TrashData trash))
            trash.DisActivate();
        // 풀 사라지고 거북이 구출되며 스탯 증가 로직 
        if (TryGetComponent<Turtle>(out Turtle turtle))
            turtle.GrassDisable();
    }
}
