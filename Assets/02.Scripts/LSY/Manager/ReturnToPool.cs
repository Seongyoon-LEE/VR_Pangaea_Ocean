using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPool : MonoBehaviour
{
    [SerializeField] float lifeTime = 2f;

    private void OnEnable()
    {
        Invoke("Deactivate", lifeTime); // 2초 후에 Deactivate 함수 호출
    }
    void Deactivate()
    {
        gameObject.SetActive(false);
    }
    void OnDisable()
    {
        CancelInvoke(); // 오브젝트가 비활성화될 때 호출 예약 취소
    }
}
