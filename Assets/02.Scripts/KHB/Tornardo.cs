using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornardo : MonoBehaviour
{
    public Transform tornardo;
    public Transform playerTr;

    public float scaleSpeed = 10f;

    public BoxCollider boxCollider;

    void Update()
    {
        boxCollider = GetComponent<BoxCollider>();
        tornardoScaleUp();
        boxColliderScale();
    }

    private void boxColliderScale()
    {
        // 1. 스케일 증가
        Vector3 scaleIncrease = Vector3.one * scaleSpeed * Time.deltaTime;
        transform.localScale += scaleIncrease;

        // 2. 콜라이더의 크기 조정
        if (boxCollider != null)
        {
            // 콜라이더의 크기를 현재 transform.localScale에 맞춰 조정
            // boxCollider.size.y를 직접 수정하기 어려우므로, 새로운 벡터를 만듭니다.
            Vector3 newSize = new Vector3(boxCollider.size.x, boxCollider.size.y * (1 + scaleIncrease.z), boxCollider.size.y);
            boxCollider.size = newSize;

            // 3. 오브젝트의 위치 조정
            // 오브젝트의 피봇(pivot)이 중앙에 있으므로, 스케일이 커지는 만큼 y축 위치를 올려서 땅 위에 유지
            transform.position += Vector3.up * scaleIncrease.z * 0.5f;
        }
    }

    private void tornardoScaleUp()
    {
        // 현재 오브젝트의 스케일 가져오기
        Vector3 currentScale = transform.localScale;

        // 시간에 비례하여 스케일 증가량 계산
        // Time.deltaTime을 곱하여 프레임 속도에 관계없이 동일하게 커지도록 합니다.
        Vector3 scaleIncrease = Vector3.one * scaleSpeed * Time.deltaTime;

        // 현재 스케일에 증가량을 더해 스케일 업데이트
        transform.localScale += scaleIncrease;
    }

    private void TornadoDistance()
    {
        float distance = Vector3.Distance(tornardo.position, playerTr.position);

    }
    void TornadoRotate()
    {
        
    }
}
