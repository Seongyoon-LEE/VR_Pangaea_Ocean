using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seaweed : MonoBehaviour, IHittable
{
    [SerializeField] ParticleSystem cutEffect; // 해초가 베일때 이펙트
    
    public void TakeHit(Transform hitPoint)
    {
        print("해초가 베였다 !!");
        if (cutEffect != null)
        {
            // 해초 베이는 이펙트 생성 
            var cutFX = Instantiate(cutEffect,hitPoint.position,Quaternion.LookRotation(-hitPoint.forward));
            Destroy(cutFX.gameObject, 1f);
        }
        // 해초는 한방에 베이니까 바로 자기 자신 파괴 
        if (TryGetComponent<TrashData>(out TrashData trash))
            trash.DisActivate();
        // 풀 사라지고 거북이 구출되며 스탯 증가 로직 
        if (TryGetComponent<Turtle>(out Turtle turtle))
            turtle.GrassDisable();
    }
}
