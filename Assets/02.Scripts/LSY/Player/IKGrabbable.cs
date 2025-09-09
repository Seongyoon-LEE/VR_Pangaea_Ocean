using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKGrabbable : MonoBehaviour
{
    [Header("IK 손잡이 위치")]
    public Transform rightHandGrip; // 손이 따라갈 목표 지점
    public Transform leftHandGrip; // 왼손이 따라갈 목표 지점
    [SerializeField] CharacterIKController characterIK; // 캐릭터 IK 컨트롤러

    Animator animator;
    
    public void Grab()
    {
        if(characterIK == null) return;

        if (rightHandGrip != null)
        {
            characterIK.SetHandTarget(AvatarIKGoal.RightHand, rightHandGrip); // 오른손 IK 설정
        }
        if (leftHandGrip != null)
        {
            characterIK.SetHandTarget(AvatarIKGoal.LeftHand, leftHandGrip); // 왼손 IK 설정
        }
    }
    public void Release()
    {
        if (characterIK == null) return;
        if (rightHandGrip != null)
        {
            characterIK.ClearHandTarget(AvatarIKGoal.RightHand); // 오른손 IK 해제
        }
        if (leftHandGrip != null)
        {
            characterIK.ClearHandTarget(AvatarIKGoal.LeftHand); // 왼손 IK 해제
        }
    }
}
