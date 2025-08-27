using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GateTrigger : MonoBehaviour
{
    // 유니티 에디터에서 쇠창살 에셋의 부모 오브젝트를 연결합니다.
    public GameObject gateObject;

    // 소켓 인터랙터 컴포넌트를 자동으로 가져올 변수입니다.
    private XRSocketInteractor socketInteractor;

    void Start()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();

        // 소켓에 열쇠가 들어갔을 때(Select Entered) 호출될 함수를 연결합니다.
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.AddListener(OnKeyPlaced);
        }
    }

    private void OnKeyPlaced(SelectEnterEventArgs interactable)
    {
        // 소켓에 들어온 오브젝트가 '열쇠' 태그를 가졌는지 확인합니다.
        if (interactable.interactableObject.transform.gameObject.CompareTag("Key"))
        {
            // 쇠창살 애니메이션 재생 로직
            if (gateObject != null)
            {
                Animator animator = gateObject.GetComponent<Animator>();
                if (animator != null)
                {
                    // "OpenGate" 트리거를 설정하여 애니메이션을 재생합니다.
                    animator.SetTrigger("OpenGate");
                }
            }
        }
    }
}
