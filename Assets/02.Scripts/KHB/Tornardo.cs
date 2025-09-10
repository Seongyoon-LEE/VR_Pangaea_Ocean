using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornardo : MonoBehaviour
{
    public float RotateSpeed = 300f;

    public float pullRadius = 500f; // 회오리가 물체를 끌어당기는 반경

    private float currentPullForce = 300f; //현재 당기는 힘


    public void SetPullForce(float force)
    {
        currentPullForce = Mathf.Abs(force); 
        //절대값으로 지정
        
    }

    void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * RotateSpeed * Time.deltaTime);
        // 당기는 힘이 없으면 아무것도 안함
        if (currentPullForce <= 0) return;

        // 회오리 주변의 모든 콜라이더를 찾음
        Collider[] colliders = Physics.OverlapSphere(transform.position, pullRadius);



        foreach (Collider hit in colliders)
        {
            
                //회오리 오브젝트 자신은 힘을 받지 않도록 스킵
                if (hit.transform == transform) continue;

            if (hit.CompareTag("Player"))
            {
                // 리지드바디가 있는 오브젝트만 힘을 받음
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // 회오리 중심으로 향하는 방향 벡터
                    Vector3 direction = transform.position - hit.transform.position;

                    // 이 방향으로 힘을 적용
                    rb.AddForce(direction.normalized * currentPullForce, ForceMode.Force);
                }
            }
        }
        
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow; // 기즈모 색상을 노란색으로 설정
        Gizmos.DrawSphere(transform.position, pullRadius); // pullRadius 크기의 구를 그림
    }
}
