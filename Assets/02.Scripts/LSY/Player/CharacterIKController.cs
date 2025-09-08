using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterIKController : MonoBehaviour
{
    Animator animator;
    Transform rightHandTarget = null; // 손이 따라갈 목표 지점
    Transform leftHandTarget = null; // 왼손이 따라갈 목표 지점

    float leftHandWeight = 0f; // IK 가중치 (0이면 안씀,1이면 완전 사용)
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

        // 왼손 IK 설정
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeight);
        if (rightHandTarget != null)
        {
            // 오른손의 위치와 회전을 타겟에 맞춤
            print("as");
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
        }
        if (leftHandTarget != null)
        {
            // 왼손의 위치와 회전을 타겟에 맞춤
            print("왼손");
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
        }
    }
    // 외부에서 이 손잡이 잡으라는 함수 호출
    public void SetHandTarget(AvatarIKGoal hand, Transform newTarget)
    {
        if(hand == AvatarIKGoal.RightHand)
            rightHandTarget = newTarget;
        else if(hand == AvatarIKGoal.LeftHand)
            leftHandTarget = newTarget;
        StartCoroutine(ChangeWeight(hand, 1f, 0.25f));
    }
    // 외부에서 이 손잡이 놓으라는 함수 호출
    public void ClearHandTarget(AvatarIKGoal hand)
    {
        if(hand == AvatarIKGoal.RightHand)
            rightHandTarget = null;
        else if (hand == AvatarIKGoal.LeftHand)
            leftHandTarget = null;
       
        StartCoroutine(ChangeWeight(hand, 0f, 0.25f));
    }
    IEnumerator ChangeWeight(AvatarIKGoal hand, float targetWeight, float duration)
    {
        float startWeight = (hand == AvatarIKGoal.RightHand) ? rightHandWeight : leftHandWeight;
        float time = 0f;

        while (time < duration)
        {
            float currentWeight = Mathf.Lerp(startWeight, targetWeight, time / duration);

            if (hand == AvatarIKGoal.RightHand)
                rightHandWeight = currentWeight;
            else if (hand == AvatarIKGoal.LeftHand)
                leftHandWeight = currentWeight;

            time += Time.deltaTime;
            yield return null;
        }
        if (hand == AvatarIKGoal.RightHand)
        {
            rightHandWeight = targetWeight;
            if (targetWeight == 0f)
                rightHandTarget = null; // 타겟을 null로 설정하여 손을 원래 위치로 돌아가게 함
        }
        else if (hand == AvatarIKGoal.LeftHand)
        {
            leftHandWeight = targetWeight;
            if (targetWeight == 0f)
                leftHandTarget = null; // 타겟을 null로 설정하여 손을 원래 위치로 돌아가게 함
        }
    }
}
