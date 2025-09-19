using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [Header("스크립트 참조")]
    [SerializeField] SwimMovement swimMovement;
    [SerializeField] Transform bubbleSpawnPoint;

    [SerializeField] AudioClip bubbleClip; // 거품 사운드

    [SerializeField] float bubbleInterval = 10f; // 거품 생성 간격
    void Start()
    {
        swimMovement = GetComponent<SwimMovement>();
        if(bubbleSpawnPoint != null )
        bubbleSpawnPoint = transform.GetChild(2).GetComponent<Transform>();
        // 10초마다 거품 생성 코루틴
        StartCoroutine(SpawnBubblesRoutine());
    }
    IEnumerator SpawnBubblesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(bubbleInterval);
            SpawnBubble();
        }
    }
    void SpawnBubble()
    {
        if (swimMovement != null && swimMovement.IsUnderwater)
        {
            SoundManager.s_Instance.PlaySfx(transform.position,bubbleClip, false);
            // 이펙트 풀링에서 거품 이펙트 가져오기
            var bubbleEffect = EffectPoolingManager.Instance.GetFromPool(
                EffectPoolingManager.Instance.bubbleEffectPoolList,
                EffectPoolingManager.Instance.bubbleEffectPrefab);

            if(bubbleEffect != null)
            {
                bubbleEffect.transform.position = bubbleSpawnPoint.position;
                bubbleEffect.SetActive(true);
            }
        }
    }
}
