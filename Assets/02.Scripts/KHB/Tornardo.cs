using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornardo : ObstacleData
{
    public Transform tornardo;


    //회오리 회전 및 크기 관련 변수
    public float scaleSpeed = 5f;
    public float rotateSpeed = 300f; // 회전 속도
    public Vector3 targetScale = new Vector3(10, 10, 10);

    //끌어당기는 힘 관련 변수
    public float pullRadius = 300f;    // 물체를 끌어당기는 반경
    public float minPullForce = 100f;  // 최소 당기는 힘 (작을 때)
    public float maxPullForce = 500f;  // 최대 당기는 힘 (클 때)
    void Update()
    {
        tornardoScaleUp();
        TornadoRotate();
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

    private void TornadoRotate()
    {
        tornardo.Rotate(Vector3.up,rotateSpeed * Time.deltaTime);
    }
    void FixedUpdate()
    {
        // 1. 오브젝트의 크기를 부드럽게 키웁니다.
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);

        // 2. 현재 크기에 비례하여 당기는 힘을 계산합니다.
        float currentScaleRatio = transform.localScale.magnitude / targetScale.magnitude;
        float currentPullForce = Mathf.Lerp(minPullForce, maxPullForce, currentScaleRatio);

        // 3. 당기는 힘이 없으면 아무것도 하지 않습니다.
        if (currentPullForce <= 0) return;

        // 4. 회오리 주변의 모든 콜라이더를 찾습니다.
        Collider[] colliders = Physics.OverlapSphere(transform.position, pullRadius);

        // 5. 콜라이더들을 하나씩 검사합니다.
        foreach (Collider hit in colliders)
        {
            // 회오리 오브젝트 자신은 힘을 받지 않도록 스킵
            if (hit.transform == transform) continue;

            // "Player" 태그를 가진 오브젝트만 대상으로 힘을 적용합니다.
            if (hit.CompareTag("Player"))
            {
                // 리지드바디가 있는 오브젝트만 힘을 받습니다.
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // 회오리 중심으로 향하는 방향 벡터
                    Vector3 direction = transform.position - hit.transform.position;

                    // 현재 크기에 따라 계산된 힘을 적용
                    rb.AddForce(direction.normalized * currentPullForce, ForceMode.Force);
                }

            }
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.state = GameManager.State.TORNADO;
        }
    }
    private void OnCollisionExit(Collision collision) 
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.state = GameManager.State.NORMAL;
        }
    }
    void OnDrawGizmosSelected()
    {
        // 유니티 에디터에서 회오리의 영향 범위를 시각적으로 보여줍니다.
        Gizmos.color = Color.yellow; // 기즈모 색상을 노란색으로 설정
        Gizmos.DrawWireSphere(transform.position, pullRadius); // pullRadius 크기의 구를 그림
    }
}

