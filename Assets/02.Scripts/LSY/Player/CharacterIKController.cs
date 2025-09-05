using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterIKController : MonoBehaviour
{
    Animator animator;
    Transform rightHandTarget = null; // 손이 따라갈 목표 지점
    float rightHandWeight = 0f; // IK 가중치 (0이면 안씀,1이면 완전 사용)

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // IK Pass 켰을때만 유니티가 매프레임 호출
    private void OnAnimatorIK(int layerIndex)
    {
        // 오른손 IK 설정
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandWeight);

        if (rightHandTarget != null)
        {
            // 오른손의 위치와 회전을 타겟에 맞춤
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
        }
    }
    // 외부에서 이 손잡이 잡으라는 함수 호출
    public void SetHandTarget(Transform newTarget)
    {
        rightHandTarget = newTarget;
        StartCoroutine(ChangeWeight(1f, 0.25f));

    }
    // 외부에서 이 손잡이 놓으라는 함수 호출
    public void ClearHandTarget()
    {
        rightHandTarget = null;
        StartCoroutine(ChangeWeight(0f, 0.25f));
    }
    IEnumerator ChangeWeight(float targetWeight, float duration)
    {
        float startWeight = rightHandWeight;
        float time = 0f;
        while (time < duration)
        {
            rightHandWeight = Mathf.Lerp(startWeight, targetWeight, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        rightHandWeight = targetWeight; // Ensure it reaches the target weight
    }
}
